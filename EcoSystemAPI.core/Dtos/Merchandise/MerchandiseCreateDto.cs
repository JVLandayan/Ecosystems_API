using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EcoSystemAPI.Core.Dtos
{
    public class MerchandiseCreateDto
    {
        [Required]
        public string MerchName { get; set; }
        [Required]
        public string MerchDetails { get; set; }
        public string MerchLink { get; set; }
        [Required]
        public string MerchImage { get; set; }
    }
}
