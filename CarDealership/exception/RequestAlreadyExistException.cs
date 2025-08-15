namespace CarDealership.exception;

public class RequestAlreadyExistException : CarDealershipException
{
    public RequestAlreadyExistException(string message)
        : base(message)
    {
    }
}