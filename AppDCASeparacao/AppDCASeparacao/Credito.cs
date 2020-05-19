using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppDCASeparacao
{
    public class Credito
    {
        public int Filial { get; set; }
        public long Carga { get; set; }
        public long Pedido { get; set; }
        public string Posicao { get; set; }
        public string Cobranca { get; set; }
        public string Classe { get; set; }
        public int Codigo { get; set; }
        public string Cliente { get; set; }
        public string Bloq { get; set; }
        public string BloqDef { get; set; }

    }
}