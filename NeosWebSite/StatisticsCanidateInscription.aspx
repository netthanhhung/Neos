<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StatisticsCanidateInscription.aspx.cs" Inherits="StatisticsCanidateInscription" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%--<%@ Register TagPrefix="telerik" Namespace="Telerik.Charting" Assembly="Telerik.Charting" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Candidates inscriptions statistics Page</title>     

    <script src="script/utils.js" type="text/javascript"></script>
    <link href="Styles/Neos.css" rel="Stylesheet" />
</head>
<body>
    <form id="InscriptionStatisticsForm" runat="server">
    <div>         
        <telerik:RadScriptManager ID="ScriptManager" runat="server"/>          
        <div class="rightpane_title">
                <asp:Literal runat="server" ID="lblTitle" Text="Candidate Inscriptions"></asp:Literal></div>
        <div style="margin-top: 30px;">
            <table width="100%">  
                <tr>
                    <td colspan="2" align="center" style="font-size:x-large; font-style:inherit; color:Purple">
                        <asp:Label ID="lblCanIncriptionSta" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:500px" align="left">
                        <telerik:RadChart ID="radChartInscriptionYear" runat="server" 
                            Width="450px" Height="340px" Skin="LightBrown">                                                        
                        </telerik:RadChart>
                    </td>
                    <td style="width:500px" align="right">
                        <telerik:RadChart ID="radChartInscriptionMonth" runat="server" 
                            Width="450px" Height="340px" Skin="LightBrown">                                                        
                        </telerik:RadChart>
                    </td>
                </tr>
                 <tr>
                    <td align="left">
                        <telerik:RadChart ID="radChartInscriptionWeek" runat="server" 
                            Width="450px" Height="340px" Skin="LightBrown">                                                        
                        </telerik:RadChart>
                    </td>
                    <td align="right">
                        <telerik:RadChart ID="radChartInscriptionDay" runat="server" 
                            Width="450px" Height="340px" Skin="LightBrown">                                                        
                        </telerik:RadChart>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
