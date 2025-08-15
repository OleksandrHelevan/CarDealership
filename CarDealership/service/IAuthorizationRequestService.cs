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

        bool UpdateRequest(int id, string? message = null, RequestStatus? status = null);

        bool DeleteRequest(int id);
    }
}