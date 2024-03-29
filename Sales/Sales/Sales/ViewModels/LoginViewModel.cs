﻿using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using Sales.Common.Models;
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


        public bool IsRunning
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

        public ICommand  RegisterCommand
        {
            get
            {
                return new RelayCommand(Register);
            }

        }

        private async void Register()
        {
            MainViewModel.GetInstance().Register = new RegisterViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }

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

            this.IsRunning = true;
            this.IsEnabled = false;


            //cheacamos si hay conexión y la almacenamos en una vaiable
            var connection = await this.apiService.CheckConnection();
            //si´no hay conexión le pintamos un mensaje al usuario
            if (!connection.IsSuccess)
            {
                this.IsRunning  = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }

            /* var url = Application.Current.Resources["urlApi"].ToString();*/  //la url está en una llave en el App.xaml
            var url = "http://salesapi.somee.com/";
            var token = await this.apiService.GetToken(url, this.Email, this.Password);

            if(token == null || string.IsNullOrEmpty(token.AccessToken))
            {

                this.IsRunning = false;
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
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlUsersController"].ToString();
            var response = await this.apiService.GetUser(url, prefix, $"{controller}/GetUser", this.Email, token.TokenType, token.AccessToken);
            if (response.IsSuccess)
            {
                var userASP = (MyUserASP)response.Result;
                MainViewModel.GetInstance().UserAsp  = userASP;
                //MainViewModel.GetInstance().RegisterDevice();
                Settings.UserASP = JsonConvert.SerializeObject(userASP);
            }


            MainViewModel.GetInstance().ProductsLuis = new ProductsLuisViewModel();
            Application.Current.MainPage = new MasterPage();
            this.IsRunning = false;
            this.IsEnabled = true;


        }



        public ICommand LoginFacebookComand
        {
            get
            {
                return new RelayCommand(LoginFacebook);
            }
        }

        private async void LoginFacebook()
        {
            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsRunning  = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            await Application.Current.MainPage.Navigation.PushAsync(
                new LoginFacebookPage());
        }

        public ICommand LoginInstagramComand
        {
            get
            {
                return new RelayCommand(LoginInstagram);
            }
        }

        private async void LoginInstagram()
        {
            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            await Application.Current.MainPage.Navigation.PushAsync(
                new LoginInstagramPage());
        }

        public ICommand LoginTwitterComand
        {
            get
            {
                return new RelayCommand(LoginTwitter);
            }
        }

        private async void LoginTwitter()
        {
            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            await Application.Current.MainPage.Navigation.PushAsync(
                new LoginTwitterPage());
        }
        #endregion




    }
}



