using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities;
public class ClienteTelefono : BaseEntity
{
    [Required]
    public int IdClienteFk { get; set; }
    public Cliente Clientes { get; set; }

    [Required]
    public string Numero { get; set; }
}
