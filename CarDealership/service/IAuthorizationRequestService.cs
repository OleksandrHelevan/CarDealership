using CarDealership.entity;

namespace CarDealership.service
{
    public interface IAuthorizationRequestService
    {
        AuthorizationRequest CreateRequest(string login);

        IEnumerable<AuthorizationRequest> GetAllRequests();

        bool UpdateRequest(AuthorizationRequest request);

    }
}