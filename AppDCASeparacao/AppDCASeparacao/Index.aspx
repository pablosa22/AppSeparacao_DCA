<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="AppDCASeparacao.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" />
    <link rel="stylesheet" href="estilo.css" />
    <link href="signin.css" rel="stylesheet" />
    <script type="text/javascript" DEFER="DEFER">
        // INICIO FUNÇÃO DE MASCARA MAIUSCULA
        function maiuscula(z){
                v = z.value.toUpperCase();
                z.value = v;
        }
        //FIM DA FUNÇÃO MASCARA MAIUSCULA            
    </script>
    <style>
        body {         
            
            background-image: url("/Img/fundo2.jpg");
        }
    </style>
    <title>Acesso Separação</title>
</head>
<body class="text-center" >
    <form id="form1" runat="server">
        
         <br />
        <br />
        <h4>Expedição (Separação)</h4>        
        <asp:Image ID="Image1" runat="server" ImageUrl="~/Img/Icon.png"/> 
        <br />
        <br />
        <div class="container">           
                <asp:TextBox ID="TextBoxMatricula" placeholder="Matricula" class="form-control" runat="server"></asp:TextBox>
            <br />
                <asp:TextBox ID="TextBoxUsuario" placeholder="Usuário" class="form-control"  runat="server" onkeyup="maiuscula(this)"></asp:TextBox>
            <br />                   
                    <asp:DropDownList ID="DropDownList2" class="form-control" runat="server">
                         <asp:ListItem Value="0">Filial</asp:ListItem>
                         <asp:ListItem Value="1">1</asp:ListItem>
                         <asp:ListItem Value="2">2</asp:ListItem>
                         <asp:ListItem Value="3">3</asp:ListItem>                     
                         <asp:ListItem Value="4">4</asp:ListItem>                     
                         <asp:ListItem Value="5">5</asp:ListItem>                                              
                         <asp:ListItem Value="72">72</asp:ListItem>
                         <asp:ListItem Value="73">73</asp:ListItem>
                         <asp:ListItem Value="74">74</asp:ListItem>
                         <asp:ListItem Value="77">77</asp:ListItem>
                     </asp:DropDownList>
            <br />
                <asp:Button ID="btEntrar" class="btn btn-outline-success btn-block" runat="server"  Text="Entrar" OnClick="btEntrar_Click"   />                    
            <br />                 
            <p class="mt-5 mb-3 text-muted">&copy; Automação Versão 2.0</p>             
        </div>   
    </form>
      <!-- Optional JavaScript -->
    <!-- jQuery first, then Popper.js, then Bootstrap JS -->
    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
</body>
</html>
