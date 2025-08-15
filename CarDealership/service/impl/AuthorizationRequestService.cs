using System.Collections.Generic;
using CarDealership.entity;
using CarDealership.enums;
using CarDealership.repo;

namespace CarDealership.service.impl
{
    public class AuthorizationRequestService : IAuthorizationRequestService
    {
        private readonly IAuthorizationRequestRepository _repository;

        public AuthorizationRequestService(IAuthorizationRequestRepository repository)
        {
            _repository = repository;
        }

        public AuthorizationRequest CreateRequest(string login)
        {
            var request = new AuthorizationRequest
            {
                Login = login,
                Status = RequestStatus.Pending
            };

            return _repository.Add(request);
        }

        public AuthorizationRequest? GetRequestById(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<AuthorizationRequest> GetAllRequests()
        {
            return _repository.GetAll();
        }

        public bool UpdateRequest(int id, string? message = null, RequestStatus? status = null)
        {
            var request = _repository.GetById(id);
            if (request == null)
                return false;

            if (status != null)
                request.Status = status.Value;

            _repository.Update(request);
            return true;
        }

        public bool DeleteRequest(int id)
        {
            var request = _repository.GetById(id);
            if (request == null)
                return false;

            _repository.Delete(id);
            return true;
        }
    }
}