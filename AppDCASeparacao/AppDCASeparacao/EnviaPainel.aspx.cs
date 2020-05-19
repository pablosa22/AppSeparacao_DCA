using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppDCASeparacao
{
    public partial class EnviaPainel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btConfirmar_Click(object sender, EventArgs e)
        {
            int opcao = Convert.ToInt32(DropDownList1.SelectedItem.Value);            
            long numped = string.IsNullOrEmpty(TextBoxNumero.Text) ? 0 : Convert.ToInt64(TextBoxNumero.Text);
            ServiceReference1.WebService1SoapClient nn = new ServiceReference1.WebService1SoapClient();
            int EnvioPainel = Convert.ToInt32(ValidarIsEnvioPainel(numped, opcao));

            if (EnvioPainel != 0)
            {
                String mensagem = "Atenção! Já foi lançado no painel.";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "MensagemDeAlert", "alert('" + mensagem + "');", true);
            }
            else if (opcao == 1)
            {
                String mensagem = "Não foi possível enviar para o painel, selecione a opção!";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "MensagemDeAlert", "alert('" + mensagem + "');", true);
            }            
            else if (opcao == 2 && numped != 0) // OPÇÃO PEDIDO
            {
                nn.EnviaPedidoParaPainel(opcao, numped);
                String mensagem = "Pedido Nº " + numped + " enviado pra o painel!";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "MensagemDeAlert", "alert('" + mensagem + "');", true);
                TextBoxNumero.Text = "";
            }
            else if (opcao == 3 && numped != 0) // OPÇÃO CARREGAMENTO
            {
                nn.EnviaPedidoParaPainel(opcao, numped);
                String mensagem = "Carregamento " + numped + " enviado pra o painel!";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "MensagemDeAlert", "alert('" + mensagem + "');", true);
                TextBoxNumero.Text = "";
            }
            else
            {
                String mensagem = "Não foi possível enviar o pedido favor verificar as informações ou Verifique com a TI!";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "MensagemDeAlert", "alert('" + mensagem + "');", true);
            }
        }

        protected void btExcluir_Click(object sender, EventArgs e)
        {
            int opcao = Convert.ToInt32(DropDownList1.SelectedItem.Value);            
            long numped = string.IsNullOrEmpty(TextBoxNumero.Text) ? 0 : Convert.ToInt64(TextBoxNumero.Text);
            ServiceReference1.WebService1SoapClient nn = new ServiceReference1.WebService1SoapClient();

            if (opcao == 2 && numped != 0)
            {
                nn.ExcluirPedidoDoPainel(opcao, numped);
                String mensagem = "Pedido de Nº " + numped + " excluido do painel!";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "MensagemDeAlert", "alert('" + mensagem + "');", true);
                TextBoxNumero.Text = "";
            }
            else if (opcao == 3 && numped != 0)
            {
                nn.ExcluirPedidoDoPainel(opcao, numped);
                String mensagem = "Carregamento " + numped + " excluido do painel!";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "MensagemDeAlert", "alert('" + mensagem + "');", true);
                TextBoxNumero.Text = "";
            }
            else
            {
                String mensagem = "Não foi possível excluido do painel verificar as informações!";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "MensagemDeAlert", "alert('" + mensagem + "');", true);
            }
        }


        protected int ValidarIsEnvioPainel(long numcar, int opcao)
        {
            int TotLinhas = 0;
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            try
            {
                cnn.Open();
                if (opcao == 3)
                {
                    OracleCommand cmd = new OracleCommand("SELECT NVL(ORDEMCONF,0)ORDEMCONF FROM PCPEDC WHERE NUMCAR =:numcar ", cnn);
                    cmd.Parameters.Add(new OracleParameter("NUMCAR", numcar));
                    OracleDataReader rdr = cmd.ExecuteReader();

                    Produto produto = null;
                    while (rdr.Read())
                    {
                        produto = new Produto();
                        TotLinhas = Convert.ToInt32(rdr["ORDEMCONF"]);
                    }
                    rdr.Close();
                }
                else if (opcao == 2)
                {
                    OracleCommand cmd = new OracleCommand("SELECT NVL(ORDEMCONF,0)ORDEMCONF FROM PCPEDC WHERE NUMPED =:numcar ", cnn);
                    cmd.Parameters.Add(new OracleParameter("NUMPED", numcar));
                    OracleDataReader rdr = cmd.ExecuteReader();

                    Produto produto = null;
                    while (rdr.Read())
                    {
                        produto = new Produto();
                        TotLinhas = Convert.ToInt32(rdr["ORDEMCONF"]);
                    }
                    rdr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
            }
            return TotLinhas;
        }
    }
}