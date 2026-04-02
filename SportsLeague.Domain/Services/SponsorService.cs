using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SportsLeague.Domain.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly ISponsorRepository _sponsorRepository;
        private readonly ITournamentSponsorRepository _tournamentSponsorRepository;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly ILogger<SponsorService> _logger;

        public SponsorService(
            ISponsorRepository sponsorRepository,
            ITournamentSponsorRepository tournamentSponsorRepository,
            ITournamentRepository tournamentRepository,
            ILogger<SponsorService> logger)
        {
            _sponsorRepository = sponsorRepository;
            _tournamentSponsorRepository = tournamentSponsorRepository;
            _tournamentRepository = tournamentRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Sponsor>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all sponsors");
            var sponsorList = await _sponsorRepository.GetAllAsync();
            return sponsorList;
        }

        public async Task<Sponsor?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving sponsor with ID: {SponsorId}", id);

            var sponsorEntity = await _sponsorRepository.GetByIdAsync(id);

            if (sponsorEntity == null)
                _logger.LogWarning("Sponsor with ID {SponsorId} not found", id);

            return sponsorEntity;
        }

        public async Task<Sponsor> CreateAsync(Sponsor sponsor)
        {
            // VALIDAR EMAIL
            if (!IsValidEmail(sponsor.ContactEmail))
            {
                throw new InvalidOperationException("El correo electrónico no tiene un formato válido");
            }

            // VALIDAR NOMBRE DUPLICADO
            var nameExists = await _sponsorRepository.ExistsByNameAsync(sponsor.Name);
            if (nameExists)
            {
                throw new InvalidOperationException("El nombre del sponsor ya se encuentra registrado");
            }

            _logger.LogInformation("Creating sponsor: {SponsorName}", sponsor.Name);

            var createdSponsor = await _sponsorRepository.CreateAsync(sponsor);

            return createdSponsor;
        }

        public async Task UpdateAsync(int id, Sponsor sponsor)
        {
            var existingSponsor = await _sponsorRepository.GetByIdAsync(id);

            if (existingSponsor == null)
            {
                throw new KeyNotFoundException($"No se encontró el sponsor con ID {id}");
            }

            // VALIDAR EMAIL
            if (!IsValidEmail(sponsor.ContactEmail))
            {
                throw new InvalidOperationException("El correo electrónico no es válido");
            }

            existingSponsor.Name = sponsor.Name;
            existingSponsor.ContactEmail = sponsor.ContactEmail;
            existingSponsor.Phone = sponsor.Phone;
            existingSponsor.WebsiteUrl = sponsor.WebsiteUrl;
            existingSponsor.Category = sponsor.Category;

            _logger.LogInformation("Updating sponsor with ID: {SponsorId}", id);

            await _sponsorRepository.UpdateAsync(existingSponsor);
        }

        public async Task DeleteAsync(int id)
        {
            var exists = await _sponsorRepository.ExistsAsync(id);

            if (!exists)
            {
                throw new KeyNotFoundException($"No se encontró el sponsor con ID {id}");
            }

            _logger.LogInformation("Deleting sponsor with ID: {SponsorId}", id);

            await _sponsorRepository.DeleteAsync(id);
        }

        public async Task<TournamentSponsor> RegisterSponsorAsync(int tournamentId,int sponsorId,decimal contractAmount)
        {
            //VALIDAR MONTO
            if (contractAmount <= 0)
            {
                throw new InvalidOperationException("El monto del contrato debe ser mayor a cero");
            }

            //  TORNEO
            var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);
            if (tournament == null)
            {
                throw new KeyNotFoundException($"No se encontró el torneo con ID {tournamentId}");
            }

            // SPONSOR
            var sponsor = await _sponsorRepository.GetByIdAsync(sponsorId);
            if (sponsor == null)
            {
                throw new KeyNotFoundException($"No se encontró el sponsor con ID {sponsorId}");
            }

            // DUPLICADO
            var relationExists = await _tournamentSponsorRepository
                .GetByTournamentAndSponsorAsync(tournamentId, sponsorId);

            if (relationExists != null)
            {
                throw new InvalidOperationException("El sponsor ya está vinculado a este torneo");
            }

            //  RELACIÓN
            var newRelation = new TournamentSponsor
            {
                TournamentId = tournamentId,
                SponsorId = sponsorId,
                ContractAmount = contractAmount,
                JoinedAt = DateTime.UtcNow,

                //  navegación
                Tournament = tournament,
                Sponsor = sponsor
            };

            _logger.LogInformation(
                "Registering sponsor {SponsorId} in tournament {TournamentId}",
                sponsorId, tournamentId);

            await _tournamentSponsorRepository.CreateAsync(newRelation);

            // RETORNO LA RELACION 
            return newRelation;
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsBySponsorAsync(int sponsorId)
        {
            var sponsorEntity = await _sponsorRepository.GetByIdAsync(sponsorId);

            if (sponsorEntity == null)
            {
                throw new KeyNotFoundException($"No se encontró el sponsor con ID {sponsorId}");
            }

            var relations = await _tournamentSponsorRepository.GetBySponsorAsync(sponsorId);

            var tournaments = relations.Select(r => r.Tournament);

            return tournaments;
        }

        public async Task RemoveSponsorAsync(int tournamentId, int sponsorId)
        {
            var relation = await _tournamentSponsorRepository
                .GetByTournamentAndSponsorAsync(tournamentId, sponsorId);

            if (relation == null)
            {
                throw new KeyNotFoundException("No existe la relación entre el sponsor y el torneo");
            }

            _logger.LogInformation(
                "Removing sponsor {SponsorId} from tournament {TournamentId}",
                sponsorId, tournamentId);

            await _tournamentSponsorRepository.DeleteAsync(relation.Id);
        }

        // MÉTODO MIO PRIVADO PARA VALIDAR EMAIL
        private bool IsValidEmail(string email)
        {
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
