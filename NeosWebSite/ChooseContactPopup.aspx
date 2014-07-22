<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChooseContactPopup.aspx.cs" Inherits="ChooseContactPopup" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Choose contact</title>
    <link href="Styles/Neos.css" rel="stylesheet" type="text/css" />
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
        
        function OnBtnCancelClientClicked(sender, eventArgs)         
        {         
            var currentWindow = GetRadWindow();
            currentWindow.close();
        }
        
        function OnBtnOkClientClicked(argument)         
        {
            var currentWindow = GetRadWindow();
            currentWindow.argument = argument;
            currentWindow.close();
        }
    </script>
</head>
<body>
    <form id="chooseContactPopup" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" /> 
    <div>
        <asp:Panel runat="server" ID="pnlSearchContacts" DefaultButton="btnSearchContacts">

         <table cellpadding="0" cellspacing="0" width="80%" style="margin-top: 0px;">
            <tr>
                <td>
                    <asp:Label ID="lblConLastName" runat="server" Text="Last Name"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtConLastName" runat="server" Width="145"></telerik:RadTextBox>
                </td>
                <td>
                    <asp:Label ID="lblConFirstName" runat="server" Text="First Name"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtConFirstName" runat="server" Width="145"></telerik:RadTextBox>
                </td>
                <td>
                    <asp:Button ID="btnSearchContacts" runat="server" Text="Search" CssClass="flatButton" Width="50px"
                        OnClick="OnContactSearchClicked"></asp:Button>               
                </td>
            </tr>
         </table>
         </asp:Panel>
         <table cellpadding="0" cellspacing="0" width="100%" style="margin-top: 0px;">
            <tr>
                <td>
                    <div>
                         <telerik:RadAjaxManager ID="RadAjaxManager" runat="server">
                            <AjaxSettings>
                                <telerik:AjaxSetting AjaxControlID="gridContact">
                                    <UpdatedControls>
                                        <telerik:AjaxUpdatedControl ControlID="gridContact" LoadingPanelID="RadAjaxLoadingPanel"/>
                                    </UpdatedControls>
                                </telerik:AjaxSetting>
                            </AjaxSettings>
                        </telerik:RadAjaxManager>
                        
                         <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server" Height="75px" Width="75px" Transparency="50">
                            <img alt="Loading..." src='<%= RadAjaxLoadingPanel.GetWebResourceUrl(Page, "Telerik.Web.UI.Skins.Default.Ajax.loading.gif") %>' style="border:0;" />
                        </telerik:RadAjaxLoadingPanel>
                        <table style="width:100%">
                            <tr>
                                <td>
                                    <telerik:RadGrid ID="gridContact" GridLines="None" Skin="Office2007" AllowMultiRowSelection="false"
                                        runat="server" AllowPaging="True" AllowSorting="True"
                                        PageSize="25" Width="100%"  AutoGenerateColumns="false">
                                        <PagerStyle Mode="NextPrevAndNumeric">
                                        </PagerStyle>
                                        <MasterTableView DataKeyNames="ContactId" DataMember="Candidate" AllowMultiColumnSorting="True"
                                            Width="100%" EditMode="PopUp">                    
                                            <Columns>   
                                                <telerik:GridBoundColumn UniqueName="LastName" SortExpression="LastName" HeaderText="Last Name" DataField="LastName">
                                                    <HeaderStyle Width="30%"></HeaderStyle>
                                                </telerik:GridBoundColumn> 
                                                <telerik:GridBoundColumn UniqueName="FirstName" SortExpression="FirstName" HeaderText="First Name" DataField="FirstName">
                                                    <HeaderStyle Width="30%"></HeaderStyle>
                                                </telerik:GridBoundColumn>                                                 
                                                <telerik:GridBoundColumn UniqueName="Function" SortExpression="Position" HeaderText="Function" DataField="Position">
                                                    <HeaderStyle Width="40%"></HeaderStyle>
                                                </telerik:GridBoundColumn>                                                                       
                                            </Columns>

                                        </MasterTableView>
                                        <ClientSettings>
                                             <Selecting AllowRowSelect="true" />
                                        </ClientSettings>               
                                    </telerik:RadGrid>
                                </td>
                            </tr>                            
                        </table>                        
                    </div>
                </td>
            </tr>
        </table>
        <div style="text-align:center">
            <asp:Button runat="server" ID="btnOK" Text="OK" CssClass="flatButton" Width="60" OnClick="OnBtnOkClicked"/>&nbsp;
            <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="flatButton" Width="60" OnClientClick="OnBtnCancelClientClicked()"/>
        </div>
    </div>
    </form>
</body>
</html>
