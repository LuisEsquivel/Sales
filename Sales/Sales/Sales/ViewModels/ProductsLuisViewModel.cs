

namespace Sales.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;
    using Xamarin.Forms;
    using Sales.Common.Models;
    using Sales.Services;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Sales.Helpers;


    //ESTA CLASE HEREDA DE LA BASEVIEWMODEL PARA REFRESAR LOS CAMBIOS GENERADOS
    public class ProductsLuisViewModel : BaseViewModel
    {

        private ApiService apiService;
        private bool isRefreshing;


        //atributo privado
        public ObservableCollection<ProductsLuis> productsLuis;

        //creamos una colección de ProductsLuis
        //refresca la viewModel
        public ObservableCollection<ProductsLuis> ProductsLuis
        {

            get { return this.productsLuis; }
            set { this.SetValue(ref this.productsLuis, value); }

        }


        //refresar la vista de los productos
        public bool IsRefreshing
        {

            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }

        }



        //creamos el constructor para consumir el SERVICIO
        public ProductsLuisViewModel()
        {
            this.apiService = new ApiService();
            this.LoadProductsLuis();
        }

        private async void LoadProductsLuis()
        {

            this.IsRefreshing = true;

            //cheacamos si hay conexión y la almacenamos en una vaiable
            var connection = await this.apiService.CheckConnection();
            //si´no hay conexión le pintamos un mensaje al usuario
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error , connection.Message, Languages.Accept);
                return;
            }


           /* var url = Application.Current.Resources["urlApi"].ToString();*/  //la url está en una llave en el App.xaml
            var response = await this.apiService.GetList<ProductsLuis>("https://salesapigratis.azurewebsites.net", "/api", "/ProductsLuis");

            //no hay lista de productos
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }


            // obtenemos una lista del response.Result 
            var list = (List<ProductsLuis>)response.Result;

            //pasamos esa lista a ObservableCollection
            this.ProductsLuis = new ObservableCollection<ProductsLuis>(list);
            this.IsRefreshing = false;
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadProductsLuis);
               
            }
            
        }

    }
}
