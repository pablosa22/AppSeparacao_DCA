using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppDCASeparacao
{
    public partial class AppSeparacao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int matricula = Convert.ToInt32(Request.QueryString["mat"]);
            string nome = Request.QueryString["name"].ToString();            
            LbNome.Text = nome;
            LbMatricula.Text = Convert.ToString(matricula);
            DesabilitarDigitação();
            btFinalizarConf.Visible = false;
        }

        protected void DesabilitarDigitação()
        {
            TextBoxCodigo.Visible = false;
            TextBoxDescricao.Visible = false;
            TextBoxQtPedida.Visible = false;
            TextBoxConferida.Visible = false;
            TextBoxEmbalagem.Visible = false;
            TextBoxCorte.Visible = false;
            btConfirmar.Visible = false;
            btProduto.Visible = false;
            lbCodigo.Visible = false;
            lbDescricao.Visible = false;
            lbQtPedida.Visible = false;
            lbQtConf.Visible = false;
            lbCorte.Visible = false;
            lbEmbalagem.Visible = false;
        }

        protected void btSair_Click(object sender, EventArgs e)
        {
            Response.Redirect("Index.aspx");
        }

        protected void btPesquisar_Click(object sender, EventArgs e)
        {            
            int separador = Convert.ToInt32(Request.QueryString["mat"]);
            int filial = Convert.ToInt32(Request.QueryString["filial"]);
            long numped = string.IsNullOrEmpty(TextBoxPedido.Text) ? 0 : Convert.ToInt64(TextBoxPedido.Text);
            ServiceReference1.WebService1SoapClient nn = new ServiceReference1.WebService1SoapClient();
            int matricula = nn.IniciaConferencia(numped,filial).Maticula;            
            int ordemConf = Convert.ToInt32(nn.IniciaConferencia(numped, filial).OrdemConf);
            string posicao = nn.IniciaConferencia(numped, filial).Posicao;         
            int qt_Itens_org = nn.ValidaConferenciaCompleta(numped, filial).QT_Itens_Org;
            int qt_Itens_conf = nn.ValidaConferenciaCompleta(numped, filial).QT_Itens_Conf;
            int achou = VerificaPedidoNaFilial(numped,filial);            

            if (achou == 0)
            {
                String mensagem = "Erro!! Verifique o pedido:" + numped + " e filial:" + filial;
                ClientScript.RegisterClientScriptBlock(this.GetType(), "MensagemDeAlert", "alert('" + mensagem + "');", true);
            }
            else if (!posicao.Equals("M"))
            {
                String mensagem = "Pedido não está como Montado " + numped;
                ClientScript.RegisterClientScriptBlock(this.GetType(), "MensagemDeAlert", "alert('" + mensagem + "');", true);
            }
            else if (numped == 0)
            {
                String mensagem = "Número do pedido é invalido :" + numped;
                ClientScript.RegisterClientScriptBlock(this.GetType(), "MensagemDeAlert", "alert('" + mensagem + "');", true);
            }
            else if (ordemConf != 1)
            {
                String mensagem = "Pedido não foi liberado para separação:" + numped;
                ClientScript.RegisterClientScriptBlock(this.GetType(), "MensagemDeAlert", "alert('" + mensagem + "');", true);
            }
            else if (posicao.Equals("M") && numped != 0 && ordemConf == 1)
            {
                if (matricula == 0 && ((qt_Itens_org) > qt_Itens_conf))
                {
                    nn.AtribuirPedidoParaSeparador(separador, numped, filial);
                    HabilitarBotaoPesquisar(numped, filial);
                }
                else if (matricula == separador && ((qt_Itens_org) > qt_Itens_conf))
                {
                    HabilitarBotaoPesquisar(numped, filial);
                }
                else if(matricula != separador)
                {
                    String mensagem = "Separação já iniciada pelo usuário! " + matricula;
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "MensagemDeAlert", "alert('" + mensagem + "');", true);
                }
            }
            else
            {
                String mensagem = "Erro não catalogado procure a TI!";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "MensagemDeAlert", "alert('" + mensagem + "');", true);
            }          
        }

        protected void HabilitarBotaoPesquisar(long numped, int filial)
        {
            ServiceReference1.WebService1SoapClient nn = new ServiceReference1.WebService1SoapClient();
            this.GridView1.DataSource = nn.ListaItensParaConferencia(numped, filial);
            this.GridView1.DataBind();
            this.GridView2.DataSource = nn.ListaItensFinalizados(numped, filial);
            this.GridView2.DataBind();            
            HabilitarDigitação();
        }

        protected void HabilitarDigitação()
        {
            TextBoxCodigo.Visible = true;
            TextBoxDescricao.Visible = true;
            TextBoxQtPedida.Visible = true;
            TextBoxConferida.Visible = true;
            TextBoxCorte.Visible = true;
            TextBoxEmbalagem.Visible = true;
            btConfirmar.Visible = true;
            btProduto.Visible = true;
            lbCodigo.Visible = true;
            lbDescricao.Visible = true;
            lbQtPedida.Visible = true;
            lbQtConf.Visible = true;
            lbCorte.Visible = true;
            lbEmbalagem.Visible = true;
            TextBoxPedido.Enabled = false;
            btPesquisar.Visible = false;
        }

        protected int VerificaPedidoNaFilial(long numped, int filial)
        {
            int TotLinhas = 0;
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            try
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand("SELECT COUNT(NUMPED)QT FROM PCPEDC C WHERE NUMPED =:numped AND CODFILIAL ="+filial, cnn);
                cmd.Parameters.Add(new OracleParameter("NUMPED", numped));
                OracleDataReader rdr = cmd.ExecuteReader();

                Produto produto = null;
                while (rdr.Read())
                {
                    produto = new Produto();
                    TotLinhas = Convert.ToInt32(rdr["QT"]);
                }
                rdr.Close();
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

        protected string UnidadeDeVenda(long numped, int filial, int codprod)
        {
            string Unidade = "";
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            try
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand("SELECT EMBALAGEM FROM PCEMBALAGEM E WHERE E.CODPROD ="+codprod+" AND E.CODFILIAL =" + filial+"  AND CODAUXILIAR IN " +
                    "(SELECT CODAUXILIAR FROM PCPEDI I WHERE I.NUMPED ="+numped+" AND I.CODPROD ="+codprod+")", cnn);
                cmd.Parameters.Add(new OracleParameter("NUMPED", numped));
                cmd.Parameters.Add(new OracleParameter("CODPROD", codprod));
                OracleDataReader rdr = cmd.ExecuteReader();

                Produto produto = null;
                if (rdr.Read())
                {
                    produto = new Produto();
                    Unidade = rdr["EMBALAGEM"].ToString();
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
            }
            return Unidade;
        }

        protected void btConfirmar_Click(object sender, EventArgs e)
        {
            int codigo = string.IsNullOrEmpty(TextBoxCodigo.Text) ? 0 : Convert.ToInt32(TextBoxCodigo.Text);
            long numped = string.IsNullOrEmpty(TextBoxPedido.Text) ? 0 : Convert.ToInt64(TextBoxPedido.Text);
            int matricula = Convert.ToInt32(Request.QueryString["mat"]);
            int filial = Convert.ToInt32(Request.QueryString["filial"]);
            decimal qt_separada = string.IsNullOrEmpty(TextBoxConferida.Text) ? 0 : Convert.ToDecimal(TextBoxConferida.Text);
            decimal qt_corte = string.IsNullOrEmpty(TextBoxCorte.Text) ? 0 : Convert.ToDecimal(TextBoxCorte.Text);
            ServiceReference1.WebService1SoapClient nn = new ServiceReference1.WebService1SoapClient();

            decimal quantidadeSep = (qt_separada + nn.SepararProduto(numped, codigo, filial).QtS);
            decimal quantidadeCorte = (qt_corte + nn.SepararProduto(numped, codigo, filial).QtC);

            decimal qtOrigem = Convert.ToDecimal(nn.SepararProduto(numped, codigo, filial).QtN);
            int numSeq = Convert.ToInt32(nn.SepararProduto(numped, codigo, filial).Seq);

            if (quantidadeCorte > qtOrigem)
            {
                String mensagem = "Quantidade de corte está incorreta, verifique!" + qt_corte;
                ClientScript.RegisterClientScriptBlock(this.GetType(), "MensagemDeAlert", "alert('" + mensagem + "');", true);
                HabilitarDigitação();
            }
            else if (quantidadeSep > qtOrigem)
            {
                String mensagem = "Quantidade separada está incorreta, verifique!" + quantidadeSep;
                ClientScript.RegisterClientScriptBlock(this.GetType(), "MensagemDeAlert", "alert('" + mensagem + "');", true);
                HabilitarDigitação();
            }
            else if (qt_separada == 0 && qt_corte == 0)
            {
                String mensagem = "Não foi informado a quantidade, verifique!";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "MensagemDeAlert", "alert('" + mensagem + "');", true);
                HabilitarDigitação();
            }
            else if (qtOrigem < (quantidadeCorte + quantidadeSep))
            {
                String mensagem = "Erro! Qtde conferida + Corte maior que a solicitada.";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "MensagemDeAlert", "alert('" + mensagem + "');", true);
                HabilitarDigitação();
            }
            else if (matricula != 0 && numped != 0 && codigo != 0 && 
                (quantidadeCorte != 0 || quantidadeSep != 0) && ((quantidadeSep + quantidadeCorte) <= qtOrigem))
            {
                nn.ConfirmaConferencia(qt_separada, qt_corte, matricula, numped, codigo, numSeq);
                nn.FinalizaConferenciaItem(numped, codigo, numSeq);
                HabilitarDigitação();
                this.AtualizarGrid();

                int qt_Itens_org = nn.ValidaConferenciaCompleta(numped, filial).QT_Itens_Org;
                int qt_Itens_conf = nn.ValidaConferenciaCompleta(numped, filial).QT_Itens_Conf;                
                int qtlinhas = TotalDeLinhasConferencia(numped, filial);

                if (qtlinhas == 0)
                {
                    InformaDataFimConferencia(qt_Itens_org, qt_Itens_conf);
                }
            }
            else
            {
                String mensagem = "Verifique a quantidade conferida: " + qt_separada;
                ClientScript.RegisterClientScriptBlock(this.GetType(), "MensagemDeAlert", "alert('" + mensagem + "');", true);
                HabilitarDigitação();   
            }
        }

        protected void btProduto_Click(object sender, EventArgs e)
        {
            HabilitarDigitação();
            long pedido = string.IsNullOrEmpty(TextBoxPedido.Text) ? 0 : Convert.ToInt64(TextBoxPedido.Text);
            int codigo = string.IsNullOrEmpty(TextBoxCodigo.Text) ? 0 : Convert.ToInt32(TextBoxCodigo.Text);
            int filial = Convert.ToInt32(Request.QueryString["filial"]);
            ServiceReference1.WebService1SoapClient nn = new ServiceReference1.WebService1SoapClient();
            string descricao = nn.SepararProduto(pedido, codigo, filial).Descricao;
            decimal qtPedida = nn.SepararProduto(pedido, codigo, filial).QtN;

            this.GridView4.DataSource = nn.ListaDetalhesDoPedido(pedido, filial, codigo);
            this.GridView4.DataBind();

            if (descricao == null || qtPedida == 0)
            {
                String mensagem = "Produto não consta na lista pra conferir: " + codigo;
                ClientScript.RegisterClientScriptBlock(this.GetType(), "MensagemDeAlert", "alert('" + mensagem + "');", true);
                TextBoxDescricao.Text = "";
                TextBoxQtPedida.Text = "";
                TextBoxCorte.Text = "";
            }
            if (descricao != null && qtPedida != 0)
            {
                string embalagem = UnidadeDeVenda(pedido, filial, codigo);
                TextBoxEmbalagem.Text = embalagem;
                TextBoxDescricao.Text = descricao;
                TextBoxQtPedida.Text = Convert.ToString(qtPedida);
            }
        }

        protected void InformaDataFimConferencia(int separacao, int conferencia)
        {
            if (separacao == conferencia)
            {
                btFinalizarConf.Visible = true;
            }
        }

        protected void AtualizarGrid()
        {
            long numped = string.IsNullOrEmpty(TextBoxPedido.Text) ? 0 : Convert.ToInt64(TextBoxPedido.Text);
            int filial = Convert.ToInt32(Request.QueryString["filial"]);
            ServiceReference1.WebService1SoapClient nn = new ServiceReference1.WebService1SoapClient();
            GridView1.DataSource = nn.ListaItensParaConferencia(numped, filial);
            GridView1.DataBind();
            GridView2.DataSource = nn.ListaItensFinalizados(numped, filial);
            GridView2.DataBind();
            LimparCampos();
        }

        protected void LimparCampos()
        {
            TextBoxDescricao.Text = "";
            TextBoxQtPedida.Text = "";
            TextBoxConferida.Text = "";
            TextBoxCodigo.Text = "";
            TextBoxEmbalagem.Text = "";
            TextBoxCorte.Text = "";
        }

        protected int TotalDeLinhasConferencia(long numped, int filial)
        {
            int TotLinhas = 0;
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            try
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand("SELECT I.* FROM PCPEDI I, PCPRODUT P, PCEMBALAGEM E WHERE I.CODPROD = P.CODPROD AND E.CODPROD = P.CODPROD AND I.CODFILIALRETIRA = " + filial + " AND E.CODFILIAL ="+filial+"  " +
                    " AND(I.QT / E.QTUNIT) <> NVL(NVL(I.QTSEPARADA, 0) + NVL(I.QTSEPARARUN , 0),0) AND I.NUMPED =: numped ORDER BY 2 ASC", cnn);
                cmd.Parameters.Add(new OracleParameter("NUMPED", numped));
                OracleDataReader rdr = cmd.ExecuteReader();

                Produto produto = null;
                while (rdr.Read())
                {
                    produto = new Produto();
                    TotLinhas++;
                }
                rdr.Close();
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

        protected void btFinalizarConf_Click1(object sender, EventArgs e)
        {
            ServiceReference1.WebService1SoapClient nn = new ServiceReference1.WebService1SoapClient();
            int matricula = Convert.ToInt32(Request.QueryString["mat"]);
            long numped = string.IsNullOrEmpty(TextBoxPedido.Text) ? 0 : Convert.ToInt64(TextBoxPedido.Text);
            nn.FinalizaConferencia(numped, matricula);
            LimparCampos();
            DesabilitarDigitação();
            TextBoxPedido.Visible = true;
            TextBoxPedido.Enabled = true;
            TextBoxPedido.Text = "";
            btPesquisar.Visible = true;
        }

    }
}