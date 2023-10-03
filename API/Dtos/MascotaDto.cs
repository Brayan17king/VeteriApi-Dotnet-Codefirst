using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class MascotaDto
    {
        public int Id { get; set; }
        public string NombreMascota { get; set; }
        public string Especie { get; set; }
        public DateTime FechaNacimientoMascota { get; set; }
    }
}