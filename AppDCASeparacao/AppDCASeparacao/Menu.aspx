<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="AppDCASeparacao.Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" />
    <link rel="stylesheet" href="estilo.css" />
    <link href="signin.css" rel="stylesheet" />
    <title>Menu Opções</title>
    <script>
        function DivClickedEnviaPainel() {
            var btEnviaPainel = $('#<%=btEnviaPainel.ClientID%>');
            if (btEnviaPainel != null) {
                btEnviaPainel.click();
            }
        }
        
        function DivClickedPainelSeparacao() {
            var btPainelSeparacao = $('#<%=btPainelSeparacao.ClientID%>');
            if (btPainelSeparacao != null) {
                btPainelSeparacao.click();
            }
        }
        function DivClickedPainelCorte() {
            var btPainelCorte = $('#<%=btPainelCorte.ClientID%>');
            if (btPainelCorte != null) {
                btPainelCorte.click();
            }
        }
        function DivClickedGrafico() {
            var btGrafico = $('#<%=btGrafico.ClientID%>');
            if (btGrafico != null) {
                btGrafico.click();
            }
        }

        function DivClickedAuditoria() {
            var btAuditoria = $('#<%=btAuditoria.ClientID%>');
            if (btAuditoria != null) {
                btAuditoria.click();
            }
        }

        function DivClickedPainelFaturamento() {
            var btPainelFaturamento = $('#<%=btPainelFaturamento.ClientID%>');
            if (btPainelFaturamento != null) {
                btPainelFaturamento.click();
            }
        }

        

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-1">
        </div>
        <br />
        <div class="col-lg-11 pb-3 mt-3 mb-3 border-bottom container text-center">
            <h4>DCA Distribuidora</h4>
        </div>
        <br />

        <div class="row">
            <div class="col-lg-4 text-center">
                <asp:Button runat="server" ID="btEnviaPainel" Style="display: none" OnClick="btEnviaPainel_Onclik" />
                <div onclick="javascript:DivClickedEnviaPainel(); return true;">
                    <asp:Image ID="Image6" runat="server" ImageUrl="~/img/LancarParaConferencia.png" Width="20%" />
                    <h4>Iniciar Separação</h4>
                </div>
            </div>
            <div class="col-lg-4 text-center">
                <asp:Button runat="server" ID="btPainelSeparacao" Style="display: none" OnClick="btbtPainelSeparacao_Onclik" />
                <div onclick="javascript:DivClickedPainelSeparacao(); return true;">
                    <asp:Image ID="Image3" runat="server" ImageUrl="~/img/Conferencia.png" Width="20%" />
                    <h4>Painel Separação</h4>
                </div>
            </div>
            <!-- -->

            <div class="col-lg-4 text-center">
                <asp:Button runat="server" ID="btPainelCorte" Style="display: none" OnClick="btbtPainelCorte_Onclik" />
                <div onclick="javascript:DivClickedPainelCorte(); return true;">
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/img/Faturamento.png" Width="20%" />
                    <h4>Painel de Cortes</h4>
                </div>
            </div>

        </div>
        <br />
        <br />
        <br />
        <br />

        <div class="row">
            <div class="col-lg-4 text-center">
                <asp:Button runat="server" ID="btPainelFaturamento" Style="display: none" OnClick="btPainelFaturamento_Onclik" />
                <div onclick="javascript:DivClickedPainelFaturamento(); return true;">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/img/Almoxarifado.png" Width="20%" />
                    <h4>Painel Faturamento</h4>
                </div>
            </div>
            <div class="col-lg-4 text-center">
                <asp:Button runat="server" ID="btGrafico" Style="display: none" OnClick="btGrafico_Onclik" />
                <div onclick="javascript:DivClickedGrafico(); return true;">
                    <asp:Image ID="Image5" runat="server" ImageUrl="~/img/Graficos.png" Width="20%" />
                    <h4>Gráfico Separação</h4>
                </div>
            </div>
            <div class="col-lg-4 text-center">
                <asp:Button runat="server" ID="btAuditoria" Style="display: none" OnClick="btAuditoria_Onclik" />
                <div onclick="javascript:DivClickedAuditoria(); return true;">
                    <asp:Image ID="Image4" runat="server" ImageUrl="~/img/Painel_corte.png" Width="20%" />
                    <h4>Auditoria Clientes</h4>
                </div>
            </div>

        </div>
    </form>
    <!-- Optional JavaScript -->
    <!-- jQuery first, then Popper.js, then Bootstrap JS -->
    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
</body>
</html>
