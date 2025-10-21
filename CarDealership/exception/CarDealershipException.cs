namespace CarDealership.exception;
/*
 * It's the main exception class of this application
 * All exception in this project must extend this class 
 */
public class CarDealershipException : Exception
{
    public CarDealershipException(string message) : base(message) { }
}