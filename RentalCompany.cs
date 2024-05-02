﻿using ScooterRental.Interfaces;
using ScooterRental.Scooters;

namespace ScooterRental
{
    public class RentalCompany : IRentalCompany
    {
        private readonly IScooterService _scooterService;
        private readonly IRentedScooterArchive _archive;
        private readonly IRentalCalculatorService _calculatorService;

        public RentalCompany(
            string name, 
            IScooterService scooterService, 
            IRentedScooterArchive archive,
            IRentalCalculatorService calculatorService )
        {
            Name = name;
            _scooterService = scooterService;
            _archive = archive;
            _calculatorService = calculatorService;
        }

        public string Name { get; }

        public void StartRent(string id)
        {
            var scooter = _scooterService.GetScooterById(id);
            _archive.AddRentedScooter(new RentedScooter(scooter.Id, DateTime.Now, scooter.PricePerMinute));
            scooter.IsRented = true;
        }

        public decimal EndRent(string id)
        {
            var scooter = _scooterService.GetScooterById(id);
            var rentalRecord = _archive.EndRental(scooter.Id, DateTime.Now);
            
            scooter.IsRented = false;

            return _calculatorService.CalculateRent(rentalRecord);
        }
        
        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            return _calculatorService.CalculateIncome(year, includeNotCompletedRentals);
        }
    }
}
