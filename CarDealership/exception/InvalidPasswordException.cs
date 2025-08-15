namespace CarDealership.exception;

public class InvalidPasswordException : CarDealershipException
{
    public InvalidPasswordException(string message)
        : base(message)
    {
    }
}