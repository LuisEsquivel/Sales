using GalaSoft.MvvmLight.Command;
using Sales.Helpers;
using Sales.Services;
using Sales.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class LoginViewModel : BaseViewModel 
    {
        #region Atributes
        private bool isRunning;
        private bool isEnabled;
        private ApiService apiService;
        #endregion


        #region Propiertes
        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsRemembered { get; set; }


        public bool IsRunninng
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled ; }
            set { this.SetValue(ref this.isEnabled, value); }
        }


        #endregion



        #region Constructor
        public LoginViewModel()
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;
        }
        #endregion


        #region Commands
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
         }

        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert
                (
                Languages.Error,
                Languages.EmailValidation,
                Languages.Accept
                );

                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert
                (
                Languages.Error,
                Languages.PasswordValidation,
                Languages.Accept
                );

                return;
            }

            this.IsRunninng = true;
            this.IsEnabled = false;


            //cheacamos si hay conexión y la almacenamos en una vaiable
            var connection = await this.apiService.CheckConnection();
            //si´no hay conexión le pintamos un mensaje al usuario
            if (!connection.IsSuccess)
            {
                this.IsRunninng  = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }

            /* var url = Application.Current.Resources["urlApi"].ToString();*/  //la url está en una llave en el App.xaml
            var url = "https://salesapigratis.azurewebsites.net";
            var token = await this.apiService.GetToken(url, this.Email, this.Password);

            if(token == null || string.IsNullOrEmpty(token.AccessToken))
            {

                this.IsRunninng = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert
                    (

                    Languages.Error ,
                    Languages.SomethingWrong ,
                    Languages.Accept

                    );

                return;
            }

            //guardamos el tipo de token, el token como tal y si el usuario es recordado o no
            Settings.TokenType = token.TokenType;
            Settings.AccessToken  = token.AccessToken ;
            Settings.IsRemembered = this.IsRemembered;



            MainViewModel.GetInstance().ProductsLuis = new ProductsLuisViewModel();
            Application.Current.MainPage = new MasterPage();
            this.IsRunninng = false;
            this.IsEnabled = true;

        }


        #endregion


        #region Methods
        #endregion


    }
}
