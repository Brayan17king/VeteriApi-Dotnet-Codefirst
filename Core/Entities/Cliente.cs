using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities;
public class Cliente : BaseEntity //herencia
{
    [Required] // Este campo es requerido
    public string NombreCliente { get; set; }
    [Required]
    public string ApellidoCliente { get; set; }
    [Required]
    public string EmailCliente { get; set; }
}
