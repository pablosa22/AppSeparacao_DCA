using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppDCASeparacao
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btEntrar_Click(object sender, EventArgs e)
        {
            int matricula = string.IsNullOrEmpty(TextBoxMatricula.Text) ? 0 : Convert.ToInt32(TextBoxMatricula.Text);
            string usuario = TextBoxUsuario.Text;
            ServiceReference1.WebService1SoapClient nn = new ServiceReference1.WebService1SoapClient();
            nn.ConfirmaAcesso(usuario, matricula);
            string name = nn.ConfirmaAcesso(usuario, matricula).Nome;            
            int filial = Convert.ToInt32(DropDownList2.SelectedItem.Value);

            if (name != null && filial != 0)
            {
                Response.Redirect("AppSeparacao.aspx?filial=" + filial + "&name=" + name + "&mat=" + matricula );
            }
            else
            {
                String mensagem = "Usuário ou senha incorreto!";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "MensagemDeAlert", "alert('" + mensagem + "');", true);
            }
        }

    }
    
}