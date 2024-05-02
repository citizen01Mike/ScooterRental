namespace ScooterRental.PossibleExceptions
{
    public class ScooterNotFoundException : Exception
    {
        public ScooterNotFoundException() : base("Scooter not found")
        {
            
        }
    }
}
