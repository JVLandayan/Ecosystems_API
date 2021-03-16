
using System.ComponentModel.DataAnnotations;

namespace EcoSystemAPI.uow.Token
{
    public class AuthenticateRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}