using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.DataAccess.Repositories
{
    public class TournamentSponsorRepository : GenericRepository<TournamentSponsor>, ITournamentSponsorRepository
    {
        public TournamentSponsorRepository(LeagueDbContext context) : base(context)
        {
        }

        public async Task<TournamentSponsor?> GetByTournamentAndSponsorAsync(int tournamentId, int sponsorId)
        {
            var relation = await _dbSet
                .Include(ts => ts.Tournament)   
                .Include(ts => ts.Sponsor)      
                .FirstOrDefaultAsync(ts => ts.TournamentId == tournamentId && ts.SponsorId == sponsorId);

            return relation;
        }

        public async Task<IEnumerable<TournamentSponsor>> GetBySponsorAsync(int sponsorId)
        {
            var sponsorRelations = await _dbSet
                .Where(ts => ts.SponsorId == sponsorId)
                .Include(ts => ts.Tournament)
                .Include(ts => ts.Sponsor) 
                .ToListAsync();

            return sponsorRelations;
        }

        public async Task<IEnumerable<TournamentSponsor>> GetByTournamentAsync(int tournamentId)
        {
            var tournamentRelations = await _dbSet
                .Where(ts => ts.TournamentId == tournamentId)
                .Include(ts => ts.Sponsor)
                .Include(ts => ts.Tournament) 
                .ToListAsync();

            return tournamentRelations;
        }
    }
}
