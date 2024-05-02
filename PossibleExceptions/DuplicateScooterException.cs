namespace ScooterRental.PossibleExceptions
{
    public class DuplicateScooterException : Exception
    {
        public DuplicateScooterException() : base("Scooter with Id already exists.")
        {

        }
    }
}
