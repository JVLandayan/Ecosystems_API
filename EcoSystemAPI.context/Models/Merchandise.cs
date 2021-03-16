using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EcoSystemAPI.Context.Models
{
    public class Merchandise
    {
        [Key]
        public int MerchId { get; set; }
        [Required]
        public string MerchName { get; set; }
        [Required]
        public string MerchDetails { get; set; }
        public string MerchLink { get; set; }
        [Required]
        public string MerchImage { get; set; }

    }
}
