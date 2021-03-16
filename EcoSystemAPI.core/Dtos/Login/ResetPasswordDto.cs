using System;
using System.Collections.Generic;
using System.Text;

namespace EcoSystemAPI.core.Dtos.Login
{
    public class ResetPasswordDto
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
