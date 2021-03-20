using AutoMapper;
using EcoSystemAPI.Context.Models;
using EcoSystemAPI.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcoSystemAPI.Profiles
{ 
    public class AccountsProfile :Profile
    {
        public AccountsProfile()
        {
            //Source -> Target
            CreateMap<Account, AccountsReadDto>();
            CreateMap<Account, AccountsAuthorRead>();
            CreateMap<AccountsCreateDto, Account>();
            CreateMap<AccountsUpdateDto, Account>();
            CreateMap<Account, AccountsUpdateDto>();
        }
    }
}
