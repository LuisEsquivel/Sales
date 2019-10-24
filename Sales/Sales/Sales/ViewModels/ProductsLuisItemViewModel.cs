using GalaSoft.MvvmLight.Command;
using Sales.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Sales.Services;
using Xamarin.Forms;
using Sales.Helpers;
using System.Linq;
using Sales.Views;

namespace Sales.ViewModels
{
    public class ProductsLuisItemViewModel : ProductsLuis
    {

        #region Properties
        private ApiService apiService;
        #endregion

        #region Constructors
        public ProductsLuisItemViewModel()
        {
            this.apiService = new ApiService();
        }
        #endregion


        #region Methods

        #endregion

        #region Commands

        public ICommand DeleteProductCommand
        {
            get { return new RelayCommand(DeleteProduct); }
        }

        public async void DeleteProduct()
        {
            var answer = await  Application.Current.MainPage.DisplayAlert
               (
               
                Languages.Confirm ,
                Languages.DeleteConfirmation ,
                Languages .Yes,
                Languages.No
                
               );

            if(!answer)
            {
                return;
            }

            var connection = await this.apiService.CheckConnection();
            //si no hay conexión le pintamos un mensaje al usuario
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }

            /* var url = Application.Current.Resources["urlApi"].ToString();*/  //la url está en una llave en el App.xaml
            var response = await this.apiService.Delete("http://salesapi.somee.com/", "/api", "/ProductsLuis", this.CVE_PRODUCTO_VAR, Settings.TokenType, Settings.AccessToken);

            //no hay lista de productos
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            var productsViewModel = ProductsLuisViewModel.GetInstance();
            var deleteProduct = productsViewModel.MyProducts.Where(p => p.CVE_PRODUCTO_VAR == this.CVE_PRODUCTO_VAR).FirstOrDefault();

            if(deleteProduct != null)
            {
                productsViewModel.MyProducts.Remove(deleteProduct);
            }

            productsViewModel.RefreshList();

        }


        public ICommand EditProductCommand
        {
            get { return new RelayCommand(EditProduct); }
        }

        private async void EditProduct()
        {
            MainViewModel.GetInstance().EditProduct = new EditProdctsViewModel(this);
            await App.Navigator.PushAsync(new EditProductPage());
        }

        #endregion



    }
}
