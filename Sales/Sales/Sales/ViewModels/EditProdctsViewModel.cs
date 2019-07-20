using GalaSoft.MvvmLight.Command;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Sales.Common.Models;
using Sales.Helpers;
using Sales.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class EditProdctsViewModel:BaseViewModel 
    {
        #region Atributtes
        private ProductsLuis productsLuis;
        public bool isRunning;
        public bool isEnabled;
        private Services.ApiService apiService;
        private ImageSource imageSource;
        private MediaFile file;
        #endregion

        #region Propierties
        public ProductsLuis Product
        {
            get { return this.productsLuis; }
            set {this.SetValue (ref this.productsLuis, value) ; }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }


        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); } 
        }

        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { this.SetValue(ref this.imageSource, value); }
        }
        #endregion

        #region Constructor
        public EditProdctsViewModel(ProductsLuis productsLuis)
        {
            this.productsLuis = productsLuis;
            this.isEnabled = true;
            this.apiService = new ApiService();
            this.ImageSource = productsLuis.ImageFullPath;
        }
        #endregion



        #region Commands
        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }

        }

        private async void Save()
        {

            //clave del producto vacía
            if (string.IsNullOrEmpty(this.Product.CVE_PRODUCTO_VAR ))
            {
                await Application.Current.MainPage.DisplayAlert
                     (
                      Languages.Error,
                      Languages.KeyProduct,
                      Languages.Accept
                     );


                return;
            }


            //nombre del producto vacío
            if (string.IsNullOrEmpty(this.Product.NOM_PROD_VAR ))
            {
                await Application.Current.MainPage.DisplayAlert
                     (
                      Languages.Error,
                      Languages.DescriptionError,
                      Languages.Accept
                     );


                return;
            }



            if (this.Product.PRECIO_DEC  <= 0)
            {
                await Application.Current.MainPage.DisplayAlert
                     (
                      Languages.Error,
                      Languages.PriceError,
                      Languages.Accept
                     );


                return;
            }


            //unidad de medida vacía
            if (string.IsNullOrEmpty(this.Product.UNIDAD_MEDIDA_VAR))
            {
                await Application.Current.MainPage.DisplayAlert
                     (
                      Languages.Error,
                      Languages.UnitOfMeasurement,
                      Languages.Accept
                     );


                return;
            }


            this.IsRunning = true;
            this.IsEnabled = false;


            //checamos si el cel tiene conexión a internet
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert
                 (
                  Languages.Error,
                  connection.Message,
                  Languages.Accept
                 );
                return;
            }



            //checamos si el usuario seleccionó una imagen 
            // si es así la convertimos en un array de bites
            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FileHelper.ReadFully(this.file.GetStream());
                this.Product.ImageArray = imageArray;
            }


            //enviamos el nuevo producto
            /* var url = Application.Current.Resources["urlApi"].ToString();*/  //la url está en una llave en el ApnewProduct.xaml
            var response = await this.apiService.Put("https://salesapigratis.azurewebsites.net", "/api", "/ProductsLuis", this.Product, this.Product.CVE_PRODUCTO_VAR);

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert
                (
                 Languages.Error,
                 response.Message,
                 Languages.Accept
                );
                return;
            }




            // cargamos el nuevo producto para mostrarlo en la página principal de productos
            var newProduct = (ProductsLuis)response.Result;
            var productsLuisViewModel = ProductsLuisViewModel.GetInstance();


            var oldProduct = productsLuisViewModel.MyProducts.Where(p => p.CVE_PRODUCTO_VAR == this.Product.CVE_PRODUCTO_VAR).FirstOrDefault();
            if(oldProduct != null)
            {
                productsLuisViewModel.MyProducts.Remove(oldProduct);
            }

            productsLuisViewModel.MyProducts.Add(newProduct);
            productsLuisViewModel.RefreshList();

            this.IsRunning = false;
            this.IsEnabled = true;

            //volvemos a la página principal
            await Application.Current.MainPage.Navigation.PopAsync();

        }





        public ICommand ChangeImageCommand
        {
            get { return new RelayCommand(ChangeImage); }
        }

        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                Languages.ImageSource,
                Languages.Cancel,
                null,
                Languages.FromGallery,
                Languages.NewPicture);

            if (source == Languages.Cancel)
            {
                this.file = null;
                return;
            }

            if (source == Languages.NewPicture)
            {
                this.file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = this.file.GetStream();
                    return stream;
                });
            }
        }


        #endregion
    }
}
