using Ecosystem.util.helpers;
using EcoSystemAPI.Context.Models;
using EcoSystemAPI.Core.Dtos;
using EcoSystemAPI.Data.Context;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using EcoSystemAPI.uow.Token;
using EcoSystemAPI.util.services;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace EcosystemAPI.util.services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        Account GetById(int id);
        string HashPassword(string password);


    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private readonly AppSettings _appSettings;
        private readonly EcosystemContext _context;


        public UserService(IOptions<AppSettings> appSettings, EcosystemContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;

        }


        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var account = _context.Accounts.SingleOrDefault(x => x.Email == model.Email && x.Password == HashPassword(model.Password));

            // return null if user not found
            if (account == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(account);

            return new AuthenticateResponse(account, token);
        }

        public Account GetById(int id)
        {
            return _context.Accounts.FirstOrDefault(x => x.Id == id);
        }

        // helper methods

        private string generateJwtToken(Account user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("Id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(120),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string HashPassword(string password)
        {
            var passBytes = Encoding.ASCII.GetBytes(password);
            var sha = new SHA512Managed();
            var hash = sha.ComputeHash(passBytes);
            var encryptedPass = ""; 
            foreach (byte b in hash)
            {
                encryptedPass += b.ToString("x2");
            }
            return encryptedPass;
        }

    }
}