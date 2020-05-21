using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace AppDCASeparacao
{
    /// <summary>
    /// Descrição resumida de WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que esse serviço da web seja chamado a partir do script, usando ASP.NET AJAX, remova os comentários da linha a seguir. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        //AppCelular Valida usuario e matricula
        [WebMethod]
        public Pessoa ConfirmaAcesso(string usuario, int matricula)
        {
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            Pessoa pessoa = new Pessoa();
            try
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand("SELECT MATRICULA, NOME, NOME_GUERRA FROM PCEMPR WHERE MATRICULA =:matricula AND NOME_GUERRA =:usuario", cnn);
                cmd.Parameters.Add(new OracleParameter("MATRICULA", matricula));
                cmd.Parameters.Add(new OracleParameter("NOME_GUERRA", usuario));
                OracleDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    pessoa.Matricula = Convert.ToInt32(rdr["MATRICULA"]);
                    pessoa.Nome = rdr["NOME"].ToString();
                    pessoa.Usuario = rdr["NOME_GUERRA"].ToString();
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
            return pessoa;
        }

        //AppCelular Inicia o processo de conferencia de mercadoria por pedido
        [WebMethod]
        public Pedido IniciaConferencia(long numped, int filial)
        {
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            Pedido pedido = new Pedido();
            try
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand("SELECT C.NUMPED, C.CODFILIAL, NVL(C.ORDEMCONF,0)ORDEMCONF, NVL(C.CODFUNCSEP, 0)AS CODFUNCSEP, COUNT(DISTINCT(P.CODPROD)) AS QT_PRODUTOS, SUM(I.QT)AS QT_ITENS, C.CODCLI, C.POSICAO " +
                    " FROM PCPEDC C, PCPEDI I, PCPRODUT P WHERE C.NUMPED = I.NUMPED AND P.CODPROD = I.CODPROD AND C.NUMPED =: numped AND C.POSICAO = 'M' AND C.CODFILIAL =:filial GROUP BY C.NUMPED, C.CODFILIAL, C.ORDEMCONF, C.CODFUNCSEP,C.CODCLI, C.POSICAO", cnn);
                cmd.Parameters.Add(new OracleParameter("NUMPED", numped));
                cmd.Parameters.Add(new OracleParameter("CODFILIAL", filial));
                OracleDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    int separador = string.IsNullOrEmpty(rdr["CODFUNCSEP"].ToString()) ? 0 : Convert.ToInt32(rdr["CODFUNCSEP"]);
                    pedido.NumPedido = Convert.ToInt64(rdr["NUMPED"]);
                    pedido.Filial = Convert.ToInt32(rdr["CODFILIAL"]);
                    pedido.OrdemConf = Convert.ToInt32(rdr["ORDEMCONF"]);
                    pedido.Maticula = separador;
                    pedido.QtProduto = Convert.ToDecimal(rdr["QT_PRODUTOS"]);
                    pedido.QtItens = Convert.ToDecimal(rdr["QT_ITENS"]);
                    pedido.CodCli = Convert.ToInt32(rdr["CODCLI"]);
                    pedido.Posicao = rdr["POSICAO"].ToString();                    
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
            return pedido;
        }

        //AppCelular Valida conferencia se já foi finalizada por pedido
        [WebMethod]
        public Conferencia ValidaConferenciaCompleta(long numped, int filial)
        {
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            Conferencia conferencia = new Conferencia();
            try
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand("SELECT COUNT(I.CODFUNCSEP)QT_ITENS_SEP, COUNT(I.CODPROD)QT_ITENS_ORG " +
                    " FROM PCPEDI I, PCPEDC C WHERE I.NUMPED = C.NUMPED AND I.NUMPED =:numped AND C.CODFILIAL = "+ filial, cnn);
                cmd.BindByName = true;
                cmd.Parameters.Add(new OracleParameter("NUMPED", numped));                
                OracleDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    conferencia.QT_Itens_Conf = Convert.ToInt32(rdr["QT_ITENS_SEP"]);
                    conferencia.QT_Itens_Org = (Convert.ToInt32(rdr["QT_ITENS_ORG"]));                    
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
            return conferencia;
        }

        //AppCelular lista itens pra conferir
        [WebMethod]
        public List<Produto> ListaItensParaConferencia(long numped, int filial)
        {
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            List<Produto> list = new List<Produto>();
            try
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand("SELECT RUA, SEQ, CODPROD, DESCRICAO, QTN, QTS, QTC FROM (SELECT P.RUA, P.CODPROD, P.DESCRICAO, I.NUMSEQ AS SEQ, NVL(SUM(I.QT / E.QTUNIT), 0) AS QTN, NVL(I.QTSEPARADA, 0)AS QTS, NVL(I.QTSEPARARUN , 0)AS QTC  " +
                    " FROM PCPEDI I, PCPRODUT P, PCEMBALAGEM E WHERE I.CODPROD = P.CODPROD AND E.CODPROD = P.CODPROD AND I.CODFILIALRETIRA ="+filial+" AND E.CODFILIAL ="+filial+ " AND I.NUMPED =:numped AND I.CODAUXILIAR = E.CODAUXILIAR " +
                    " GROUP BY P.RUA, P.CODPROD, P.DESCRICAO, I.NUMSEQ, I.QTSEPARADA, I.QTSEPARARUN) WHERE QTN <> (QTS + QTC) GROUP BY RUA, SEQ, CODPROD, DESCRICAO, QTN, QTS, QTC ORDER BY RUA, DESCRICAO ASC ", cnn);
                cmd.BindByName = true;
                cmd.Parameters.Add(new OracleParameter("NUMPED", numped));                
                OracleDataReader rdr = cmd.ExecuteReader();

                Produto produto = null;
                while (rdr.Read())
                {
                    produto = new Produto();

                    produto.Rua = Convert.ToInt32(rdr["RUA"]);
                    produto.Seq = Convert.ToInt32(rdr["SEQ"]);
                    produto.Cod = Convert.ToInt32(rdr["CODPROD"]);
                    produto.Descricao = rdr["DESCRICAO"].ToString();
                    produto.QtN = Convert.ToDecimal(rdr["QTN"]);
                    produto.QtS = Convert.ToDecimal(rdr["QTS"]);
                    produto.QtC = Convert.ToDecimal(rdr["QTC"]);
                    list.Add(produto);
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
            return list;
        }

        //AppCelular lista itens já finalizados
        [WebMethod]
        public List<Produto> ListaItensFinalizados(long numped, int filial)
        {
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            List<Produto> list = new List<Produto>();
            try
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand("SELECT RUA, SEQ, CODPROD, DESCRICAO, QTN, QTS, QTC FROM (SELECT P.RUA, P.CODPROD, P.DESCRICAO, I.NUMSEQ AS SEQ, NVL(SUM(I.QT / E.QTUNIT), 0) AS QTN, NVL(I.QTSEPARADA, 0)AS QTS, NVL(I.QTSEPARARUN , 0)AS QTC  " +
                    " FROM PCPEDI I, PCPRODUT P, PCEMBALAGEM E WHERE I.CODPROD = P.CODPROD AND E.CODPROD = P.CODPROD AND I.CODFILIALRETIRA =" + filial + " AND E.CODFILIAL =" + filial + " AND I.NUMPED =:numped AND I.CODAUXILIAR = E.CODAUXILIAR " +
                    " GROUP BY P.RUA, P.CODPROD, P.DESCRICAO, I.NUMSEQ, I.QTSEPARADA, I.QTSEPARARUN) WHERE QTN = (QTS + QTC) GROUP BY RUA, SEQ, CODPROD, DESCRICAO, QTN, QTS, QTC ORDER BY RUA, DESCRICAO ASC", cnn);
                cmd.BindByName = true;
                cmd.Parameters.Add(new OracleParameter("NUMPED", numped));
                cmd.Parameters.Add(new OracleParameter("CODFILIAL", filial));
                OracleDataReader rdr = cmd.ExecuteReader();

                Produto produto = null;
                while (rdr.Read())
                {
                    produto = new Produto();

                    produto.Rua = Convert.ToInt32(rdr["RUA"]);
                    produto.Seq = Convert.ToInt32(rdr["SEQ"]);
                    produto.Cod = Convert.ToInt32(rdr["CODPROD"]);
                    produto.Descricao = rdr["DESCRICAO"].ToString();
                    produto.QtN = Convert.ToDecimal(rdr["QTN"]);
                    produto.QtS = Convert.ToDecimal(rdr["QTS"]);
                    produto.QtC = Convert.ToDecimal(rdr["QTC"]);
                    list.Add(produto);
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
            return list;
        }

        //AppCelular lista detalhes do pedido campos OBS
        [WebMethod]
        public List<DetalhePed> ListaDetalhesDoPedido(long numped, int filial, int codprod)
        {
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            List<DetalhePed> list = new List<DetalhePed>();
            try
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand("SELECT C.NUMPED, P.RUA, P.NUMERO, P.APTO, C.OBS, C.OBS1, C.OBS2 FROM PCPEDC C, PCPEDI I, PCPRODUT P WHERE C.NUMPED = I.NUMPED AND I.CODPROD = P.CODPROD AND C.NUMPED =:numped AND P.CODPROD =:codprod AND C.CODFILIAL =" + filial, cnn);
                cmd.BindByName = true;
                cmd.Parameters.Add(new OracleParameter("NUMPED", numped));
                cmd.Parameters.Add(new OracleParameter("CODPROD", codprod));
                OracleDataReader rdr = cmd.ExecuteReader();

                DetalhePed detalhes = null;

                while (rdr.Read())
                {
                    detalhes = new DetalhePed();

                    detalhes.Pedido = Convert.ToInt64(rdr["NUMPED"]);
                    detalhes.Rua = rdr["RUA"].ToString();
                    detalhes.Numero = rdr["NUMERO"].ToString();
                    detalhes.Apto = rdr["APTO"].ToString();
                    detalhes.Obs = rdr["OBS"].ToString();
                    detalhes.Obs1 = rdr["OBS1"].ToString();
                    detalhes.Obs2 = rdr["OBS2"].ToString();
                    list.Add(detalhes);
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
            return list;
        }

        //AppCelular Vincular pedido ao conferente 
        [WebMethod]
        public void AtribuirPedidoParaSeparador(int matricula, long numped, int filial)
        {
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            try
            {
                if (matricula != 0 && numped != 0)
                {
                    cnn.Open();
                    OracleCommand cmd = new OracleCommand("UPDATE PCPEDC SET CODFUNCSEP =:matricula, DTINICIALSEP = SYSDATE WHERE NUMPED =:numped AND CODFILIAL =:filial", cnn);
                    cmd.Parameters.Add(new OracleParameter("CODFUNCSEP", matricula));
                    cmd.Parameters.Add(new OracleParameter("NUMPED", numped));
                    cmd.Parameters.Add(new OracleParameter("CODFILIAL", filial));
                    cmd.ExecuteNonQuery();
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
        }

        //AppCelular valida quandidade pedida e quantidade conferida          
        [WebMethod]
        public Produto SepararProduto(long numped, int codprod, int filial)
        {
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            Produto produto = new Produto();
            try
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand("SELECT NUMSEQ, CODPROD, DESCRICAO, QT, QTSEPARADA FROM(SELECT NUMSEQ, P.CODPROD, P.DESCRICAO, SUM(I.QT / E.QTUNIT)AS QT, NVL(I.QTSEPARADA, 0) AS QTSEPARADA " +
                    " FROM PCPEDI I, PCPRODUT P, PCEMBALAGEM E WHERE I.CODPROD = P.CODPROD AND E.CODPROD = P.CODPROD AND I.CODFILIALRETIRA ="+filial+"  AND E.CODFILIAL ="+filial+ " AND I.CODAUXILIAR = E.CODAUXILIAR AND I.NUMPED =:numped AND I.CODPROD =:codprod AND NUMSEQ IN(SELECT MIN(NUMSEQ) FROM PCPEDI WHERE NUMPED =:numped AND CODPROD =:codprod AND DTFINALSEP IS NULL) " +
                    " GROUP BY I.NUMSEQ, P.CODPROD, P.DESCRICAO, I.QT, E.QTUNIT, I.QTSEPARADA ORDER BY 2 ASC)  WHERE QT<> QTSEPARADA GROUP BY NUMSEQ, CODPROD, DESCRICAO, QT, QTSEPARADA ", cnn);
                cmd.BindByName = true;
                cmd.Parameters.Add(new OracleParameter("NUMPED", numped));
                cmd.Parameters.Add(new OracleParameter("CODPROD", codprod));                
                OracleDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    produto.Seq = Convert.ToInt32(rdr["NUMSEQ"]);
                    produto.Cod = Convert.ToInt32(rdr["CODPROD"]);
                    produto.Descricao = rdr["DESCRICAO"].ToString();
                    produto.QtN = Convert.ToDecimal(rdr["QT"]);
                    produto.QtS = Convert.ToDecimal(rdr["QTSEPARADA"]);
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
            return produto;
        }

        //AppCelular Inclui quantidade conferida no pedido por produto
        [WebMethod]
        public void ConfirmaConferencia(decimal qt_digitada, decimal qt_corte, int matricula, long numped, int codigo, int numSeq)
        {
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            try
            {
                if (matricula != 0 && numped != 0 && codigo != 0 && (qt_digitada != 0 || qt_corte != 0))
                {
                    cnn.Open();
                    OracleCommand cmd = new OracleCommand("UPDATE PCPEDI I SET QTSEPARADA = NVL(I.QTSEPARADA,0) + :qt_digitada, I.QTSEPARARUN = NVL(I.QTSEPARARUN,0) + :qt_corte, I.DTINICIALSEP = SYSDATE, I.CODFUNCSEP =:matricula " +
                        "WHERE I.NUMPED =:numped AND I.CODPROD =:codigo AND I.NUMSEQ =:numSeq", cnn);
                    cmd.Parameters.Add(new OracleParameter("QTSEPARADA", qt_digitada));
                    cmd.Parameters.Add(new OracleParameter("QTSEPARARUN", qt_corte));
                    cmd.Parameters.Add(new OracleParameter("CODFUNCSEP", matricula));
                    cmd.Parameters.Add(new OracleParameter("NUMPED", numped));
                    cmd.Parameters.Add(new OracleParameter("CODPROD", codigo));
                    cmd.Parameters.Add(new OracleParameter("NUMSEQ", numSeq));
                    cmd.ExecuteNonQuery();
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
        }

        //AppCelular Finaliza Conferencia do item no pedido
        [WebMethod]
        public void FinalizaConferenciaItem(long numped, int codigo, int numSeq)
        {
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            try
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand("UPDATE PCPEDI I SET DTFINALSEP = SYSDATE WHERE I.NUMPED =:numped AND I.CODPROD =:codigo AND I.NUMSEQ =:numSeq " +
                    " AND I.CODAUXILIAR IN(SELECT CODAUXILIAR FROM PCEMBALAGEM WHERE CODPROD =:codigo AND CODFILIAL IN(SELECT CODFILIAL FROM PCPEDC WHERE NUMPED =:numped)) " +
                    " AND I.DTFINALSEP IS NULL", cnn);
                cmd.Parameters.Add(new OracleParameter("NUMPED", numped));
                cmd.Parameters.Add(new OracleParameter("CODPROD", codigo));
                cmd.Parameters.Add(new OracleParameter("NUMSEQ", numSeq));
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
            }
        }

        //Painel faturamento dispara pedido para processo de separação/conferencia
        [WebMethod]
        public void EnviaPedidoParaPainel(int opcao, long numero)
        {
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            try
            {
                if (opcao == 2 && numero != 0)
                {
                    cnn.Open();
                    OracleCommand cmd = new OracleCommand("UPDATE PCPEDC SET ORDEMCONF = 1, NUMVIASMAPASEP = 1, DTIMPORTACAO = SYSDATE WHERE NUMPED =:numero AND DATA > TRUNC(SYSDATE) - 220 AND ORDEMCONF IS NULL", cnn);
                    cmd.Parameters.Add(new OracleParameter("NUMPED", numero));                    
                    cmd.ExecuteNonQuery();

                    OracleCommand cmd1 = new OracleCommand("UPDATE PCCARREG C SET C.NUMVIASMAPA = 1 WHERE NUMCAR IN (SELECT NUMCAR FROM PCPEDC WHERE NUMPED =:numped AND DATA > TRUNC(SYSDATE) - 220 AND ORDEMCONF IS NULL)", cnn);
                    cmd1.Parameters.Add(new OracleParameter("NUMPED", numero));
                    cmd1.ExecuteNonQuery();

                    OracleCommand cmd2 = new OracleCommand("UPDATE PCPEDI SET QTSEPARADA = NULL WHERE NUMPED =:numero AND DATA > TRUNC(SYSDATE) - 220 AND ORDEMCONF IS NULL", cnn);
                    cmd.Parameters.Add(new OracleParameter("NUMPED", numero));
                    cmd2.ExecuteNonQuery();
                }
                else if (opcao == 3 && numero != 0)
                {
                    cnn.Open();
                    OracleCommand cmd = new OracleCommand("UPDATE PCPEDC SET ORDEMCONF = 1, NUMVIASMAPASEP = 1, DTIMPORTACAO = SYSDATE WHERE NUMCAR =:numero AND DATA > TRUNC(SYSDATE) - 220 AND ORDEMCONF IS NULL", cnn);
                    cmd.Parameters.Add(new OracleParameter("NUMCAR", numero));
                    cmd.ExecuteNonQuery();

                    OracleCommand cmd1 = new OracleCommand("UPDATE PCCARREG C SET C.NUMVIASMAPA = 1 WHERE NUMCAR =:numped", cnn);
                    cmd1.Parameters.Add(new OracleParameter("NUMCAR", numero));
                    cmd1.ExecuteNonQuery();

                    OracleCommand cmd2 = new OracleCommand("UPDATE PCPEDI SET QTSEPARADA = NULL WHERE NUMCAR =:numero AND DATA > TRUNC(SYSDATE) - 220 AND ORDEMCONF IS NULL", cnn);
                    cmd.Parameters.Add(new OracleParameter("NUMPED", numero));
                    cmd2.ExecuteNonQuery();
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
        }

        //Painel faturamento excluir pedido para processo de separação/conferencia
        [WebMethod]
        public void ExcluirPedidoDoPainel(int opcao, long numero)
        {
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            try
            {
                if (opcao == 2 && numero != 0)
                {
                    cnn.Open();
                    OracleCommand cmd = new OracleCommand("UPDATE PCPEDC SET ORDEMCONF = null, CODFUNCSEP = null, DTINICIALSEP = null, DTFINALSEP = null, DTEXPORTACAO = SYSDATE, NUMVIASMAPASEP = null WHERE NUMPED =:numero AND DATA > TRUNC(SYSDATE) - 120 AND DTFINALSEP IS NULL AND ORDEMCONF IS NOT NULL", cnn);
                    cmd.Parameters.Add(new OracleParameter("NUMPED", numero));                    
                    int numeroLinhas = cmd.ExecuteNonQuery();

                    if (numeroLinhas > 0)
                    {
                        OracleCommand cmd1 = new OracleCommand("UPDATE PCPEDI SET CODFUNCSEP = null, QTSEPARADA = null, QTSEPARARUN = null, DTINICIALSEP = null, DTFINALSEP = null WHERE NUMPED =:numero AND DATA > TRUNC(SYSDATE) - 120", cnn);
                        cmd1.Parameters.Add(new OracleParameter("NUMPED", numero));                        
                        cmd1.ExecuteNonQuery();
                    }

                }
                else if (opcao == 3 && numero != 0)
                {
                    cnn.Open();
                    OracleCommand cmd = new OracleCommand("UPDATE PCPEDC SET ORDEMCONF = null, CODFUNCSEP = null, DTINICIALSEP = null, DTFINALSEP = null, DTEXPORTACAO = SYSDATE, NUMVIASMAPASEP = null WHERE NUMCAR =:numero AND DATA > TRUNC(SYSDATE) - 120 AND DTFINALSEP IS NULL AND ORDEMCONF IS NOT NULL", cnn);
                    cmd.Parameters.Add(new OracleParameter("NUMCAR", numero));
                    int numeroLinhas = cmd.ExecuteNonQuery();

                    if (numeroLinhas > 0)
                    {
                        OracleCommand cmd1 = new OracleCommand("UPDATE PCPEDI SET CODFUNCSEP = null, QTSEPARADA = null, QTSEPARARUN = null, DTINICIALSEP = null, DTFINALSEP = null WHERE NUMCAR =:numero AND DATA > TRUNC(SYSDATE) - 120 ", cnn);
                        cmd1.Parameters.Add(new OracleParameter("NUMCAR", numero));
                        cmd1.ExecuteNonQuery();
                    }
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
        }

        //AppCelular Finaliza o pedido por conferente, informando o processo ao faturamento por meio de finalização
        [WebMethod]
        public void FinalizaConferencia(long numped, int conferente)
        {

            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            try
            {
                if (conferente != 0 && numped != 0)
                {
                    cnn.Open();
                    OracleCommand cmd = new OracleCommand("UPDATE PCPEDC C SET C.CODFUNCSEP =:conferente, C.DTFINALSEP = SYSDATE WHERE C.NUMPED =:numped AND ((SELECT COUNT(I.CODFUNCSEP) FROM PCPEDI I WHERE NUMPED =:numped) = (SELECT COUNT(I.CODPROD) FROM PCPEDI I WHERE NUMPED =:numped))", cnn);
                    cmd.Parameters.Add(new OracleParameter("CODFUNCSEP", conferente));
                    cmd.Parameters.Add(new OracleParameter("NUMPED", numped));
                    cmd.ExecuteNonQuery();
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
        }

        //Painel pedidos em processo de separação
        [WebMethod]
        public List<PainelSeparacao> ListaPedidosParaConferencia()
        {
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            List<PainelSeparacao> list = new List<PainelSeparacao>();
            try
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand("SELECT C.NUMCAR, C.NUMPED, C.CODCLI, SUBSTR(L.CLIENTE, 1, INSTR(L.CLIENTE,' ')-1 )||SUBSTR(L.CLIENTE, INSTR(L.CLIENTE, ' '), INSTR(L.CLIENTE, ' ',2))CLIENTE," +
                    " C.CODFILIAL, C.OBS, C.OBS1, C.OBS2, SUBSTR(E.NOME, 1, INSTR(E.NOME, ' ') - 1)CONFERENTE, I.PERC FROM PCPEDC C, PCCLIENT L, PCEMPR E,(SELECT I.NUMPED, ROUND(((COUNT(I.QTSEPARADA) * 100) / COUNT(I.QT)), 2) || '%' PERC FROM PCPEDI I GROUP BY I.NUMPED)I " +
                    " WHERE C.CODCLI = L.CODCLI AND E.MATRICULA(+) = C.CODFUNCSEP AND I.NUMPED = C.NUMPED AND DATA > TRUNC(SYSDATE) - 120 AND ORDEMCONF IS NOT NULL AND C.DTFINALSEP IS NULL AND C.DTFAT IS NULL ORDER BY C.DTIMPORTACAO ASC ", cnn);
                cmd.BindByName = true;
                OracleDataReader rdr = cmd.ExecuteReader();

                PainelSeparacao painelSeparacao = null;

                while (rdr.Read())
                {
                    painelSeparacao = new PainelSeparacao();

                    painelSeparacao.Carga = Convert.ToInt32(rdr["NUMCAR"]);
                    painelSeparacao.Pedido = Convert.ToInt64(rdr["NUMPED"]);
                    painelSeparacao.Codigo = Convert.ToInt32(rdr["CODCLI"]);
                    painelSeparacao.Cliente = rdr["CLIENTE"].ToString();
                    painelSeparacao.Filial = rdr["CODFILIAL"].ToString();
                    painelSeparacao.Obs = rdr["OBS"].ToString();
                    painelSeparacao.Obs1 = rdr["OBS1"].ToString();
                    painelSeparacao.Obs2 = rdr["OBS2"].ToString();
                    painelSeparacao.Conferente = rdr["CONFERENTE"].ToString();
                    painelSeparacao.Separado = rdr["PERC"].ToString();
                    list.Add(painelSeparacao);
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
            return list;

        }

        //Painel pedidos em processo de separação
        [WebMethod]
        public List<PainelCorte> ListaItensDeCorte()
        {
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            List<PainelCorte> list = new List<PainelCorte>();
            try
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand("SELECT C.NUMCAR, C.NUMPED, T.CLIENTE, P.CODPROD, P.DESCRICAO, E.UNIDADE, E.EMBALAGEM, SUBSTR(R.NOME, 1, INSTR(R.NOME, ' ') - 1)SEPARADOR, I.QTSEPARARUN AS QTCORTE" +
                    "  FROM PCPEDI I, PCPEDC C, PCPRODUT P, PCCLIENT T, PCEMBALAGEM E, PCEMPR R WHERE I.CODFUNCSEP = R.MATRICULA AND E.CODAUXILIAR = I.CODAUXILIAR AND C.CODFILIAL = E.CODFILIAL AND T.CODCLI = I.CODCLI AND P.CODPROD = I.CODPROD AND I.NUMPED = C.NUMPED AND I.QTSEPARARUN > 0 AND C.DTFAT IS NULL " +
                    "  GROUP BY C.NUMCAR, C.NUMPED, T.CLIENTE, P.CODPROD, P.DESCRICAO, E.UNIDADE, E.EMBALAGEM, R.NOME, I.QTSEPARARUN ", cnn);
                cmd.BindByName = true;
                OracleDataReader rdr = cmd.ExecuteReader();

                PainelCorte painelCorte = null;

                while (rdr.Read())
                {
                    painelCorte = new PainelCorte();

                    painelCorte.Carga = Convert.ToInt64(rdr["NUMCAR"]);
                    painelCorte.Pedido = Convert.ToInt64(rdr["NUMPED"]);
                    painelCorte.Cliente = rdr["CLIENTE"].ToString();
                    painelCorte.Codigo = Convert.ToInt32(rdr["CODPROD"]);
                    painelCorte.Descricao = rdr["DESCRICAO"].ToString();
                    painelCorte.Und = rdr["UNIDADE"].ToString();
                    painelCorte.Emb = rdr["EMBALAGEM"].ToString();
                    painelCorte.Separador = rdr["SEPARADOR"].ToString();
                    painelCorte.QtCorte = Convert.ToDecimal(rdr["QTCORTE"]);                    
                    list.Add(painelCorte);
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
            return list;

        }

        //Painel financeiro credito cliente
        [WebMethod]
        public List<Credito> ListaClientesDeClasseBloqueada()
        {
            OracleConnection cnn = new OracleConnection("DATA SOURCE=192.168.132.20:1521/WINT;PERSIST SECURITY INFO=True;USER ID=AUTOMACAO; Password=AUT0M4C302020;");
            List<Credito> list = new List<Credito>();
            try
            {
                cnn.Open();
                OracleCommand cmd = new OracleCommand("SELECT C.CODFILIAL, NVL(C.NUMCAR,0)NUMCAR, C.NUMPED,C.POSICAO,C.CODCOB,I.CLASSEVENDA,I.CODCLI,I.CLIENTE,I.BLOQUEIO,I.BLOQUEIODEFINITIVO  " +
                    " FROM PCPEDC C, PCCLIENT I WHERE C.CODCLI = I.CODCLI AND DATA >= TRUNC(SYSDATE) -5 AND I.CLASSEVENDA IN ('B', 'C') AND C.POSICAO NOT IN ('C','F') ORDER BY CLASSEVENDA ", cnn);
                cmd.BindByName = true;
                OracleDataReader rdr = cmd.ExecuteReader();

                Credito credito = null;

                while (rdr.Read())
                {
                    credito = new Credito();

                    credito.Filial = Convert.ToInt32(rdr["CODFILIAL"]);
                    credito.Carga = Convert.ToInt64(rdr["NUMCAR"]);
                    credito.Pedido = Convert.ToInt64(rdr["NUMPED"]);
                    credito.Posicao = rdr["POSICAO"].ToString();
                    credito.Cobranca = rdr["CODCOB"].ToString();
                    credito.Classe = rdr["CLASSEVENDA"].ToString();
                    credito.Codigo = Convert.ToInt32(rdr["CODCLI"]);
                    credito.Cliente = rdr["CLIENTE"].ToString();
                    credito.Bloq = rdr["BLOQUEIO"].ToString();
                    credito.BloqDef = rdr["BLOQUEIODEFINITIVO"].ToString();
                    list.Add(credito);
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
            return list;

        }
    }
}
