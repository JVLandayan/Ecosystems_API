using EcoSystemAPI.Context.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcoSystemAPI.uow.Interfaces
{
    public interface ITeamsRepo
    {
        bool SaveChanges();
        IEnumerable<Teams> GetAllTeams();
        Teams GetTeamById(int id);
        void CreateTeam(Teams teams);
        void UpdateTeam(Teams teams);
        void DeleteTeam(Teams teams);
        void PartialTeamsUpdate(Teams teams); 


    }
}
