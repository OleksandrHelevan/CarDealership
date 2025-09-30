using System.Collections.Generic;
using CarDealership.entity;
using CarDealership.enums;

namespace CarDealership.service
{
    public interface IAuthorizationRequestService
    {
        AuthorizationRequest CreateRequest(string login);

        AuthorizationRequest? GetRequestByLogin(string login);

        IEnumerable<AuthorizationRequest> GetAllRequests();

        bool UpdateRequest(AuthorizationRequest request);

        bool DeleteRequest(int id);
    }
}