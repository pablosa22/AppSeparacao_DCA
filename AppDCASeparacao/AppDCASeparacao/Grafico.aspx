<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Grafico.aspx.cs" Inherits="AppDCASeparacao.Grafico" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
     <!-- Required meta tags -->
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <meta http-equiv="refresh" content="120" />
    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" />
    <link rel="stylesheet" href="estilo.css" />
    <title>Dashboard</title>
     <script src="https://www.gstatic.com/charts/loader.js"></script>
    <script type="text/javascript">
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(drawChart);
        google.charts.setOnLoadCallback(drawChartItens);
        google.charts.setOnLoadCallback(drawChartOntem);
        google.charts.setOnLoadCallback(drawChartItensOntem);

        function drawChart() {

            var data = google.visualization.arrayToDataTable(<%=this.ObterDados()%>);

            var options = {

                "title": "Por Pedido (Hoje)",
                "backgroundColor": false,
                "is3D": true, "pieHole": 0.4,
                "fontSize": 15,
                "pieSliceTextStyle": { "color": "#FFF" },
                "sliceVisibilityThrshold": false,
                "legend": {
                    "position": "rigth",
                    "textStyle": {
                        "color": "#000000",
                        "fontSize": 12
                    }
                },
                "tooltip": {
                    "textStyle": { "color": "#000000" },
                    "showColorCode": false
                },
                "colors": ["#1f386b", "#DC3912", "#FF9900", "#109618", "#990099"]
            };

            var chart = new google.visualization.LineChart(document.getElementById('piechart'));

            chart.draw(data, options);

        }


        function drawChartItens() {

            var data = google.visualization.arrayToDataTable(<%=this.ObterDadosPorItens()%>);

            var options = {

                "title": "Por Itens Conferidos (Hoje)",
                "backgroundColor": false,
                "is3D": true, "pieHole": 0.4,
                "fontSize": 15,
                "pieSliceTextStyle": { "color": "#FFF" },
                "sliceVisibilityThrshold": false,
                "legend": {
                    "position": "rigth",
                    "textStyle": {
                        "color": "#000000",
                        "fontSize": 12
                    }
                },
                "tooltip": {
                    "textStyle": { "color": "#000000" },
                    "showColorCode": false
                },
                "colors": ["#1f386b", "#DC3912", "#FF9900", "#109618", "#990099"]
            };

            var chart1 = new google.visualization.PieChart(document.getElementById('piechartItens'));

            chart1.draw(data, options);

        }



    </script>


</head>
<body>
    <form id="form1" runat="server">
        <div class="pb-2 mt-4 mb-2 border-bottom container">
            <h4>Ranking de Separação DCA Distribuição</h4>
        </div>
        
        <div class="row">
            <div class="col-5 col-sm-5 col-md-5 col-lg-6 col-xl-6">
                <div id="piechart" style="width:680px; height:300px;">           
                </div>
            </div>            
            <div class="col-6 col-sm-6 col-md-6 col-lg-6 col-xl-6">
                <div id="piechartItens" style="width:680px; height:300px;">           
                </div>
            </div>
        </div>

        <!-- Rank do dia atual-->         
        <div class="pb-2 mt-4 mb-2 border-bottom container">
            <h6>KPI de Separação hoje</h6>
        </div>

         <div class="row">
            <div class="col-1 col-sm-1 col-md-1 col-lg-1 col-xl-1">                
            </div>           
            <div class="col-2 col-sm-2 col-md-2 col-lg-2 col-xl-2">                
                <asp:Label id="LbTotalPedidos" runat="server" Font-Bold="True" Font-Size="10pt">Qtde.Pedido</asp:Label>
            </div>
            <div class="col-2 col-sm-2 col-md-2 col-lg-2 col-xl-2">                
                <asp:Label id="Label2" runat="server" Font-Bold="True" Font-Size="10pt">Peso</asp:Label>            
            </div>  
            <div class="col-2 col-sm-2 col-md-2 col-lg-2 col-xl-2">                
                <asp:Label id="Label4" runat="server" Font-Bold="True" Font-Size="10pt">Qtde.Itens</asp:Label>            
            </div>  
            
            <div class="col-2 col-sm-2 col-md-2 col-lg-2 col-xl-2">                
                <asp:Label id="Label6" runat="server" Font-Bold="True" Font-Size="10pt">Vl.Separado</asp:Label>            
            </div> 

             <div class="col-2 col-sm-2 col-md-2 col-lg-2 col-xl-2">                
                <asp:Label id="Label8" runat="server" Font-Bold="True" Font-Size="10pt">Qtde.Separador</asp:Label>            
            </div> 
        </div>
        

        <div class="row">
            <div class="col-1 col-sm-1 col-md-1 col-lg-1 col-xl-1">                
            </div>            
            <div class="col-2 col-sm-2 col-md-2 col-lg-2 col-xl-2" >                
                <div class="p-3 mb-2 bg-success text-white">
                    <asp:Label id="lbQtdePedido" runat="server" Font-Bold="True" Font-Size="20pt">0</asp:Label>
                </div>     
            </div>

            <div class="col-2 col-sm-2 col-md-2 col-lg-2 col-xl-2">                
                <div class="p-3 mb-2 bg-success text-white ">
                    <asp:Label id="lbPesoAtual" runat="server" Font-Bold="True" Font-Size="20pt">0</asp:Label>
                </div>
            </div> 
            
            <div class="col-2 col-sm-2 col-md-2 col-lg-2 col-xl-2">                
                <div class="p-3 mb-2 bg-success text-white">
                    <asp:Label id="lbItensAtual" runat="server" Font-Bold="True" Font-Size="20pt">0</asp:Label>
                </div>
            </div>  

             <div class="col-2 col-sm-2 col-md-2 col-lg-2 col-xl-2">                
                <div class="p-3 mb-2 bg-success text-white">
                    <asp:Label id="lbValorHoje" runat="server" Font-Bold="True" Font-Size="20pt">0</asp:Label>
                </div>
            </div> 

            <div class="col-2 col-sm-2 col-md-2 col-lg-2 col-xl-2">                
                <div class="p-3 mb-2 bg-warning text-white">
                    <asp:Label id="lbQtdeConferente" runat="server" Font-Bold="True" Font-Size="20pt">0</asp:Label>
                </div>
            </div> 
        </div>

         <!-- Rank do dia atual-->         



        <!-- Rank do dia ontem-->
        <div class="pb-2 mt-4 mb-2 border-bottom container">
            <h6>KPI Separação Mesmo Dia Da Semana Anterior </h6>
        </div>


        <div class="row">
            <div class="col-1 col-sm-1 col-md-1 col-lg-1 col-xl-1">                
            </div>            
            <div class="col-2 col-sm-2 col-md-2 col-lg-2 col-xl-2" >                
                <div class="p-3 mb-2 bg-primary text-white">
                    <asp:Label id="lbQtdePedidosOntem" runat="server" Font-Bold="True" Font-Size="20pt">0</asp:Label>
                </div>     
            </div>

            <div class="col-2 col-sm-2 col-md-2 col-lg-2 col-xl-2">                
                <div class="p-3 mb-2 bg-primary text-white ">
                    <asp:Label id="lbPesoOntem" runat="server" Font-Bold="True" Font-Size="20pt">0</asp:Label>
                </div>
            </div> 
            
            <div class="col-2 col-sm-2 col-md-2 col-lg-2 col-xl-2">                
                <div class="p-3 mb-2 bg-primary text-white">
                    <asp:Label id="lbItensOntem" runat="server" Font-Bold="True" Font-Size="20pt">0</asp:Label>
                </div>
            </div>  

             <div class="col-2 col-sm-2 col-md-2 col-lg-2 col-xl-2">                
                <div class="p-3 mb-2 bg-primary text-white">
                    <asp:Label id="lbValorOntem" runat="server" Font-Bold="True" Font-Size="20pt">0</asp:Label>
                </div>
            </div> 

            <div class="col-2 col-sm-2 col-md-2 col-lg-2 col-xl-2">                
                <div class="p-3 mb-2 bg-warning text-white">
                    <asp:Label id="lbConferenteOnt" runat="server" Font-Bold="True" Font-Size="20pt">0</asp:Label>
                </div>
            </div> 
        </div>   
        
        
     <!-- Optional JavaScript -->
    <!-- jQuery first, then Popper.js, then Bootstrap JS -->
    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
    </form>
</body>
</html>
