using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class ClienteDireccionRepository : GenericRepository<ClienteDireccion>
    {
        private readonly AnimalsContext _context;

        public ClienteDireccionRepository(AnimalsContext context) : base(context)
        {
            _context = context;
        }
    }
}