using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface ISponsorRepository : IGenericRepository<Sponsor>
    {
        Task<IEnumerable<Sponsor>> GetByCategoryAsync(SponsorCategory category);

        Task<bool> ExistsByNameAsync(string name);
    }
}
