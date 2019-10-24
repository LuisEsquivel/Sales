

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
    using System.Linq;
    using System.Threading.Tasks;


    //ESTA CLASE HEREDA DE LA BASEVIEWMODEL PARA REFRESAR LOS CAMBIOS GENERADOS
    public class ProductsLuisViewModel : BaseViewModel
    {

        #region Attributes
        private ApiService apiService;
        private DataService dataService;
        private bool isRefreshing;
        //atributo privado
        public ObservableCollection<ProductsLuisItemViewModel> productsLuis;
        private string filter;
        #endregion

        #region Properties

        public List<ProductsLuis> MyProducts { get; set; }

        public string Filter
        {
          get {
                return this.filter;
              }

          set {
                this.filter = value;
                this.RefreshList();
              }
        }

        //creamos una colección de ProductsLuis
        //refresca la viewModel
        public ObservableCollection<ProductsLuisItemViewModel> ProductsLuis
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
        #endregion

        #region Constructor
        //creamos el constructor para consumir el SERVICIO
        public ProductsLuisViewModel()
        {
            instance = this;
            this.apiService  = new ApiService();
            this.dataService = new DataService();
            this.LoadProductsLuis();
        }
        #endregion

        #region Methods
        private async void LoadProductsLuis()
        {

            this.IsRefreshing = true;

            //cheacamos si hay conexión y la almacenamos en una vaiable
            var connection = await this.apiService.CheckConnection();
            //si´no hay conexión le pintamos un mensaje al usuario
            if (connection.IsSuccess)
            {
                var answer = await this.LoadProductsFromAPI();
                if(answer)
                {
                    this.SaveProductsToDB();
                }
                else
                {
                    await this.LoadProductsFromDB();
                }

            }


            if(this.MyProducts == null || this.MyProducts.Count == 0)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.NoProductsMessage , Languages.Accept);
                return;
            }



            this.RefreshList();
            this.IsRefreshing = false;
        }


        //pintamos los productos con los existentes en la BD sqLITE
        private async  Task LoadProductsFromDB()
        {
            this.MyProducts = await this.dataService.GetAllProducts();
        }


        //eliminamos los productos de SQLLite y los volvemos a instalar
        private async Task  SaveProductsToDB()
        {
            await this.dataService.DeleteAllProducts();
            this.dataService.Insert(this.MyProducts);
        }


        //cargamos los productos consumiendo la API
        private async Task<bool> LoadProductsFromAPI()
        {
            /* var url = Application.Current.Resources["urlApi"].ToString();*/  //la url está en una llave en el App.xaml
            var response = await this.apiService.GetList<ProductsLuis>("http://salesapi.somee.com/", "/api", "/ProductsLuis", Settings.TokenType, Settings.AccessToken);

            //no hay lista de productos
            if (!response.IsSuccess)
            {
                return false;
            }


            // obtenemos una lista del response.Result 
            this.MyProducts = (List<ProductsLuis>)response.Result;
            return true;
        }

        public void RefreshList()
        {


            if (string.IsNullOrEmpty(this.Filter))
            {
                var myListProductsLuisItemViewModel = this.MyProducts.Select(p => new ProductsLuisItemViewModel
                {

                    CVE_PRODUCTO_VAR = p.CVE_PRODUCTO_VAR,
                    NOM_PROD_VAR = p.NOM_PROD_VAR,
                    PRECIO_DEC = p.PRECIO_DEC,
                    REMARK_VAR = p.REMARK_VAR,
                    RUTA_IMAGEN_VAR = p.RUTA_IMAGEN_VAR,
                    IS_AVAILABLE_BIT = p.IS_AVAILABLE_BIT,
                    PUBLISH_ON_DATE = p.PUBLISH_ON_DATE,
                    ImageArray = p.ImageArray,
                    UNIDAD_MEDIDA_VAR = p.UNIDAD_MEDIDA_VAR,

                });

                //pasamos esa lista a ObservableCollection
                this.ProductsLuis = new ObservableCollection<ProductsLuisItemViewModel>(
                    myListProductsLuisItemViewModel.OrderBy(P => P.NOM_PROD_VAR));



            }else
            {
                var myListProductsLuisItemViewModel = this.MyProducts.Select(p => new ProductsLuisItemViewModel
                {

                    CVE_PRODUCTO_VAR = p.CVE_PRODUCTO_VAR,
                    NOM_PROD_VAR = p.NOM_PROD_VAR,
                    PRECIO_DEC = p.PRECIO_DEC,
                    REMARK_VAR = p.REMARK_VAR,
                    RUTA_IMAGEN_VAR = p.RUTA_IMAGEN_VAR,
                    IS_AVAILABLE_BIT = p.IS_AVAILABLE_BIT,
                    PUBLISH_ON_DATE = p.PUBLISH_ON_DATE,
                    ImageArray = p.ImageArray,
                    UNIDAD_MEDIDA_VAR = p.UNIDAD_MEDIDA_VAR,

                }).Where(p => p.NOM_PROD_VAR.ToLower().Contains(this.Filter.ToLower()));


                //pasamos esa lista a ObservableCollection
                this.ProductsLuis = new ObservableCollection<ProductsLuisItemViewModel>(
                    myListProductsLuisItemViewModel.OrderBy(P => P.NOM_PROD_VAR));
            }
        }

        
            #endregion

        #region Singlenton
        private static ProductsLuisViewModel instance;

        public static ProductsLuisViewModel GetInstance()
        {
            if(instance == null)
            {
                return new ProductsLuisViewModel();
            }

            return instance;
        }

        #endregion

        #region Commands

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(RefreshList);

            }

        }

        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadProductsLuis);

            }

        } 
        #endregion

    }
}
