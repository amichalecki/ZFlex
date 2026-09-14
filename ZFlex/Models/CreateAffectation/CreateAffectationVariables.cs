using ZFlex.Models.MapView;

namespace ZFlex.Models.CreateAffectation
{
    public class CreateAffectationVariables
    {
        public Data Data { get; set; }

        public CreateAffectationVariables(Desk desk, Guid userId, DateOnly date, ParkingSpot? parkingSpot = null)
        {
            UserId userObj = new() { Id = userId };

            Data = new Data()
            {
                MainUserIdV2 = userObj,
                UsersIdV2 = [userObj],
                DatedMoments = [
                    new() { Date = date, Moment = "MORNING" },
                    new() { Date = date, Moment = "AFTERNOON" }
                ],
                SpacesIdSelection = [
                    desk.SpaceId
                ],
                DeskId = desk.Id
            };

            if (parkingSpot != null)
                Data.Services.Add(new(parkingSpot.Id, parkingSpot.SpaceId));
        }
    }

    public class Data
    {
        public string Type { get; set; } = "OFFICE";
        public List<DatedMoment> DatedMoments { get; set; } = new();
        public required UserId MainUserIdV2 { get; set; }
        public List<UserId> UsersIdV2 { get; set; } = new();
        public List<object> Teams { get; set; } = new();
        public List<object> GuestsInfo { get; set; } = new();
        public List<Guid> SpacesIdSelection { get; set; } = new();
        public Guid DeskId { get; set; }
        public List<Service> Services { get; set; } = new();
        public List<object> DesksAttributions { get; set; } = new();
        public bool WithUsersSelectedDays { get; set; } = true;
    }

    public class DatedMoment
    {
        public DateOnly Date { get; set; }
        public required string Moment { get; set; }
    }

    public class Service(Guid serviceId, Guid spaceId)
    {
        public Guid ServiceId { get; set; } = serviceId;
        public Guid SpaceId { get; set; } = spaceId;
        public string Type { get; set; } = "PARKING";
    }
}
