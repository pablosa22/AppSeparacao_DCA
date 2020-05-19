using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppDCASeparacao{
    public class Pedido
    {
        public long NumPedido { get; set; }
        public int Filial { get; set; }
        public int OrdemConf { get; set; }
        public int Maticula { get; set; }
        public decimal QtProduto { get; set; }
        public decimal QtItens { get; set; }
        public int CodCli { get; set; }
        public string Posicao { get; set; }         
    }
}