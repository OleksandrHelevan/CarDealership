using CarDealership.config;
using CarDealership.entity;

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
            return _context.Set<AuthorizationRequest>().FirstOrDefault(r => r.Login == login);
        }
        public AuthorizationRequest? GetById(int id)
        {
            return _context.Set<AuthorizationRequest>().FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<AuthorizationRequest> GetAll()
        {
            return _context.Set<AuthorizationRequest>().ToList();
        }

        public void Update(AuthorizationRequest request)
        {
            var existing = _context.Set<AuthorizationRequest>().FirstOrDefault(r => r.Id == request.Id);
            if (existing == null)
            {
                _context.Set<AuthorizationRequest>().Attach(request);
                _context.Entry(request).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            else
            {
                existing.Login = request.Login;
                existing.Status = request.Status;
                existing.ProcessedAt = request.ProcessedAt;
                existing.ProcessedBy = request.ProcessedBy;
                existing.Notes = request.Notes;
            }
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
