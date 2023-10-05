using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class ClienteRepository : GenericRepository<Cliente>
    {
        private readonly AnimalsContext _context;

        public ClienteRepository(AnimalsContext context) : base(context)
        {
            _context = context;
        }
    }
}