using GalaSoft.MvvmLight.Command;
using Sales.Helpers;
using Sales.ViewModels;
using Sales.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewsModels
{
    public class MenuItemViewModel
    {
        public string Icon { get; set; }

        public string Title { get; set; }

        public string PageName { get; set; }



        #region Commands
        public ICommand GoToCommand
        {
            get { return new RelayCommand(Goto); }

        }

        private void Goto()
        {
          if(this.PageName == "LoginPage")
            {

                Settings.TokenType = string.Empty;
                Settings.AccessToken = string.Empty;
                Settings.IsRemembered = false;

                MainViewModel.GetInstance().Login = new LoginViewModel();
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }
        #endregion
    }
}
