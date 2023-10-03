using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ClienteDireccionDto
    {
        public int Id { get; set; }
        public string TipoDeVia { get; set; }
        public int NumeroPri { get; set; }
        public string Letra { get; set; }
        public string Bis { get; set; }
        public string LetraSec { get; set; }
        public string Cardinal { get; set; }
        public int NumeroSec { get; set; }
        public string LetraTer { get; set; }
        public int NumeroTer { get; set; }
        public string CardinalSec { get; set; }
        public string Complemento { get; set; }
        public string CodigoPostal { get; set; }
    }
}