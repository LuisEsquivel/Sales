

namespace Sales.Common.Models
{
    //LOS USING VAN DENTRO DE LOS NAMESPACE
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
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

        [Required]
        [DataType(DataType.MultilineText)]
        public string REMARK_VAR { get; set; }

        [Display (Name="Image")]
        public string RUTA_IMAGEN_VAR { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public Decimal PRECIO_DEC { get; set; }

        [Required]
        public string UNIDAD_MEDIDA_VAR { get; set; }

        [Display(Name = "IsAvailable")]
        public Boolean IS_AVAILABLE_BIT { get; set; }

        [Required]
        [Display(Name = "PublishOn")]
        [DataType (DataType.Date)]
        public DateTime PUBLISH_ON_DATE { get; set; }

        [NotMapped]//para que no me lo mapé a la base de datos ya que no forma parte de la base de datos
        public byte[] ImageArray { get; set; }

        public string  ImageFullPath
        {
            get
            {
                if(!string.IsNullOrEmpty(this.RUTA_IMAGEN_VAR))
                {
                    return $"https://salesbackendgratis.azurewebsites.net/{this.RUTA_IMAGEN_VAR.Substring(1)}";

                }

                    return "noproduct";
            }
        }


    }
}