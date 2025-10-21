using CarDealership.config;
using CarDealership.entity;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.repo.impl
{
    public class AuthorizationRequestRepository : IAuthorizationRequestRepository
    {
        private readonly DealershipContext _context;

        public AuthorizationRequestRepository(DealershipContext context)
        {
            _context = context;
        }

        public AuthorizationRequest Add(AuthorizationRequest request)
        {
            _context.Set<AuthorizationRequest>().Add(request);
            _context.SaveChanges();
            return request;
        }

        public AuthorizationRequest? GetByLogin(string login)
        {
            return _context.Set<AuthorizationRequest>()
                .Include(r => r.User)
                .FirstOrDefault(r => r.User.Login == login);
        }
        public AuthorizationRequest? GetById(int id)
        {
            return _context.Set<AuthorizationRequest>()
                .Include(r => r.User)
                .FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<AuthorizationRequest> GetAll()
        {
            return _context.Set<AuthorizationRequest>()
                .Include(r => r.User)
                .ToList();
        }

        public void Update(AuthorizationRequest request)
        {
            _context.Set<AuthorizationRequest>().Update(request);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var request = _context.Set<AuthorizationRequest>().Find(id);
            if (request != null)
            {
                _context.Set<AuthorizationRequest>().Remove(request);
                _context.SaveChanges();
            }
        }
    }
}
