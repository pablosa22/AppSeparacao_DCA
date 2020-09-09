using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppDCASeparacao
{
    public partial class PainelFaturamento : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ServiceReference1.WebService1SoapClient nn = new ServiceReference1.WebService1SoapClient();
            this.GridView1.DataSource = nn.ListaPedidosParaFaturar();
            this.GridView1.DataBind();

            foreach (GridViewRow row in GridView1.Rows)
            {
                if (row.Cells[5].Text == "SIM")
                {
                    row.Cells[5].BackColor = ColorTranslator.FromHtml("#D2691E");
                }
            }
        }
    }
}