﻿

namespace Sales.Common.Models
{
    //LOS USING VAN DENTRO DE LOS NAMESPACE
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class ProductsLuis
    {

        [Key]
        [Required]
        public string CVE_PRODUCTO_VAR { get; set; }

        [Required]
        public string NOM_PROD_VAR { get; set; }

        public Decimal PRECIO_DEC { get; set; }

        [Required]
        public string UNIDAD_MEDIDA_VAR { get; set; }


        //public Boolean IsAvailable { get; set; }

        //public DateTime PublishOn { get; set; }

    }
}