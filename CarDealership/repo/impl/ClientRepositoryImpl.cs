using System.Collections.Generic;
using System.Linq;
using CarDealership.config;
using CarDealership.entity;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.repo.impl
{
    public class ClientRepository : IClientRepository
    {
        private readonly DealershipContext _context;

        public ClientRepository(DealershipContext context)
        {
            _context = context;
        }

        public IEnumerable<Client> GetAll()
        {
            return _context.Clients
                .Include(c => c.User)
                .Include(c => c.PassportData)
                .ToList();
        }

        public Client? GetById(int id)
        {
            return _context.Clients
                .Include(c => c.User)
                .Include(c => c.PassportData)
                .FirstOrDefault(c => c.Id == id);
        }

        public void Add(Client client)
        {
            _context.Clients.Add(client);
            _context.SaveChanges();
        }

        public void Update(Client client)
        {
            _context.Clients.Update(client);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var client = _context.Clients.Find(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
                _context.SaveChanges();
            }
        }
    }
}