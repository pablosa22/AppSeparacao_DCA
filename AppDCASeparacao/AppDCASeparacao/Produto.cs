using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppDCASeparacao
{
    public class Produto
    {
        public int Seq { get; set; }
        public int Rua { get; set; }
        public int Cod { get; set; }
        public string Descricao { get; set; }
        public decimal QtN { get; set; }
        public decimal QtS { get; set; }
        public decimal QtC { get; set; }
    }
}