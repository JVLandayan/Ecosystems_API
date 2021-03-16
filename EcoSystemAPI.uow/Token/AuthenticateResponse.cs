

using EcoSystemAPI.Context.Models;

namespace EcoSystemAPI.uow.Token
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public int AuthId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhotoFileName { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(Account user, string token)
        {
            Id = user.Id;
            AuthId = user.AuthId;
            FirstName = user.FirstName;
            MiddleName = user.MiddleName;
            LastName = user.LastName;
            Email = user.Email;
            PhotoFileName = user.PhotoFileName;
            Token = token;
        }
    }
}