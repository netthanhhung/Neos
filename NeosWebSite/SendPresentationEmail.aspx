<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SendPresentationEmail.aspx.cs" Inherits="SendPresentationEmail" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Send presentation email</title>
    <link href="Styles/Neos.css" rel="stylesheet" type="text/css" />
    <telerik:RadScriptBlock runat="server" ID="script">
        <script type="text/javascript" language="javascript">
        function GetRadWindow()
        {
          var oWindow = null;
          if (window.radWindow)
             oWindow = window.radWindow;
          else if (window.frameElement.radWindow)
             oWindow = window.frameElement.radWindow;
          return oWindow;
        }        
        function closeWindow()         
        {
            var currentWindow = GetRadWindow();
            currentWindow.close();
        }
    </script>
    </telerik:RadScriptBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="ScriptManager" runat="server">
        </telerik:RadScriptManager>
        <div>
        <%--<asp:RegularExpressionValidator runat="server" ID="test"></asp:RegularExpressionValidator>--%>
            <table width="100%">
                <tr>
                    <td></td>
                    <td></td>
                    <td style="text-align:left">
                        <asp:CheckBox ID="chkAutoCreateAction" runat="server" Text="Automatically create action" />                        
                    </td>                    
                </tr>
                <tr>
                    <td style="width:7%; text-align:right" rowspan="3">
                        <asp:Button runat="server" ID="btnSendMail" OnClick="onButtonSendMail_Click" Text="Send" CssClass="sendemail" Width="70" Height="55"></asp:Button>
                    </td>
                    <td style="width:10%; text-align:right">
                        <asp:Image runat="server" ID="imgTo" ImageUrl="~/images/24x24/book_open.png" ImageAlign="AbsMiddle" />
                        <asp:Literal runat="server" ID="lblTo" Text="To"></asp:Literal>
                    </td>
                    <td style=""><asp:TextBox runat="server" ID="txtTo" Width="99%"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width:10%;text-align:right">
                        <asp:Image runat="server" ID="Image1" ImageUrl="~/images/24x24/book_open.png"  ImageAlign="AbsMiddle"/>
                        <asp:Literal runat="server" ID="lblCC" Text="Cc"></asp:Literal>
                    </td>
                    <td style=""><asp:TextBox runat="server" ID="txtCC" Width="99%"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="text-align:right">
                        <asp:Literal runat="server" ID="lblSubject" Text="Subject"></asp:Literal>
                    </td>
                    <td style=""><asp:TextBox runat="server" ID="txtSubject" Width="99%"></asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td style="text-align:right">
                        <asp:Literal runat="server" ID="lblAttach" Text="Attach..."></asp:Literal>
                    </td>
                    <td style="">
                        <asp:Repeater runat="server" ID="rptAttachFiles" OnItemDataBound="OnRepeaterAttachFiles_ItemDataBound">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chkSelectFile" ForeColor="#0000EE" Font-Italic="true" Checked="true"  />
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <telerik:RadEditor runat="server" ID="txtEmailContent"  Width="99%" Height="300" Skin="Telerik"
                            ToolsFile="~/Config/RadEditor.xml">
                        </telerik:RadEditor>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
