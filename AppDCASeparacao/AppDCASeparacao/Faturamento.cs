using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppDCASeparacao
{
    public class Faturamento
    {
        public long Carga { get; set; }
        public int QtPedido { get; set;}
        public decimal QtItens { get; set; }
        public int PedPentendente { get; set; }
        public int PedFinalizado { get; set; }
        public string Faturar { get; set; }
    }
}