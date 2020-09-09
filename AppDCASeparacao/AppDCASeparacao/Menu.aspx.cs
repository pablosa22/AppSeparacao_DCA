using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppDCASeparacao
{
    public partial class Menu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btbtPainelSeparacao_Onclik(object sender, EventArgs e)
        {
            Response.Redirect("PainelSeparacao.aspx");
        }

        protected void btEnviaPainel_Onclik(object sender, EventArgs e)
        {
            Response.Redirect("EnviaPainel.aspx");
        }

                        
        protected void btbtPainelCorte_Onclik(object sender, EventArgs e)
        {
            Response.Redirect("PainelCorte.aspx");
        }

        protected void btGrafico_Onclik(object sender, EventArgs e)
        {
            Response.Redirect("Grafico.aspx");
        }

        protected void btAuditoria_Onclik(object sender, EventArgs e)
        {
            Response.Redirect("AuditoriaCliente.aspx");
        }

        protected void btPainelFaturamento_Onclik(object sender, EventArgs e)
        {
            Response.Redirect("PainelFaturamento.aspx");
        }
    }
}