using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppDCASeparacao
{
    public class PainelCorte
    {        
        public long Carga { get; set; }
        public long Pedido { get; set; }
        public string Cliente { get; set; }
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public string Und { get; set; }
        public string Emb { get; set; }
        public string Separador { get; set; }
        public decimal QtCorte { get; set; }
    }
}