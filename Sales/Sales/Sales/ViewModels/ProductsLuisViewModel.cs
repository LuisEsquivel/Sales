

namespace Sales.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;
    using Xamarin.Forms;
    using Sales.Common.Models;
    using Sales.Services;


    //ESTA CLASE HEREDA DE LA BASEVIEWMODEL PARA REFRESAR LOS CAMBIOS GENERADOS
    public class ProductsLuisViewModel : BaseViewModel
    {

        private ApiService apiService;


        //atributo privado
        public ObservableCollection<ProductsLuis> productsLuis;

        //creamos una colección de ProductsLuis
        //refresca la viewModel
        public ObservableCollection<ProductsLuis> ProductsLuis
        {

            get { return this.productsLuis; }
            set { this.SetValue(ref this.productsLuis, value); }

        }



        //creamos el constructor para consumir el SERVICIO
        public ProductsLuisViewModel()
        {
            this.apiService = new ApiService();
            this.LoadProductsLuis();
        }

        private async void LoadProductsLuis()
        {

            var response = await this.apiService.GetList<ProductsLuis>("https://salesapigratis.azurewebsites.net", "/api", "/ProductsLuis");

            //no hay lista de productos
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }


            // obtenemos una lista del response.Result 
            var list = (List<ProductsLuis>)response.Result;

            //pasamos esa lista a ObservableCollection
            this.ProductsLuis = new ObservableCollection<ProductsLuis>(list);
        }
    }
}