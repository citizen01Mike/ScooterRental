namespace ScooterRental.PossibleExceptions
{
    public class InvalidPriceException : Exception
    {
        public InvalidPriceException() : base("Provided price is not valid")
        {
            
        }
    }
}
