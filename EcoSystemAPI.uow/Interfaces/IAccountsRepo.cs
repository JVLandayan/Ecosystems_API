using EcoSystemAPI.Context.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcoSystemAPI.uow.Interfaces
{
    public interface IAccountsRepo
    {
        bool SaveChanges();
        IEnumerable<Account> GetAllAccounts();
        Account GetAccountsById(int id);
        void CreateAccount(Account acc);
        void UpdateAccount(Account acc);
        void PartialAccountUpdate(Account acc);
        void DeleteAccount(Account acc);
        Account GetAccountByEmail(string email);
    }
}
