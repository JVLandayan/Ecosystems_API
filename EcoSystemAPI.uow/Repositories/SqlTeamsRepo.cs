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
    public class SqlTeamsRepo : ITeamsRepo
    {
        private readonly EcosystemContext _context;

        public SqlTeamsRepo(EcosystemContext context)
        {
            _context = context;
        }
        public void CreateTeam(Teams teams)
        {
            if (teams == null)
            {
                throw new ArgumentNullException(nameof(teams));
            }
            _context.Teams.Add(teams);
        }

        public void DeleteTeam(Teams teams)
        {
            if (teams == null)
            {
                throw new ArgumentNullException(nameof(teams));
            }
            _context.Remove(teams);
        }

        public IEnumerable<Teams> GetAllTeams()
        {
            return _context.Teams.ToList();
        }

        public Teams GetTeamById(int id)
        {
            return _context.Teams.FirstOrDefault(p => p.TeamsId == id);
        }

        public void PartialTeamsUpdate(Teams teams)
        {
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateTeam(Teams teams)
        {
            
        }
    }
}
