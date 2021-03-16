using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcoSystemAPI.Core.Dtos
{
    public class AccountsReadDto
    {
        public int Id { get; set; }
        public int AuthId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }       
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhotoFileName { get; set; }
    }
}
