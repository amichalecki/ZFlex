using ZFlex.Client;
using ZFlex.Configuration;
using ZFlex.Models;
using ZFlex.Models.MapView;

namespace ZFlex.Services
{
    public class ZFlexService(AppSettings appSettings)
    {
        private readonly AppSettings _settings = appSettings;
        private readonly ZFlexClient _client = new(appSettings);

        public Task<User?> GetUserWithReservationsAsync(DateOnly date, CancellationToken cancellationToken = default)
            => _client.AffectationByUserAndDatesAsync(date, cancellationToken);

        public Task<Desk?> FindAvailableDeskAsync(DateOnly date, CancellationToken cancellationToken = default)
            => _client
                .GetWorkplaceAreasInfoAsync(date, cancellationToken)
                .ContinueWith(spaces
                    => spaces
                        .Result
                        .SelectMany(wrkPlc
                            => wrkPlc
                                .SpaceLeaves
                                .SelectMany(spLeaf
                                    => spLeaf
                                        .Desks
                                        .Where(desk
                                            => !spLeaf
                                                .Affectations
                                                .Select(aff => aff.DeskId)
                                                .Contains(desk.Id))))
                        .OrderBy(desk => desk.Name)
                        .ToList(), cancellationToken)
                .ContinueWith(availableDesks
                    => _settings
                        .DeskPriority
                        .Select(name
                            => availableDesks
                                .Result
                                .FirstOrDefault(awp => awp.Name?.Contains(name) ?? false))
                        .FirstOrDefault(desk => desk != null), cancellationToken);

        public Task<ParkingSpot?> FindAvailableParkingSpotAsync(DateOnly date, CancellationToken cancellationToken = default)
            => _client
                .GetParkingAreasInfoAsync(date, cancellationToken)
                .ContinueWith(spaces
                    => spaces
                        .Result
                        .SelectMany(prkArInfo
                             => prkArInfo
                                 .SpaceLeaves
                                 .SelectMany(spLeaf
                                     => spLeaf
                                         .ParkingSpots
                                         .Where(prkSpt
                                             => prkSpt.Availability.Count() == 2
                                             && (spLeaf.SpaceAvailibility?.IsAvailable ?? false))))
                        .OrderBy(sl => sl.Name)
                        .ToList(), cancellationToken)
                .ContinueWith(parkingSpots
                    => _settings
                        .ParkingPriority
                        .Select(name
                            => parkingSpots
                                .Result
                                .FirstOrDefault(aps => aps.Name?.Contains(name) ?? false))
                        .FirstOrDefault(spot => spot != null), cancellationToken);

        public Task<(bool DeskStatus, bool ParkingStatus)> CreateReservationAsync(DateOnly date, Desk desk, ParkingSpot? parkingSpot, CancellationToken cancellationToken = default)
            => _client
                .CreateAffectationAsync(desk, date, parkingSpot, cancellationToken)
                .ContinueWith(rsp =>
                {
                    var result = rsp.Result;
                    var deskStatus = result.Any() && rsp.Result.All(entry => entry.DeskId == desk!.Id);

                    return (
                        DeskStatus: deskStatus, 
                        ParkingStatus: deskStatus && result.All(entry => entry.Services.Any(srv => srv.ParkingSpotId == parkingSpot?.Id))
                    );
                }, cancellationToken);

        public Task<bool> DeleteReservationsAsync(IEnumerable<Guid> reservationIds, CancellationToken cancellationToken = default)
            => _client
                .DeleteAffectationsAsync(reservationIds, cancellationToken)
                .ContinueWith(rsp => rsp.Result.Success, cancellationToken);
    }
}
