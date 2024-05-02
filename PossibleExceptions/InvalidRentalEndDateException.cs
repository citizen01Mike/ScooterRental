namespace ScooterRental.PossibleExceptions
{
    public class InvalidRentalEndDateException : Exception
    {
        public InvalidRentalEndDateException() : base("Invalid end date")
        {
            
        }
    }
}
