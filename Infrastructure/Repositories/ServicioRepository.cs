using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class ServicioRepository : GenericRepository<Servicio>, IServicioRepository
    {
        private readonly AnimalsContext _context;

        public ServicioRepository(AnimalsContext context) : base(context)
        {
            _context = context;
        }
    }
}