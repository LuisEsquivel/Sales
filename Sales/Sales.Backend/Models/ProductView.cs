

namespace Sales.Backend.Models
{

    using Sales.Common.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class ProductView:ProductsLuis
    {

        public HttpPostedFileBase ImageFile { get; set; }

    }
}