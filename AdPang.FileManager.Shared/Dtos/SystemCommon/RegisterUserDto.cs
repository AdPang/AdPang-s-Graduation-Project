using AdPang.FileManager.Shared.Dtos.Base;

namespace AdPang.FileManager.Shared.Dtos.SystemCommon
{
    public class RegisterUserDto : BaseDto<Guid>
    {
        private string userName;
        private string password;
        private string newPassword;

        public string UserName
        {
            get { return userName; }
            set { userName = value; OnPropertyChanged(); }
        }



        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(); }
        }



        public string NewPassword
        {
            get { return newPassword; }
            set { newPassword = value; OnPropertyChanged(); }
        }

    }
}
