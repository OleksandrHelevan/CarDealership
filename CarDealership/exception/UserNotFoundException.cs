namespace CarDealership.exception
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() 
            : base("Користувача не знайдено.") { }

        public UserNotFoundException(string message) 
            : base(message) { }

        public UserNotFoundException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}