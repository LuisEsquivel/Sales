

namespace Sales.Domain.Models
{

    //LOS USING VAN DENTRO DE LOS NAMESPACE
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Sales.Common;



    public class DataContext : DbContext
    {


        //NOMBRE DE LA CADENA DE CONEXIÓN EN EL WEB CONFIG
        public DataContext() : base("DefaultConnection")
        {

        }

        public System.Data.Entity.DbSet<Common.Models.ProductsLuis> ProductsLuis { get; set; }
    }

}
