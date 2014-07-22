<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChooseCompanyAddressPopup.aspx.cs" Inherits="ChooseCompanyAddressPopup" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Choose company</title>
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
    <form id="chooseCompanyAddressPopup" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" /> 
    <div>
        <telerik:RadAjaxManager ID="RadAjaxManager" runat="server">
            <AjaxSettings>                
                <telerik:AjaxSetting AjaxControlID="gridCompanyAddress">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridCompanyAddress"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>         
         <table cellpadding="0" cellspacing="0" width="100%" style="margin-top: 0px;">
            <tr>
                <td>
                    <div>                                                                          
                        <table style="width:100%">
                            <tr>
                                <td>
                                    <telerik:RadGrid ID="gridCompanyAddress" GridLines="None" Skin="Office2007" AllowMultiRowSelection="false"
                                        runat="server" AllowPaging="True" AllowSorting="True" 
                                        PageSize="10" Width="100%"  AutoGenerateColumns="false" EnableAjaxSkinRendering="true">
                                        <PagerStyle Mode="NextPrevAndNumeric">
                                        </PagerStyle>
                                        <MasterTableView DataKeyNames="AddressID" DataMember="CompanyAddress" AllowMultiColumnSorting="True"
                                            Width="100%" EditMode="PopUp">                                           
                                            <Columns>
                                                <telerik:GridBoundColumn UniqueName="Name" SortExpression="Name" 
                                                    HeaderText="Name" DataField="Name">                                                    
                                                    <HeaderStyle></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="Address" SortExpression="Address" 
                                                    HeaderText="Address" DataField="Address">
                                                    <HeaderStyle></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="ZipCode" SortExpression="ZipCode" 
                                                    HeaderText="ZipCode" DataField="ZipCode">                                                    
                                                    <HeaderStyle></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="City" HeaderText="City" 
                                                    SortExpression="City" DataField="City">                                                    
                                                    <HeaderStyle></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="VatNumber" HeaderText="VatNumber" 
                                                    SortExpression="VatNumber" DataField="VatNumber">
                                                    <HeaderStyle></HeaderStyle>                                                    
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="Telephone" HeaderText="Telephone"
                                                    SortExpression="Telephone" DataField="Telephone">                                                    
                                                    <HeaderStyle></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="Fax" HeaderText="Fax"
                                                    SortExpression="Fax" DataField="Fax">                                                    
                                                    <HeaderStyle></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="Email" HeaderText="Email"
                                                    SortExpression="Email" DataField="Email">                                                    
                                                    <HeaderStyle></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="IsDefault" HeaderText="Default"
                                                    SortExpression="IsDefault" DataField="IsDefault">                                                    
                                                    <HeaderStyle></HeaderStyle>
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
