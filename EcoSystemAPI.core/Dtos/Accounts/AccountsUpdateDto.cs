using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace EcoSystemAPI.Core.Dtos
{
    public class AccountsUpdateDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhotoFileName { get; set; }
        public string Password { get; set; }

        public string ResetToken { get; set; }
    }
}
