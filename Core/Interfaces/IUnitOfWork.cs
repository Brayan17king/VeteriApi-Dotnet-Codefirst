using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        public IPaisRepository Paises { get; }
        public ICitaRepository Citas { get; }
        public ICiudadRepository Ciudades { get; }
        public IClienteRepository Clientes { get; }
        public IClienteDireccionRepository ClienteDirecciones { get; }
        public IClienteTelefonoRepository ClienteTelefonos { get; }
        public IDepartamentoRepository Departamentos { get; }
        public IMascotaRepository Mascotas { get; }
        public IRazaRepository Razas { get; }
        public IServicioRepository Servicios { get; }
        Task<int> SaveAsync();
    }
}