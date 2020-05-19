<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuApp.aspx.cs" Inherits="AppDCASeparacao.MenuApp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <!-- Required meta tags -->
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" />
    <link rel="stylesheet" href="estilo.css" />
    <title>Menu App</title>
    <script>      

        function DivClickedRecebimento() {
            var btGrafico = $('#<%=btRecebimento.ClientID%>');
            if (btRecebimento != null) {
                btRecebimento.click();
            }
        }
        function DivClickedSeparacao() {
            var btSeparacao = $('#<%=btSeparacao.ClientID%>');
            if (btSeparacao != null) {
                btSeparacao.click();
            }
        }
    </script>

     <style>
        body {         
            
            background-image: url("/Img/fundo3.png");
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <br />
        <div class="col-lg-11 pb-3 mt-3 mb-3 border-bottom container text-center">
            <h4>Menu Opções</h4>
        </div>
        <br />
        
        <div class="row">
            <div class="col-lg-12 text-center">
                <asp:Button runat="server" ID="btRecebimento" Style="display: none" OnClick="btRecebimento_Onclik" />
                <div onclick="javascript:DivClickedRecebimento(); return true;">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/img/Recebimento.png" Width="30%" />
                    <h4>Recebimento</h4>
                </div>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-lg-12 text-center">
                <asp:Button runat="server" ID="btSeparacao" Style="display: none" OnClick="btSeparacao_Onclik" />
                <div onclick="javascript:DivClickedSeparacao(); return true;">
                    <asp:Image ID="Image6" runat="server" ImageUrl="~/img/Separacao.png" Width="30%" />
                    <h4>Separação</h4>
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
