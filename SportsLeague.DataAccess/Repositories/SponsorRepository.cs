using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace SportsLeague.DataAccess.Repositories
{
    public class SponsorRepository : GenericRepository<Sponsor>, ISponsorRepository
    {
        public SponsorRepository(LeagueDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Sponsor>> GetByCategoryAsync(SponsorCategory category)
        {
            var sponsorsByCategory = await _dbSet
                .Where(s => s.Category == category)
                .ToListAsync();

            return sponsorsByCategory;
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            var existingSponsor = await _dbSet
                .AnyAsync(s => s.Name.ToLower() == name.ToLower());

            return existingSponsor;
        }
    }
}
