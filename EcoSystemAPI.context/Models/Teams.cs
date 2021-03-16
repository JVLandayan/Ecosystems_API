using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EcoSystemAPI.Context.Models
{
    public class Teams
    {
        [Key]
        public int TeamsId { get; set; }
        [Required]
        public string TeamsImage { get; set; }
        [Required]
        public string TeamsName { get; set; }
        [Required]
        public string TeamsRole { get; set; }
        [Required]
        public string TeamsDetails { get; set; }
        public string TeamsFacebook { get; set; }
        public string TeamsInstagram { get; set; }
        public string TeamsTwitter { get; set; }
    }
}
