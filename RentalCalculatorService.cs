using ScooterRental.Interfaces;
using ScooterRental.PossibleExceptions;
using ScooterRental.Scooters;

namespace ScooterRental
{
    public class RentalCalculatorService : IRentalCalculatorService
    {
        private readonly decimal _maxRentCost = 20;
        private readonly IRentedScooterArchive _rentedScooterArchive;
        private readonly List<RentedScooter> _rentedScootersList;

        public RentalCalculatorService(IRentedScooterArchive rentedScooterArchive)
        {
            _rentedScooterArchive = rentedScooterArchive;
        }

        public decimal CalculateRent(RentedScooter rentalRecord)
        {
            if (rentalRecord == null)
            {
                throw new ScooterNotFoundException();
            }

            var totalCost = 0m;
            var rentEnd = rentalRecord.RentEnd ?? DateTime.Now;
            var currentDate = rentalRecord.RentStart.Date;

            while (currentDate <= rentEnd.Date)
            {
                totalCost += RentalPricePerDay(rentalRecord, currentDate);
                currentDate = currentDate.AddDays(1);
            }

            return totalCost;
        }

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            var totalIncome = 0m;
            var rentedScooters = _rentedScooterArchive.AllRentedScooters();
            foreach (var scooter in rentedScooters)
            {
                bool includeRental =
                    (year == null || scooter.RentStart.Year == year || (scooter.RentEnd?.Year == year));

                if (includeNotCompletedRentals || scooter.RentEnd.HasValue)
                {
                    if (includeRental)
                    {
                        totalIncome += CalculateRent(scooter);
                    }
                }
            }

            return totalIncome;
        }

        public decimal RentalPricePerDay(RentedScooter rentalRecord, DateTime day)
        {
            if (rentalRecord == null)
            {
                throw new ScooterHasNoRecordException();
            }

            if (day.Date < rentalRecord.RentStart.Date ||
                (rentalRecord.RentEnd.HasValue && day.Date > rentalRecord.RentEnd.Value.Date))
            {
                return 0m;
            }

            DateTime start;
            DateTime end;
            
            if (day.Date > rentalRecord.RentStart.Date)
            {
                start = day.Date;
            }
            else
            {
                start = rentalRecord.RentStart;
            }

            if (rentalRecord.RentEnd.HasValue && day.Date < rentalRecord.RentEnd.Value.Date)
            {
                end = day.Date.AddDays(1);
            }
            else
            {
                end = rentalRecord.RentEnd ?? DateTime.Now;
            }

            var minutesRented = (end - start).TotalMinutes;
            var costForDay = Math.Min((decimal)minutesRented * rentalRecord.PricePerMinute, _maxRentCost);

            return costForDay;
        }
    }
}
