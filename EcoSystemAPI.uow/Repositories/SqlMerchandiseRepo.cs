using EcoSystemAPI.Context.Models;
using EcoSystemAPI.Data;
using EcoSystemAPI.Data.Context;
using EcoSystemAPI.uow.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoSystemAPI.uow.Repositories
{
    public class SqlMerchandiseRepo : IMerchandiseRepo
    {
        private readonly EcosystemContext _context;

        public SqlMerchandiseRepo (EcosystemContext context)
        {
            _context = context;
        }
        public void CreateMerch(Merchandise merch)
        {
            if(merch == null)
            {
                throw new ArgumentNullException(nameof(merch));
            }
            _context.Merchandise.Add(merch);
           
        }

        public void DeleteMerch(Merchandise merch)
        {
            if (merch == null)
            {
                throw new ArgumentNullException(nameof(merch));
            }
            _context.Remove(merch);
        }

        public IEnumerable<Merchandise> GetAllMerch()
        {
            return _context.Merchandise.ToList();
        }

        public Merchandise GetMerchById(int id)
        {
            return _context.Merchandise.FirstOrDefault(p => p.MerchId == id);
        }

        public void PartialAccountUpdate(Merchandise merch)
        {

        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateMerch(Merchandise merch)
        {

        }
    }
}
