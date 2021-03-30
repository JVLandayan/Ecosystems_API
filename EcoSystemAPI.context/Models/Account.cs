using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EcoSystemAPI.Context.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int AuthId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        [Required] 
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [Column(TypeName = "varchar(MAX)")]
        public string PhotoFileName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ResetToken { get; set; }

    }
}
