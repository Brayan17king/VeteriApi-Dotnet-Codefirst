using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities;
public class Servicio : BaseEntity
{
    public string NombreServicio { get; set; }

    [Required]
    public double PrecioServicio { get; set; }
    public ICollection<Cita> Citas { get; set; }
}
