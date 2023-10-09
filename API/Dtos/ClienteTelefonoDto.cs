using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ClienteTelefonoDto
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public int IdClienteFk { get; set; }
    }
}