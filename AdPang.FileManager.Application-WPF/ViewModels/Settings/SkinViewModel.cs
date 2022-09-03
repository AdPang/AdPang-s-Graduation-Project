using System;
using System.Collections.Generic;
using System.Windows.Media;
using MaterialDesignColors;
using MaterialDesignColors.ColorManipulation;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;

namespace AdPang.FileManager.Application_WPF.ViewModels.Settings
{
    public class SkinViewModel : BindableBase
    {
        private bool _isDarkTheme = true;

        public bool IsDarkTheme
        {
            get { return _isDarkTheme; }
            set
            {
                if (SetProperty(ref _isDarkTheme, value))
                {
                    ModifyTheme(theme => theme.SetBaseTheme(value ? Theme.Dark : Theme.Light));
                }
            }
        }
        private static void ModifyTheme(Action<ITheme> modificationAction)
        {
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            modificationAction?.Invoke(theme);
            paletteHelper.SetTheme(theme);
        }

        public IEnumerable<ISwatch> Swatches { get; } = SwatchHelper.Swatches;
        public DelegateCommand<object> ChangeHueCommand { get; private set; }
        private readonly PaletteHelper paletteHelper = new PaletteHelper();

        public SkinViewModel()
        {
            ChangeHueCommand = new DelegateCommand<object>(ChangeHue);
        }

        private void ChangeHue(object obj)
        {
            var hue = (Color)obj;
            ITheme theme = paletteHelper.GetTheme();
            theme.PrimaryLight = new ColorPair(hue.Lighten());
            theme.PrimaryMid = new ColorPair(hue);
            theme.PrimaryDark = new ColorPair(hue.Darken());

            paletteHelper.SetTheme(theme); ;
        }
    }
}
