using System;
using AdPang.FileManager.Application_WPF.Extensions;
using AdPang.FileManager.Application_WPF.Services.IServices;
using AdPang.FileManager.Shared.Dtos.SystemCommon;
using HttpRequestClient.Services.IRequestServices;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace AdPang.FileManager.Application_WPF.ViewModels
{
    public class LoginViewModel : BindableBase, IDialogAware
    {
        private readonly IAuthModel authModel;
        private readonly IAuthRequestService authRequestService;
        private readonly IEventAggregator eventAggregator;

        public string Title { get; set; } = "文件管理器-登录";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }
        #region LogicBusiness

        public LoginViewModel(IAuthModel authModel, IAuthRequestService authRequestService, IEventAggregator eventAggregator)
        {
            ExecuteCommand = new DelegateCommand<string>(Execute);
            this.authModel = authModel;
            this.authRequestService = authRequestService;
            this.eventAggregator = eventAggregator;
            verfiyCodeImgUrl = authRequestService.GetVerfiyCodeImgUrl(seed);
        }

        private void Execute(string arg)
        {
            switch (arg)
            {
                case "Login": Login(); break;
                case "LoginOut": LoginOut(); break;
                case "GoRegister": SelectedIndex = 1; break; //跳转注册页面
                case "Register": Register(); break;          //注册账号
                case "Return": SelectedIndex = 0; break;   //返回登陆页面
                case "RefreshVerfiyCode":
                    seed = Guid.NewGuid();
                    VerfiyCodeImgUrl = authRequestService.GetVerfiyCodeImgUrl(seed);
                    break;
            }
        }

        private async void Register()
        {
            if (string.IsNullOrWhiteSpace(UserDto.UserName)
                || string.IsNullOrWhiteSpace(UserDto.Password)
                || string.IsNullOrWhiteSpace(UserDto.NewPassword))
            {
                eventAggregator.SendMessage("输入不能为空！", "Login");
                //填入参数为null..
                return;
            }

            if (UserDto.Password != UserDto.NewPassword)
            {
                //验证失败提示
                eventAggregator.SendMessage("密码输入不一致！", "Login");
                return;
            }

            var registerResult = await authRequestService.RegisterAsync(new UserDto
            {
                //U = UserDto.Account,
                UserName = UserDto.UserName,
                Password = UserDto.Password
            }, seed, VerfiyCode);
            if (!registerResult.Status)
            {
                //注册成功
                eventAggregator.SendMessage("注册失败：" + registerResult.Message, "Login");
                return;
            }
            eventAggregator.SendMessage("注册成功！", "Login");
            SelectedIndex = 0;
            //注册失败提示...
        }

        private void LoginOut()
        {
            eventAggregator.SendMessage("退出成功！", "Login");
            RequestClose?.Invoke(new DialogResult(ButtonResult.No));
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(Account) || string.IsNullOrEmpty(Password)) return;//用户名密码null，提示....

            var loginResult = await authRequestService.LoginAsync(new UserDto
            {
                UserName = Account,
                Password = Password
            });

            if (loginResult is not null && loginResult.Status)
            {
                //AppSession.UserName = loginResult.Result.UserName;
                authModel.UpdateTime = DateTime.Now;
                authModel.UserName = Account;
                authModel.Password = Password;
                authModel.JwtStr = loginResult.Result.JwtStr;
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
                return;
            }
            //登陆失败，提示....
            eventAggregator.SendMessage(loginResult.Message, "Login");
        }

        public DelegateCommand<string> ExecuteCommand { get; private set; }

        #region 属性、字段

        private string account = "test1";
        private string password = "123456";
        private Guid seed = Guid.NewGuid();
        private int selectedIndex;
        private RegisterUserDto userDto = new RegisterUserDto();
        private string verfiyCode;
        private string verfiyCodeImgUrl;

        public string VerfiyCode
        {
            get { return verfiyCode; }
            set { verfiyCode = value; RaisePropertyChanged(); }
        }


        public string VerfiyCodeImgUrl
        {
            get
            {
                return verfiyCodeImgUrl;
            }
            set
            {
                verfiyCodeImgUrl = value; RaisePropertyChanged();
            }
        }

        public RegisterUserDto UserDto
        {
            get { return userDto; }
            set { userDto = value; RaisePropertyChanged(); }
        }


        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; RaisePropertyChanged(); }
        }

        public string Account
        {
            get { return account; }
            set { account = value; RaisePropertyChanged(); }
        }

        public string Password
        {
            get { return password; }
            set { password = value; RaisePropertyChanged(); }
        }



        #endregion
        #endregion

    }
}
