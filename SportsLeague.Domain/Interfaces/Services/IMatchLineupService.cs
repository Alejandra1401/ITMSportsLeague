using SportsLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface IMatchLineupService
    {
     
        Task<MatchLineup> RegisterLineupAsync( int matchId, MatchLineup lineup);// Registrar alineación
        Task<IEnumerable<MatchLineup>> GetLineupByMatchAsync( int matchId);// Obtener toda la alineación del partido
        Task<IEnumerable<MatchLineup>> GetLineupByMatchAndTeamAsync( int matchId, int teamId);//Alineación por equipo   
        Task DeleteLineupAsync(int lineupId);// Eliminar jugador de la alineación
    }
}
