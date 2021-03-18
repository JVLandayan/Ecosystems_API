using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EcosystemAPI.util.services;
using EcoSystemAPI.Context.Models;
using EcoSystemAPI.core.Dtos.Login;
using EcoSystemAPI.Core.Dtos;
using EcoSystemAPI.Data;
using EcoSystemAPI.uow.Interfaces;
using EcoSystemAPI.uow.Token;
using EcoSystemAPI.util.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EcoSystemAPI.Controllers  
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService _userService;
        private readonly IAccountsRepo _accRepo;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IAuthenticationRepos _authRepo;

        public AuthController(IUserService userService, IAccountsRepo repo, IConfiguration configuration, IMapper mapper, IAuthenticationRepos authRepo)
        {
            _userService = userService;
            _accRepo = repo;
            _configuration = configuration;
            _mapper = mapper;
            _authRepo = authRepo;
            
        }

        [HttpPost]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var modifiedModel = new AuthenticateRequest
            {
                Email = model.Email.ToLower(),
                Password = model.Password
            };
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [HttpPost("forgotpass")]
        public IActionResult ForgotPassword(ForgotPassDto model)
        {
            var user = _accRepo.GetAccountByEmail(model.Email);
            //EmailConfig
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("ecotigers.ust@gmail.com", "landayan24");
            MailMessage msg = new MailMessage();

            //Token config
            var Token = Guid.NewGuid().ToString();
            var encodedToken = _authRepo.TokenConfig(Token);

            if (user != null)
            {
                //Adding table data to db
                var modifiedData = _authRepo.AddToken(Token, user);
                if (user == null)
                {
                    return NotFound();
                }
                _mapper.Map(modifiedData, user);
                _accRepo.UpdateAccount(user);
                _accRepo.SaveChanges();

                //Sending Email with query parameters
                string url = $"{_configuration["ClientAppUrl"]}/resetpassword?email={model.Email}&token={encodedToken}";
                msg.To.Add(model.Email);
                msg.From = new MailAddress("UST Eco-Tigers <ecosystem.ust@gmail.com>");
                msg.Subject = "Password Reset Url";
                msg.Body = url;
                client.Send(msg);
                return Ok();
            }
            return BadRequest();

        }

        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordDto model)
        {
            var user = _accRepo.GetAccountByEmail(model.Email);
            if (user == null )
            {
                return BadRequest(new { message = "No user associated with given email" });
            }
            if(user.Password == model.NewPassword)
            {
                return BadRequest(new { message = "Input password is the current password" });
            }

            var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            if(normalToken != user.ResetToken)
            {
                return BadRequest();
            };
            var modifiedData = _authRepo.UpdateAccount(model, user);
            _mapper.Map(modifiedData, user);
            _accRepo.UpdateAccount(user);
            _accRepo.SaveChanges();
            return Ok(new {message="Password successfully been updated"});

        }

    }
}
