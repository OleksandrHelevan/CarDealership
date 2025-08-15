using System.Collections.Generic;
using CarDealership.entity;

namespace CarDealership.repo
{
    public interface IAuthorizationRequestRepository
    {
        AuthorizationRequest Add(AuthorizationRequest request);

        AuthorizationRequest? GetById(int id);

        IEnumerable<AuthorizationRequest> GetAll();

        void Update(AuthorizationRequest request);

        void Delete(int id);
    }
}