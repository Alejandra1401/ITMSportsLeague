using SportsLeague.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Entities
{
    public class Sponsor : AuditBase
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;

        public string? Phone { get; set; }
        public string? WebsiteUrl { get; set; }

        public SponsorCategory Category { get; set; }

        // Navigation Property
        public ICollection<TournamentSponsor> SponsorTournaments { get; set; } = new List<TournamentSponsor>();
    }
}
