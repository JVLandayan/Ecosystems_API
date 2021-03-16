using EcoSystemAPI.Context.Models;
using EcoSystemAPI.core.Dtos.Login;
using EcoSystemAPI.Core.Dtos;
using EcoSystemAPI.Data.Context;
using EcoSystemAPI.uow.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSystemAPI.Data
{
    public class SqlAuthenticationRepo : IAuthenticationRepos
    {
        private readonly EcosystemContext _context;

        public SqlAuthenticationRepo(EcosystemContext context)
        {
            _context = context;
        }

        public AccountsUpdateDto AddToken(string token, Account user)
        {
            var modifiedData = new AccountsUpdateDto
            {
                FirstName = user.FirstName,
                Email = user.Email,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                Password = user.Password,
                PhotoFileName = user.PhotoFileName,
                ResetToken = token
            };
            return modifiedData;
        }

        public Account Authenticate(Login login)
        {
            return _context.Accounts.FirstOrDefault(user => user.Email == login.Email && user.Password == login.Password);

        }

        public string TokenConfig(string token)
        {
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);
            return validToken;
        }

        public AccountsUpdateDto UpdateAccount(ResetPasswordDto model, Account user)
        {
            var modifiedData = new AccountsUpdateDto
            {
                FirstName = user.FirstName,
                Email = user.Email,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                Password = model.NewPassword,
                PhotoFileName = user.PhotoFileName,
                ResetToken = Guid.NewGuid().ToString()
            };
            return modifiedData;
        }
    }
}
