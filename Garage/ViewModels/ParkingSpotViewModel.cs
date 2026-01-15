using Garage.Models;
using System.ComponentModel.DataAnnotations;

namespace Garage.ViewModels
{
    public class ParkingSpotViewModel
    {

        public int Id { get; }

        public int? ParkedCarID { get;}

        [Display(Name = "Parking Spot Size")]
        public string ParkingSpotSize { get; } = string.Empty;

        [Display(Name = "Name")]
        public string Name { get; }

        [Display(Name = "Blocked?")]
        public bool Blocked { get; } = false;

        [Display(Name = "Parked For")]
        public bool IsAvailable { get; }
          
        [Display(Name = "Parked Since")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime ParkTime { get; }

        [Display(Name = "Parked For")]
        public TimeSpan ParkedDuration { get; }


        public ParkingSpotViewModel(ParkingSpot spot)
        {
            ArgumentNullException.ThrowIfNull(spot);
            Id = spot.Id;
            Name = spot.Name;
            Blocked = spot.Blocked;
            ParkTime = spot.ParkTime;
            ParkedDuration = DateTime.Now - spot.ParkTime;
            IsAvailable = (ParkedCarID != null && !Blocked);
            switch (spot.ParkingSpotSize) {
                case 0: ParkingSpotSize = "Small"; break;
                case 1: ParkingSpotSize = "Medium"; break;
                case 2: ParkingSpotSize = "Large"; break;
            }
        }
    }
}