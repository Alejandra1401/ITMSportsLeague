using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Entities
{
    public class MatchLineup : AuditBase
    {
        public int MatchId { get; set; }
        public int PlayerId { get; set; }

        // false = Suplente,  true = Titular
        public bool IsStarter { get; set; }

        // GK, CB, CDM, ST
        public string Position { get; set; } = string.Empty;

        // Navigation Properties
        public Match Match { get; set; } = null!;
        public Player Player { get; set; } = null!;
    }
}
