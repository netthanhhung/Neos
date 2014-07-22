<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StatisticsGeneral.aspx.cs" Inherits="StatisticsGeneral" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%--<%@ Register TagPrefix="telerik" Namespace="Telerik.Charting" Assembly="Telerik.Charting" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>General statistics page</title>     

    <script src="script/utils.js" type="text/javascript"></script>
    <link href="Styles/Neos.css" rel="Stylesheet" />
    <script type="text/javascript">
        function centerlizeControl()
        {
            getSize();
            var mapWidth;
            var mapHeight;    
            if(isIE)
            {
                mapWidth = browserWidth - 0;    
                mapHeight = browserHeight - 0;
            }
            else
            {
                mapWidth = browserWidth - 0;    
                mapHeight = browserHeight - 0;
            }
            if(document.getElementById("statisticSummary") != null)
            {
                document.getElementById("statisticSummary").style.left = (mapWidth/2 - 366) + "px";
                document.getElementById("statisticSummary").style.top = (mapHeight/2 - 180) + "px";
            }
        }
    </script>
</head>
<body >
    <form id="generalStatisticsForm" runat="server">
     <div class="rightpane_title"><asp:Literal runat="server" ID="lblStatisticTitle" Text="Statistics"></asp:Literal></div>        
     <div style="margin-top:30px;">        
        <telerik:RadScriptManager ID="ScriptManager" runat="server"/>
        <div id="statisticSummary" style="">        
            <%--<table style="width:100%;" >                
                <tr>
                    <td align="right" style="width:50%">
                        <asp:Label ID="lblNbrActiveCandidateText" runat="server" ForeColor="seagreen"
                            Text="Number of active candidates :"></asp:Label>                        
                    </td>
                    <td align="left">
                        <asp:Label ID="lblNbrActiveCandidateValue" runat="server" ForeColor="seagreen" Text="0"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblTotalCandidateText" runat="server" ForeColor="seagreen" Text="Total number of candidates :"></asp:Label>                        
                    </td>
                    <td align="left">
                        <asp:Label ID="lblTotalCandidateValue" runat="server" ForeColor="seagreen" Text="0"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblNbrActCanOfCurUserText" runat="server" ForeColor="seagreen" Text="Number of active candidates related to current user :"></asp:Label>                        
                    </td>
                    <td align="left">
                        <asp:Label ID="lblNbrActCanOfCurUserValue" runat="server" ForeColor="seagreen" Text="0"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblTotalCanOfCurUserText" runat="server" ForeColor="seagreen" Text="Total number of candidates related to current user :"></asp:Label>                        
                    </td>
                    <td align="left">
                        <asp:Label ID="lblTotalCanOfCurUserValue" runat="server" ForeColor="seagreen" Text="0"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblNbrCompanyText" runat="server" ForeColor="seagreen" Text="Number of companies :"></asp:Label>                        
                    </td>
                    <td align="left">
                        <asp:Label ID="lblNbrCompanyValue" runat="server" ForeColor="seagreen" Text="0"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblNbrActiveCustomerText" runat="server" ForeColor="seagreen" Text="Number of active customers :"></asp:Label>                        
                    </td>
                    <td align="left">
                        <asp:Label ID="lblNbrActiveCustomerValue" runat="server" ForeColor="seagreen" Text="0"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblNbrProspectsText" runat="server" ForeColor="seagreen" Text="Number of prospects :"></asp:Label>                        
                    </td>
                    <td align="left">
                        <asp:Label ID="lblNbrProspectsValue" runat="server" ForeColor="seagreen" Text="0"></asp:Label>
                    </td>
                </tr>
            </table>   --%>     
            
            <telerik:RadChart ID="RadChartGeneralStatistics" runat="server" Skin="WEbBlue" Legend-Visible="false"  
                Height="600px" Width="800px" SeriesOrientation="Horizontal" AutoTextWrap="true" IntelligentLabelsEnabled="false" >
                <ChartTitle TextBlock-Text="General"></ChartTitle>
                 <%--<PlotArea>
                   <XAxis AutoScale="false">
                       <Appearance TextAppearance-AutoTextWrap="false">
                       </Appearance>
                       <Items>
                       
                           <telerik:ChartAxisItem TextBlock-Text="Cote de Blaye" />
                            <telerik:ChartAxisItem TextBlock-Text="Thuringer Rostbratwurst" />
                           <telerik:ChartAxisItem TextBlock-Text="Mishi Kobe Niku" />
                           <telerik:ChartAxisItem TextBlock-Text="Sir Rodney's Marmalade" />
                           <telerik:ChartAxisItem TextBlock-Text="Carnarvon Tigers" />
                           <telerik:ChartAxisItem TextBlock-Text="Raclette Courdavault" />
                           <telerik:ChartAxisItem TextBlock-Text="Manjimup Dried Apples" />
                           <telerik:ChartAxisItem TextBlock-Text="Tarte au sucre" />                           
                       </Items>
                   </XAxis>
               </PlotArea>--%>
                </telerik:RadChart>
            
        </div>
    </div>
    </form>
</body>
</html>