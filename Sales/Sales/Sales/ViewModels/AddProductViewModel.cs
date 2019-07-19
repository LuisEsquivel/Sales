

namespace Sales.ViewModels
{


    using GalaSoft.MvvmLight.Command;
    using Sales.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Services;
    using Sales.Common.Models;
    using Plugin.Media.Abstractions;
    using Plugin.Media;

    public class AddProductViewModel:BaseViewModel 
    {

        #region Attributes
        public bool isRunning;
        public bool isEnabled;
        private ApiService apiService;
        private ImageSource imageSource;
        private MediaFile file;
        #endregion

        #region Properties

        public string KeyProduct { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string Remarks { get; set; }
        public string UnitOfMeasurement { get; set; }
        public bool IsRunning
        {
            get { return this.isRunning ; }
            set { this.SetValue(ref this.isRunning, value); }
        }


        public bool IsEnabled
        {
            get { return this.isEnabled  ; }
            set { this.SetValue(ref this.isEnabled, value); }
        }

        public ImageSource ImageSource
        {
            get { return this.imageSource ; }
            set { this.SetValue(ref this.imageSource, value); }
        }

        #endregion

        #region constructors

        public AddProductViewModel()
        {

            this.isEnabled = true;
            this.apiService = new ApiService();
            this.ImageSource = "noproduct";
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

        private async  void Save()
        {

            //clave del producto vacía
            if (string.IsNullOrEmpty(this.KeyProduct))
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
            if (string.IsNullOrEmpty(this.Name))
            {
                await Application.Current.MainPage.DisplayAlert
                     (
                      Languages.Error,
                      Languages.DescriptionError,
                      Languages .Accept 
                     );


                return;
            }



            //precio vacío
            if (string.IsNullOrEmpty(this.Price))
            {
                await Application.Current.MainPage.DisplayAlert
                     (
                      Languages.Error,
                      Languages.PriceError ,
                      Languages.Accept
                     );


                return;
            }


            //precio menor o igual a cero
            var price = decimal.Parse(this.Price);
            if (price <= 0)
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
            if (string .IsNullOrEmpty(UnitOfMeasurement))
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
            this.IsEnabled = false ;


            //checamos si el cel tiene conexión a internet
            var connection = await this.apiService.CheckConnection();      
            if (!connection.IsSuccess)
            {
                this.IsRunning = false ;
                this.IsEnabled = true  ;

                await Application.Current.MainPage.DisplayAlert
                 (
                  Languages.Error,
                  connection.Message ,
                  Languages.Accept
                 );
               return;
            }



            //checamos si el usuario seleccionó una imagen 
            // si es así la convertimos en un array de bites
            byte[] imageArray = null;
            if(this.file != null)
            {
                imageArray = FileHelper.ReadFully(this.file.GetStream());
            }


            //vamos a agregar un nuevo producto
            var product = new ProductsLuis
            {
                CVE_PRODUCTO_VAR   = KeyProduct ,
                NOM_PROD_VAR       = this.Name ,
                PRECIO_DEC         = price ,
                REMARK_VAR         = this.Remarks ,
                UNIDAD_MEDIDA_VAR  = this.UnitOfMeasurement,
                ImageArray         = imageArray ,
            };


            //enviamos el nuevo producto
            /* var url = Application.Current.Resources["urlApi"].ToString();*/  //la url está en una llave en el ApnewProduct.xaml
            var response = await this.apiService.PostList("https://salesapigratis.azurewebsites.net", "/api", "/ProductsLuis", product);

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true ;
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
            var viewModel = ProductsLuisViewModel.GetInstance();

            viewModel.ProductsLuis.Add(new ProductsLuisItemViewModel
            {
              CVE_PRODUCTO_VAR = newProduct.CVE_PRODUCTO_VAR ,
              NOM_PROD_VAR     = newProduct.NOM_PROD_VAR ,
              PRECIO_DEC       = newProduct.PRECIO_DEC ,
              REMARK_VAR       = newProduct.REMARK_VAR ,
              RUTA_IMAGEN_VAR  = newProduct.RUTA_IMAGEN_VAR ,
              IS_AVAILABLE_BIT = newProduct.IS_AVAILABLE_BIT ,
              PUBLISH_ON_DATE  = newProduct.PUBLISH_ON_DATE ,
              ImageArray       = newProduct.ImageArray ,
            });

        
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
