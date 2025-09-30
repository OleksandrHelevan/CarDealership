using System.Collections.Generic;
using CarDealership.entity;
using CarDealership.enums;
using CarDealership.exception;
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
            if (_repository.GetByLogin(login) != null)
                throw new RequestAlreadyExistException("Запит вже поданий, очікуйте відповіді");
            var request = new AuthorizationRequest
            {
                Login = login,
                Status = RequestStatus.Pending
            };

            return _repository.Add(request);
        }

        public AuthorizationRequest? GetRequestByLogin(string login)
        {
            return _repository.GetByLogin(login);
        }

        public IEnumerable<AuthorizationRequest> GetAllRequests()
        {
            return _repository.GetAll();
        }

        public bool UpdateRequest(AuthorizationRequest request)
        {
            var currentRequest = _repository.GetById(request.Id);

            currentRequest.Status = request.Status;

            _repository.Update(currentRequest);
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