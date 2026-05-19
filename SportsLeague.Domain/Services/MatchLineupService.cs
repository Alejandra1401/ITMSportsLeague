using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Helper;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;


namespace SportsLeague.Domain.Services
{
    public class MatchLineupService : IMatchLineupService
    {
        private readonly IMatchLineupRepository _matchLineupRepository;
        private readonly MatchValidationHelper _validationHelper;
        private readonly ILogger<MatchLineupService> _logger;

        public MatchLineupService(
            IMatchLineupRepository matchLineupRepository,
            MatchValidationHelper validationHelper,
            ILogger<MatchLineupService> logger)
        {
            _matchLineupRepository = matchLineupRepository;
            _validationHelper = validationHelper;
            _logger = logger;
        }

        public async Task<MatchLineup> RegisterLineupAsync(int matchId, MatchLineup lineup)
        {
            // V1 Y V6
            var match = await _validationHelper.ValidateMatchForLineupAsync(matchId);

            // V2 Y V3
            var player = await _validationHelper.ValidatePlayerInMatchAsync(lineup.PlayerId, match);

            // V4
            var playerExists = await _matchLineupRepository.PlayerExistsInMatchAsync(matchId, lineup.PlayerId);

            if (playerExists)
                throw new InvalidOperationException(
                    "El jugador ya está registrado en la alineación de este partido");

            // V5
            if (lineup.IsStarter)
            {
                var startersCount = await _matchLineupRepository.CountStartersByTeamAsync(matchId, player.TeamId);

                if (startersCount >= 11)
                    throw new InvalidOperationException(
                        "El equipo ya tiene 11 titulares registrados en este partido");
            }

            lineup.MatchId = matchId;

            _logger.LogInformation(
                "Registering lineup: Match {MatchId}, Player {PlayerId}, Starter {IsStarter}",
                matchId,
                lineup.PlayerId,
                lineup.IsStarter);

            return await _matchLineupRepository.CreateAsync(lineup);
        }

        public async Task<IEnumerable<MatchLineup>> GetLineupByMatchAsync(int matchId)
        {
            var match = await _validationHelper.ValidateMatchForLineupAsync(matchId);

            return await _matchLineupRepository.GetByMatchWithDetailsAsync(matchId);
        }

        public async Task<IEnumerable<MatchLineup>> GetLineupByMatchAndTeamAsync(int matchId, int teamId)
        {
            var match = await _validationHelper.ValidateMatchForLineupAsync(matchId);

            return await _matchLineupRepository.GetByMatchAndTeamAsync(matchId, teamId);
        }

        public async Task DeleteLineupAsync(int lineupId)
        {
            var exists = await _matchLineupRepository.ExistsAsync(lineupId);

            if (!exists)
                throw new KeyNotFoundException(
                    $"No se encontró la alineación con ID {lineupId}");

            _logger.LogInformation(
                "Deleting lineup with ID: {LineupId}",
                lineupId);

            await _matchLineupRepository.DeleteAsync(lineupId);
        }
    }
}
