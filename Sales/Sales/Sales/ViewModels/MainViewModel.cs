
namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Sales.Views;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class MainViewModel
    {

        public ProductsLuisViewModel ProductsLuis { get; set; }
        public AddProductViewModel  AddProduct { get; set; }


        //constructor que crea un nuevo obeto de productos como MainViewModel
        public MainViewModel()
        {
            this.ProductsLuis = new ProductsLuisViewModel();
        }

        public ICommand AddProductCommand
        {
            get
            {
                return new RelayCommand(GoToAddProduct);
            }
                
        }

        private async void GoToAddProduct()
        {
            this.AddProduct = new AddProductViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new AddProductPage());
        }
    }


}