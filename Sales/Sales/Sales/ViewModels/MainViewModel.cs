
namespace Sales.ViewModels
{

    using System;
    using System.Collections.Generic;
    using System.Text;

    public class MainViewModel
    {

        public ProductsLuisViewModel ProductsLuis { get; set; }



        //constructor que crea un nuevo obeto de productos como MainViewModel
        public MainViewModel()
        {
            this.ProductsLuis = new ProductsLuisViewModel();
        }
    }


}