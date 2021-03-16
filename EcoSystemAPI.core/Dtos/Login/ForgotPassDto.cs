using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EcoSystemAPI.core.Dtos.Login
{
    public class ForgotPassDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
