using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities;
public class Cita : BaseEntity
{
[Required]
        public DateTime FechaCita { get; set; }

        [Required]
        public TimeSpan HoraCita { get; set; }

        [Required]
        public int IdClienteFk { get; set; }
        public Cliente Clientes { get; set; }
        [Required]
        public int IdMascotaFk { get; set; }
        public Mascota Mascotas { get; set; }
        [Required]
        public int IdServicioFk { get; set; }
        public Servicio Servicios { get; set; } 
    
}
