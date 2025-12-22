using Garage.Models;
using Humanizer.Localisation;
using Microsoft.Build.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

namespace Garage.Data
{
    public class GarageContext : DbContext
    {
        public GarageContext(DbContextOptions<GarageContext> options)
            : base(options)
        {
        }

        public DbSet<ParkedVehicle> ParkedVehicle { get; set; } = default!;

        // Seed data
        // create 5 different vehicles with different VehicleType, Registration, Color, Brand, Model, Wheels, and ParkTime
        // Use realistic data for each vehicle
        // Make sure the Ids are unique and start from 1
        // it will be used for testing purposes creating initial data in the database only once.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ParkedVehicle>().HasData(
        new ParkedVehicle
        {
            Id = 1,
            VehicleType = VehicleType.CAR,
            Registration = "ABC123",
            Color = "Red",
            Brand = "Volvo",
            Model = "XC60",
            Wheels = 4,
            ParkTime = new DateTime(2025, 12, 17, 9, 30, 0)
        },
        new ParkedVehicle
        {
            Id = 2,
            VehicleType = VehicleType.MOTORCYCLE,
            Registration = "MOTO77",
            Color = "Black",
            Brand = "Yamaha",
            Model = "MT-07",
            Wheels = 2,
            ParkTime = new DateTime(2025, 12, 17, 10, 15, 0)
        },
        new ParkedVehicle
        {
            Id = 3,
            VehicleType = VehicleType.ATV,
            Registration = "ATV999",
            Color = "Green",
            Brand = "Polaris",
            Model = "Sportsman",
            Wheels = 4,
            ParkTime = new DateTime(2025, 12, 17, 11, 0, 0)
        },
        new ParkedVehicle
        {
            Id = 4,
            VehicleType = VehicleType.BUS,
            Registration = "BUS001",
            Color = "White",
            Brand = "Scania",
            Model = "Citywide",
            Wheels = 6,
            ParkTime = new DateTime(2025, 12, 17, 8, 45, 0)
        },
        new ParkedVehicle
        {
            Id = 5,
            VehicleType = VehicleType.TRUCK,
            Registration = "TRK888",
            Color = "Blue",
            Brand = "MAN",
            Model = "TGX",
            Wheels = 8,
            ParkTime = new DateTime(2025, 12, 17, 7, 20, 0)
        }
             );
        }
    }
}
