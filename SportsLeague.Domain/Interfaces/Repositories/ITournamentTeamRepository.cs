using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface ITournamentTeamRepository : IGenericRepository<TournamentTeam>
    {
        Task<TournamentTeam?> GetByTournamentAndTeamAsync(int tournamentId, int teamId);// Método para obtener la relación entre un torneo y un equipo específico
        Task<IEnumerable<TournamentTeam>> GetByTournamentAsync(int tournamentId);//Metodo para obtener todos los equipos registrados en un torneo específico
    }

}
