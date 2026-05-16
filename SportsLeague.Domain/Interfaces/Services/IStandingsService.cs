using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface IStandingsService

    {
        Task<object> GetStandingsAsync(int tournamentId);//Obtener la tabla de posiciones para un torneo específico
        Task<object> GetTopScorersAsync(int tournamentId);//Obtener la lista de los máximos goleadores para un torneo específico
        Task<object> GetCardStatsAsync(int tournamentId);//Obtener las estadísticas de tarjetas para un torneo específico
    }
}
