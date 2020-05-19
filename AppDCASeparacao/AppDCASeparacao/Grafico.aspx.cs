using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppDCASeparacao
{
    public partial class Grafico : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TotaldePedidoConferidoHoje();
            TotaldePedidoConferidoSemanaAnterior();
            TotaldePesoConferidoSemanaAnterior();
            TotaldePesoConferidoHoje();
            TotaldeItensConferidoHoje();
            TotaldeItensConferidoSemanaAnterior();
            ValorTotalConferidoHoje();
            ValorTotalConferidoSemanaAnterior();
            TotalConferentesHoje();
            TotalConferentesSemanaAnterior();
        }

        protected string ObterDados()
        {
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");

            cnn.Open();
            OracleCommand cmd = new OracleCommand("SELECT SUBSTR( PCEMPR.NOME, 1, INSTR( PCEMPR.NOME,' ')-1 )NOME, COUNT(DISTINCT(PCPEDI.NUMPED)) QTPEDIDOS FROM PCPEDI, PCPEDC, PCEMPR WHERE TRUNC(PCPEDC.DTFINALSEP) = TRUNC(SYSDATE) " +
                " AND PCPEDI.NUMPED = PCPEDC.NUMPED AND PCPEDC.CODFUNCSEP = PCEMPR.MATRICULA  AND PCPEDC.ORDEMCONF IS NOT NULL GROUP BY PCEMPR.NOME ", cnn);

            DataTable Dados = new DataTable();
            Dados.Load(cmd.ExecuteReader());
            cnn.Close();


            string strDados;

            strDados = "[['Task','Evolução'],";


            foreach (DataRow dr in Dados.Rows)
            {
                strDados = strDados + "[";
                strDados = strDados + "'" + dr[0] + "'" + "," + dr[1];
                strDados = strDados + "],";
            }
            strDados = strDados + "]";
            return strDados;
        }

        protected string ObterDadosPorItens()
        {
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");

            cnn.Open();
            OracleCommand cmd = new OracleCommand("SELECT SUBSTR( PCEMPR.NOME, 1, INSTR( PCEMPR.NOME,' ')-1 )NOME, COUNT(DISTINCT(PCPEDI.CODPROD)) QTITENS FROM PCPEDI, PCPEDC, PCEMPR WHERE TRUNC(PCPEDC.DTFINALSEP) = TRUNC(SYSDATE) " +
                " AND PCPEDI.NUMPED = PCPEDC.NUMPED AND PCPEDC.CODFUNCSEP = PCEMPR.MATRICULA AND PCPEDC.ORDEMCONF IS NOT NULL GROUP BY PCEMPR.NOME", cnn);

            DataTable Dados = new DataTable();
            Dados.Load(cmd.ExecuteReader());
            cnn.Close();


            string strDados;

            strDados = "[['Task','Evolução'],";


            foreach (DataRow dr in Dados.Rows)
            {
                strDados = strDados + "[";
                strDados = strDados + "'" + dr[0] + "'" + "," + dr[1];
                strDados = strDados + "],";
            }
            strDados = strDados + "]";
            return strDados;
        }

        protected int TotaldePedidoConferidoHoje()
        {
            int TotaldePedidos = 0;
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            try
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand("SELECT COUNT(DISTINCT(PCPEDI.NUMPED)) QTPEDIDOS FROM PCPEDI, PCPEDC, PCEMPR WHERE TRUNC(PCPEDC.DTFINALSEP) = TRUNC(SYSDATE) " +
                    " AND PCPEDI.NUMPED = PCPEDC.NUMPED AND PCPEDC.CODFUNCSEP = PCEMPR.MATRICULA AND PCPEDC.ORDEMCONF IS NOT NULL", cnn);
                OracleDataReader rdr = cmd.ExecuteReader();


                while (rdr.Read())
                {

                    TotaldePedidos = Convert.ToInt32(rdr["QTPEDIDOS"]);
                }
                rdr.Close();
                lbQtdePedido.Text = Convert.ToString(TotaldePedidos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
            }
            return TotaldePedidos;
        }

        protected int TotaldePedidoConferidoSemanaAnterior()
        {
            int TotaldePedidos = 0;
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            try
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand("SELECT COUNT(DISTINCT(PCPEDI.NUMPED)) QTPEDIDOS FROM PCPEDI, PCPEDC, PCEMPR WHERE TRUNC(PCPEDC.DTFINALSEP) = TRUNC(SYSDATE)-7 " +
                    " AND PCPEDI.NUMPED = PCPEDC.NUMPED AND PCPEDC.CODFUNCSEP = PCEMPR.MATRICULA AND PCPEDC.ORDEMCONF IS NOT NULL", cnn);
                OracleDataReader rdr = cmd.ExecuteReader();


                while (rdr.Read())
                {

                    TotaldePedidos = Convert.ToInt32(rdr["QTPEDIDOS"]);
                }
                rdr.Close();
                lbQtdePedidosOntem.Text = Convert.ToString(TotaldePedidos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
            }
            return TotaldePedidos;
        }

        protected decimal TotaldePesoConferidoHoje()
        {
            decimal TotaldePeso = 0;
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            try
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand("SELECT NVL(ROUND(SUM(NVL(PCPEDI.QT, 0) * NVL(PCPRODUT.PESOBRUTO, 0)),3),0) PESO FROM PCPEDC, PCPEDI, PCPRODUT " +
                    " WHERE PCPEDC.NUMPED = PCPEDI.NUMPED AND PCPEDI.CODPROD = PCPRODUT.CODPROD AND PCPEDC.CODFUNCSEP IS NOT NULL AND PCPEDC.DTFINALSEP IS NOT NULL AND PCPEDC.ORDEMCONF IS NOT NULL AND TRUNC(PCPEDC.DTFINALSEP) = TRUNC(SYSDATE) ", cnn); 
                OracleDataReader rdr = cmd.ExecuteReader();


                while (rdr.Read())
                {

                    TotaldePeso = Convert.ToDecimal(rdr["PESO"]);
                }
                rdr.Close();
                lbPesoAtual.Text = TotaldePeso + "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
            }
            return TotaldePeso;
        }

        protected decimal TotaldePesoConferidoSemanaAnterior()
        {
            decimal TotaldePeso = 0;
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            try
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand("SELECT NVL(ROUND(SUM(NVL(PCPEDI.QT, 0) * NVL(PCPRODUT.PESOBRUTO, 0)),3),0) PESO FROM PCPEDC, PCPEDI, PCPRODUT " +
                    " WHERE PCPEDC.NUMPED = PCPEDI.NUMPED AND PCPEDI.CODPROD = PCPRODUT.CODPROD AND PCPEDC.CODFUNCSEP IS NOT NULL AND PCPEDC.DTFINALSEP IS NOT NULL AND PCPEDC.ORDEMCONF IS NOT NULL AND TRUNC(PCPEDC.DTFINALSEP) = TRUNC(SYSDATE)-7 ", cnn);
                OracleDataReader rdr = cmd.ExecuteReader();


                while (rdr.Read())
                {
                    TotaldePeso = Convert.ToDecimal(rdr["PESO"]);
                }
                rdr.Close();
                lbPesoOntem.Text = TotaldePeso + "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
            }
            return TotaldePeso;
        }

        protected decimal TotaldeItensConferidoHoje()
        {
            int TotaldeItens = 0;
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            try
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand("SELECT COUNT(PCPEDI.CODPROD) QTITENS FROM PCPEDI, PCPEDC, PCEMPR WHERE TRUNC(PCPEDC.DTFINALSEP) = TRUNC(SYSDATE) AND PCPEDI.NUMPED = PCPEDC.NUMPED   AND PCPEDC.CODFUNCSEP = PCEMPR.MATRICULA AND PCPEDC.ORDEMCONF IS NOT NULL", cnn);
                OracleDataReader rdr = cmd.ExecuteReader();


                while (rdr.Read())
                {
                    TotaldeItens = Convert.ToInt32(rdr["QTITENS"]);
                }
                rdr.Close();
                lbItensAtual.Text = TotaldeItens + "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
            }
            return TotaldeItens;
        }

        protected decimal TotaldeItensConferidoSemanaAnterior()
        {
            int TotaldeItens = 0;
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            try
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand("SELECT COUNT(PCPEDI.CODPROD) QTITENS FROM PCPEDI, PCPEDC, PCEMPR WHERE TRUNC(PCPEDC.DTFINALSEP) = TRUNC(SYSDATE)-7 AND PCPEDI.NUMPED = PCPEDC.NUMPED   AND PCPEDC.CODFUNCSEP = PCEMPR.MATRICULA AND PCPEDC.ORDEMCONF IS NOT NULL", cnn);
                OracleDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    TotaldeItens = Convert.ToInt32(rdr["QTITENS"]);
                }
                rdr.Close();
                lbItensOntem.Text = TotaldeItens + "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
            }
            return TotaldeItens;
        }

        protected decimal ValorTotalConferidoSemanaAnterior()
        {
            decimal TotaldeItens = 0;
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            try
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand("SELECT NVL(ROUND(SUM(PCPEDI.QT * PCPEDI.PVENDA),2),0) VALORTOTAL FROM PCPEDI, PCPEDC, PCEMPR WHERE TRUNC(PCPEDC.DTFINALSEP) = TRUNC(SYSDATE)-7 AND PCPEDI.NUMPED = PCPEDC.NUMPED " +
                    " AND PCPEDC.CODFUNCSEP = PCEMPR.MATRICULA AND PCPEDC.ORDEMCONF IS NOT NULL", cnn);
                OracleDataReader rdr = cmd.ExecuteReader();


                while (rdr.Read())
                {
                    TotaldeItens = Convert.ToDecimal(rdr["VALORTOTAL"]);
                }
                rdr.Close();
                lbValorOntem.Text = TotaldeItens + "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
            }
            return TotaldeItens;
        }

        protected decimal ValorTotalConferidoHoje()
        {
            decimal TotaldeItens = 0;
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            try
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand("SELECT NVL(ROUND(SUM(PCPEDI.QT * PCPEDI.PVENDA),2),0) VALORTOTAL FROM PCPEDI, PCPEDC, PCEMPR WHERE TRUNC(PCPEDC.DTFINALSEP) = TRUNC(SYSDATE) AND PCPEDI.NUMPED = PCPEDC.NUMPED " +
                    " AND PCPEDC.CODFUNCSEP = PCEMPR.MATRICULA AND PCPEDC.ORDEMCONF IS NOT NULL", cnn);
                OracleDataReader rdr = cmd.ExecuteReader();


                while (rdr.Read())
                {
                    TotaldeItens = Convert.ToDecimal(rdr["VALORTOTAL"]);
                }
                rdr.Close();
                lbValorHoje.Text = TotaldeItens + "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
            }
            return TotaldeItens;
        }

        protected int TotalConferentesHoje()
        {
            int TotaldeItens = 0;
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            try
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand("SELECT COUNT(DISTINCT PCEMPR.MATRICULA)CONFERENTE FROM PCPEDI, PCPEDC, PCEMPR WHERE TRUNC(PCPEDC.DTFINALSEP) = TRUNC(SYSDATE) " +
                    " AND PCPEDI.NUMPED = PCPEDC.NUMPED AND PCPEDC.CODFUNCSEP = PCEMPR.MATRICULA AND PCPEDC.ORDEMCONF IS NOT NULL", cnn);
                OracleDataReader rdr = cmd.ExecuteReader();


                while (rdr.Read())
                {
                    TotaldeItens = Convert.ToInt32(rdr["CONFERENTE"]);
                }
                rdr.Close();
                lbQtdeConferente.Text = TotaldeItens + "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
            }
            return TotaldeItens;
        }

        protected int TotalConferentesSemanaAnterior()
        {
            int TotaldeItens = 0;
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            try
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand("SELECT COUNT(DISTINCT PCEMPR.MATRICULA)CONFERENTE FROM PCPEDI, PCPEDC, PCEMPR WHERE TRUNC(PCPEDC.DTFINALSEP) = TRUNC(SYSDATE)-7 " +
                    " AND PCPEDI.NUMPED = PCPEDC.NUMPED AND PCPEDC.CODFUNCSEP = PCEMPR.MATRICULA AND PCPEDC.ORDEMCONF IS NOT NULL", cnn);
                OracleDataReader rdr = cmd.ExecuteReader();


                while (rdr.Read())
                {
                    TotaldeItens = Convert.ToInt32(rdr["CONFERENTE"]);
                }
                rdr.Close();
                lbConferenteOnt.Text = TotaldeItens + "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
            }
            return TotaldeItens;
        }
    }
}