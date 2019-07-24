
namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Sales.Helpers;
    using Sales.Views;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;
    using System.Windows.Input;
    using Xamarin.Forms;
   

    public class MainViewModel
    {

        #region Propiertes
        public EditProdctsViewModel EditProduct { get; set; }
        public ProductsLuisViewModel ProductsLuis { get; set; }
        public AddProductViewModel AddProduct { get; set; }
        public LoginViewModel  Login { get; set; }

        public ObservableCollection<MenuItemViewModel> Menu { get; set;}
        #endregion

        #region Constructor
        //constructor que crea un nuevo obeto de productos como MainViewModel

        public MainViewModel()
        {
            instance = this;
            this.LoadMenu();
        }

        private void LoadMenu()
        {
            this.Menu = new ObservableCollection<MenuItemViewModel>();

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_info_outline",
                PageName = "AboutPage",
                Title = Languages.About,
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_phonelink_setup",
                PageName = "SetupPage",
                Title = Languages.Setup,
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_exit_to_app",
                PageName = "LoginPage",
                Title = Languages.Exit,
            });
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
            await App.Navigator.PushAsync(new AddProductPage());
        } 
        #endregion
    }


}