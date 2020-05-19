<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppSeparacao.aspx.cs" Inherits="AppDCASeparacao.AppSeparacao" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <!-- Required meta tags -->
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" />
    <link rel="stylesheet" href="estilo.css" />
    <title>Separacao</title>
</head>
<body>
    <nav>
        <div class="nav nav-tabs" id="nav-tab" role="tablist">
            <a class="nav-item nav-link active" id="nav-home-tab" data-toggle="tab" href="#nav-home" role="tab" aria-controls="nav-home" aria-selected="true">Conferência</a>
            <a class="nav-item nav-link" id="nav-conferidos-tab" data-toggle="tab" href="#nav-conferidos" role="tab" aria-controls="nav-conferidos" aria-selected="false">Conferidos </a>
            <a class="nav-item nav-link" id="nav-restantes-tab" data-toggle="tab" href="#nav-restantes" role="tab" aria-controls="nav-restantes" aria-selected="false">A Conferir </a>
        </div>
    </nav>

    <form id="form1" runat="server">
        <div class="tab-content" id="nav-tabContent">
            <div class="tab-pane fade show active" id="nav-home" role="tabpanel" aria-labelledby="nav-home-tab">

                <div class="pb-2 mt-4 mb-2 border-bottom container">
                    <div class="row">
                        <div class="col-10">
                            <asp:Label ID="LbMatricula" class="container" runat="server" />
                            <asp:Label ID="LbNome" class="container" runat="server" />                            
                        </div>
                        <asp:Button ID="btSair" class="btn btn-sm btn-danger" runat="server" Text="Sair" OnClick="btSair_Click" />
                    </div>
                </div>

                <div class="form-group row container">
                    <asp:Label class="container" runat="server">Pedido</asp:Label>
                    <div class="col-6 col-sm-6 col-md-8 col-lg-8 col-xl-10">
                        <asp:TextBox ID="TextBoxPedido" class="form-control" runat="server"></asp:TextBox>
                    </div>
                    <asp:Button ID="btPesquisar" class="btn btn-outline-dark" runat="server" Text="Pesquisar" OnClick="btPesquisar_Click" />
                </div>
                
                <div class="form-group row container">
                    <div class="col-4 col-sm-4 col-md-6 col-lg-4 col-xl-4">
                        <asp:Label ID="lbCodigo" class="container" runat="server" Font-Size="9">Código</asp:Label>
                        <asp:TextBox ID="TextBoxCodigo" class="form-control" runat="server" Font-Size="9"></asp:TextBox>
                    </div>
                    <div class="col-5 col-sm-5 col-md-5 col-lg-5 col-xl-5">
                        <asp:Label ID="lbEmbalagem" class="container" runat="server" Font-Size="9">Embalagem</asp:Label>
                        <asp:TextBox ID="TextBoxEmbalagem" ReadOnly="True" class="form-control" Font-Size="9" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-3 col-sm-3 col-md-3 col-lg-3 col-xl-3">
                        <br />
                        <asp:Button ID="btProduto" class="btn btn-outline-primary btn-sm" runat="server" Text="Pesquisar" OnClick="btProduto_Click" />
                    </div>
                </div>

                <div class="form-group row container">
                    <asp:Label ID="lbDescricao" class="container" runat="server">Descrição</asp:Label>
                    <div class="col-12 col-sm-12 col-md-12 col-lg-12 col-xl-12">
                        <asp:TextBox ID="TextBoxDescricao" ReadOnly="True" class="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group row container">
                    <div class="col-4 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                        <asp:Label ID="lbQtPedida" class="container" runat="server">Qt.Pedida</asp:Label>
                        <asp:TextBox ID="TextBoxQtPedida" ReadOnly="True" class="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-4 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                        <asp:Label ID="lbQtConf" class="container" runat="server">Qt.Conferida</asp:Label>
                        <asp:TextBox ID="TextBoxConferida" class="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-4 col-sm-4 col-md-4 col-lg-4 col-xl-4">
                        <asp:Label ID="lbCorte" class="container" runat="server">Qt.Corte</asp:Label>
                        <asp:TextBox ID="TextBoxCorte" class="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group row container">
                    <div class="col-1">
                    </div>
                    <div class="col-11">
                        <asp:Button ID="btConfirmar" class="btn btn-outline-success btn-block" runat="server" Text="Confirmar" OnClick="btConfirmar_Click" />
                    </div>
                </div>
                <div class="form-group row container">
                    <div class="col-1">
                    </div>
                    <div class="col-11">
                        <asp:Button ID="btFinalizarConf" align="center" class="btn btn-outline-danger btn-block" runat="server" Text="Finalizar Conferência" OnClick="btFinalizarConf_Click1"  />                   
                    </div>
                </div> 
                <div class="pb-2 mt-4 mb-2 border-bottom container">
                    <h6>Detalhes</h6>
                    <asp:GridView ID="GridView4" runat="server" Width="100%" CellPadding="4" ForeColor="Black" Font-Size="8" CssClass="table table-bordered" GridLines="None" AllowPaging="False">           
                    </asp:GridView>
                </div>
              </div>
            <!-- Fim da 1ª Aba de conferencia-->
            <!-- Inicio da Aba Produtos Conferidos-->
            <div class="tab-pane fade" id="nav-conferidos" role="tabpanel" aria-labelledby="nav-profile-tab">
                <div class="pb-2 mt-4 mb-2 border-bottom container">
                    <h4>Produtos Conferidos</h4>                    
                   <asp:GridView ID="GridView2" runat="server" Width="100%" CellPadding="4" ForeColor="White" Font-Size="8" CssClass="table table-bordered table-dark" GridLines="None" AllowPaging="False">           
                   </asp:GridView>                 
                </div>
            </div>
            <!-- Fim da 2ª Aba de conferencia-->
            <!-- Inicio da Aba Produtos a Conferir-->
            <div class="tab-pane fade" id="nav-restantes" role="tabpanel" aria-labelledby="nav-profile-tab">
                <div class="pb-2 mt-4 mb-2 border-bottom container">
                    <h4>Produtos a Conferir</h4>                    
			        <asp:GridView ID="GridView1" runat="server" Width="100%" CellPadding="4" ForeColor="White" Font-Size="8" CssClass="table table-bordered table-dark" GridLines="None" AllowPaging="False">           
                    </asp:GridView>                                   
                </div>
                <br />
                <div class="pb-2 mt-4 mb-2 border-bottom container">
                    <h6>Detalhes</h6>
                    <asp:GridView ID="GridView3" runat="server" Width="100%" CellPadding="4" ForeColor="Black" Font-Size="8" CssClass="table table-bordered" GridLines="None" AllowPaging="False">           
                    </asp:GridView>
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
