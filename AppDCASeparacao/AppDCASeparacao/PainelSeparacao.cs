﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppDCASeparacao
{
    public class PainelSeparacao
    {
        public int Carga { get; set; }
        public long Pedido { get; set; }
        public string Filial { get; set; }
        public int Codigo { get; set; }
        public string Cliente { get; set; }
        public string AreaExt { get; set; }
        public string AreaInt { get; set; }        
        public string Conferente { get; set; }
        public string Separado { get; set; }
    }
}