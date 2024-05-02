using ScooterRental.Interfaces;
using ScooterRental.PossibleExceptions;
using ScooterRental.Scooters;

namespace ScooterRental
{
    public class RentedScooterArchive : IRentedScooterArchive
    {
        private readonly List<RentedScooter> _rentedScootersList;

        public RentedScooterArchive(List<RentedScooter> rentedScootersList)
        {
            _rentedScootersList = rentedScootersList;
        }

        public void AddRentedScooter(RentedScooter scooter)
        {
            if (scooter == null)
            {
                throw new AddedScooterIsNullException();
            }

            if (_rentedScootersList.Any(sc => sc.ScooterId == scooter.ScooterId && sc.RentStart == scooter.RentStart))
            {
                throw new DuplicateScooterException();
            }
            
            _rentedScootersList.Add(scooter);
        }
        
        public RentedScooter EndRental(string scooterId, DateTime rentEnd)
        {
            var rentedScooter = _rentedScootersList.LastOrDefault(
                s => s.ScooterId == scooterId && s.RentEnd == null);

            if (rentedScooter == null)
            {
                throw new RentalScooterIsNullException();
            }

            if (rentEnd < rentedScooter.RentStart)
            {
                throw new InvalidRentalEndDateException();
            }

            rentedScooter.RentEnd = rentEnd;
            return rentedScooter;
        }

        public List<RentedScooter> AllRentedScooters()
        {
            return _rentedScootersList;
        }
    }
}
