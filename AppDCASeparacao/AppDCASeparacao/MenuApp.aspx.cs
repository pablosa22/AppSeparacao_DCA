using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppDCASeparacao
{
    public partial class MenuApp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btRecebimento_Onclik(object sender, EventArgs e)
        {
            Response.Redirect("http://149.56.86.164:8080/Index.aspx");
        }

        protected void btSeparacao_Onclik(object sender, EventArgs e)
        {
            Response.Redirect("Index.aspx");
        }

    }
}