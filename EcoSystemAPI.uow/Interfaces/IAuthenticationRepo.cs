using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using EcoSystemAPI.Context.Models;
using EcoSystemAPI.core.Dtos.Login;
using EcoSystemAPI.Core.Dtos;

namespace EcoSystemAPI.uow.Interfaces
{
    public interface IAuthenticationRepos
    {
        Account Authenticate(Login login);

        string TokenConfig(string token);

        AccountsUpdateDto UpdateAccount(ResetPasswordDto model, Account user);
        AccountsUpdateDto AddToken(string token, Account user);
    }


}
