using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EcoSystemAPI.Core.Dtos
{
    public class MerchandiseReadDto
    {
        public int MerchId { get; set; }
        public string MerchName { get; set; }
        public string MerchDetails { get; set; }
        public string MerchLink { get; set; }
        public string MerchImage { get; set; }
    }
}
