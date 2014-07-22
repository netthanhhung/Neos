<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StatisticsCandidateLocation.aspx.cs" Inherits="StatisticsCandidateLocation" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%--<%@ Register TagPrefix="telerik" Namespace="Telerik.Charting" Assembly="Telerik.Charting" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Candiate location statistics Page</title>     

    <script src="script/utils.js" type="text/javascript"></script>
    <link href="Styles/Neos.css" rel="Stylesheet" />
</head>
<body>
    <form id="candidateLocationStatisticsForm" runat="server">
    <div>         
        <telerik:RadScriptManager ID="ScriptManager" runat="server"/>          
        <div class="rightpane_title">
                <asp:Literal runat="server" ID="lblTitle" Text="Candidate Locations"></asp:Literal></div>
        <div style="margin-top: 30px;">
            <table>                
                <tr>
                    <td>
                        <telerik:RadChart ID="radChartCandidateLocation" runat="server" 
                            Width="900px" Height="600px" Skin="LightBrown">                                                        
                        </telerik:RadChart>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
