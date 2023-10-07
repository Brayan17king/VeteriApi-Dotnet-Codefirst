using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class CitaRepository : GenericRepository<Cita> ,ICitaRepository
    {
        private readonly AnimalsContext _context;

        public CitaRepository(AnimalsContext context) : base(context)
        {
           _context = context;
        }
    }
}