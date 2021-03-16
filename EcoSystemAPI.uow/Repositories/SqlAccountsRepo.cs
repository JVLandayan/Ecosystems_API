using EcoSystemAPI.Context.Models;
using EcoSystemAPI.Data.Context;
using EcoSystemAPI.uow.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcoSystemAPI.uow.Repositories
{
    public class SqlAccountsRepo : IAccountsRepo
    {
        private readonly EcosystemContext _context;

        public SqlAccountsRepo(EcosystemContext context)
        {
            _context = context;
        }

        public Account GetAccountsById(int id)
        {
            return _context.Accounts.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return _context.Accounts.ToList(); 
        }

        //Preparing data to be saved (.add)
        public void CreateAccount(Account acc)
        {
            if (acc == null)
            {
                throw new ArgumentNullException(nameof(acc));
            }
            _context.Accounts.Add(acc);
        }

        //Saving the actual data to db (.SaveChanges)
        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateAccount(Account acc)
        {

        }

        public void PartialAccountUpdate(Account acc)
        {
        }

        public void DeleteAccount(Account acc)
        {
            if (acc == null)
            {
                throw new ArgumentNullException(nameof(acc));
            }
            _context.Remove(acc);
        }

        public Account GetAccountByEmail(string email)
        {
            return _context.Accounts.FirstOrDefault(p => p.Email == email);
        }
    }
}
