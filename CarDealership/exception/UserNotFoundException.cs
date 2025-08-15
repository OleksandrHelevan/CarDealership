namespace CarDealership.exception
{
    public class UserNotFoundException : CarDealershipException
    {
        public UserNotFoundException(string message)
            : base(message)
        {
        }
    }
}