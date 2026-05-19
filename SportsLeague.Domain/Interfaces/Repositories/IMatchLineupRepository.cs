using System;
using System.Collections.Generic;
using System.Text;
using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories //CountStartersByMatchAndTeamAsync
{
    public interface IMatchLineupRepository : IGenericRepository<MatchLineup>
    {
        Task<IEnumerable<MatchLineup>> GetByMatchAsync(int matchId);

        Task<IEnumerable<MatchLineup>> GetByMatchWithDetailsAsync(int matchId);

        Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync( int matchId, int teamId);

        Task<bool> PlayerExistsInMatchAsync( int matchId, int playerId);

        Task<int> CountStartersByTeamAsync( int matchId, int teamId);
    }
}
