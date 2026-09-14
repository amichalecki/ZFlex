using ZFlex.Configuration;
using ZFlex.Extensions;
using ZFlex.Services;

var appName = "ZFlex";
var settings = AppSettings.Load();
var logger = Logger.Create(settings.LogPath);

try
{
    var zflex = new ZFlexService(settings);

    var reservationDate = args.GetReservationDate(settings.ReservationDayUntilNow);

    var overwriteExistingReservation = args.HasParam("o");

    if (reservationDate.IsWorkingDay())
    {
        var user = await zflex.GetUserWithReservationsAsync(reservationDate);

        if (user != null && (!user.Affectations.Any() || overwriteExistingReservation))
        {
            if (overwriteExistingReservation && !await zflex.DeleteReservationsAsync(user.Affectations.Select(aff => aff.Id)))
            {
                logger.LogError("{0} was unable to overwrite existing reservation", appName);
                return;
            }

            var deskSpot = await zflex.FindAvailableDeskAsync(reservationDate);
            var parkingSpot = await zflex.FindAvailableParkingSpotAsync(reservationDate);

            if (deskSpot != null)
            {
                var result = await zflex.CreateReservationAsync(reservationDate, deskSpot, parkingSpot);

                if (result.DeskStatus)
                {
                    var parkingPart = result.ParkingStatus ? $"parking spot '{parkingSpot!.Name}' attached" : "no parking spot attached";
                    logger.LogInformation("{0} made a reservation for {1:yyyy-MM-dd} of desk '{2}', with {3}", appName, reservationDate, deskSpot.Name!, parkingPart);

                }
                else
                    logger.LogError("{0} was unable to make a reservation for {1:yyyy-MM-dd} of desk '{2}'", appName, reservationDate, deskSpot.Name!);
            }
        }
        else if (user == null)
            logger.LogError("{0} not found user with id {1}", appName, settings.UserId);
        else
        {
            logger.LogError("{0} detected already existing a reservation for {1:yyyy-MM-dd}", appName, reservationDate);
        }
    }
    else
        logger.LogWarning("{0} stops for {1} as it is {2}", appName, reservationDate, reservationDate.DayOfWeek.ToString());
}
catch (Exception ex)
{
    logger.LogError("An exception occurred: {0}", ex);
}
