using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class PaisRepository : GenericRepository<Pais>
    {
        private readonly AnimalsContext _context;

        public PaisRepository(AnimalsContext context) : base(context)
        {
            _context = context;
        }
    }
}