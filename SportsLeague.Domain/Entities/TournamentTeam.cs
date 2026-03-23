namespace SportsLeague.Domain.Entities
{
    public class TournamentTeam : AuditBase
    {
        public int TournamentId { get; set; }//FK TABLA DERECHA
        public int TeamId { get; set; }//FK TABLA IZQUIERDA
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Tournament Tournament { get; set; } = null!;
        public Team Team { get; set; } = null!;
    }

}
