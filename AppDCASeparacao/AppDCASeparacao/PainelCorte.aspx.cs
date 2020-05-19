using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppDCASeparacao
{
    public partial class PainelConte : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ServiceReference1.WebService1SoapClient nn = new ServiceReference1.WebService1SoapClient();
            this.GridView1.DataSource = nn.ListaItensDeCorte();
            this.GridView1.DataBind();
        }
    }
}