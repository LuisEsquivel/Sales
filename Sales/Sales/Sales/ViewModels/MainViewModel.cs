
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

        #region Propiertes
        public EditProdctsViewModel EditProduct { get; set; }
        public ProductsLuisViewModel ProductsLuis { get; set; }
        public AddProductViewModel AddProduct { get; set; }
        #endregion

        #region Constructor
        //constructor que crea un nuevo obeto de productos como MainViewModel

        public MainViewModel()
        {
            instance = this;
            this.ProductsLuis = new ProductsLuisViewModel();
        }
        #endregion

        #region Singlenton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }

        #endregion

        #region Commands
        public ICommand AddProductCommand
        {
            get
            {
                return new RelayCommand(GoToAddProduct);
            }

        }
        #endregion

        #region Methods
        private async void GoToAddProduct()
        {
            this.AddProduct = new AddProductViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new AddProductPage());
        } 
        #endregion
    }


}