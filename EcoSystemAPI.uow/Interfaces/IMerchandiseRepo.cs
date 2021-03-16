using EcoSystemAPI.Context.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcoSystemAPI.uow.Interfaces
{
    public interface IMerchandiseRepo
    {
        bool SaveChanges();
        IEnumerable<Merchandise> GetAllMerch();
        Merchandise GetMerchById(int id);
        void CreateMerch(Merchandise merch);
        void UpdateMerch(Merchandise merch);
        void DeleteMerch(Merchandise merch);

        void PartialAccountUpdate(Merchandise merch);

    }
}
