<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChooseCompanyPopup.aspx.cs" Inherits="ChooseCompanyPopup" %>

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
    <form id="chooseCompanyPopup" runat="server">
    <telerik:RadScriptManager ID="ScriptManager" runat="server" /> 
    <div>
        <telerik:RadAjaxManager ID="RadAjaxManager" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="btnSearchCompanies">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridCompany"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gridCompany">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gridCompany"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <asp:Panel runat="server" ID="pnlSearchCompanies" DefaultButton="btnSearchCompanies">

         <table cellpadding="0" cellspacing="0" width="30%" style="margin-top: 0px;">
            <tr>
                <td>
                    <asp:Label ID="lblName" runat="server" Text="Name"></asp:Label>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtName" runat="server" Width="145"></telerik:RadTextBox>
                </td>                
                <td>
                    <asp:Button ID="btnSearchCompanies" runat="server" Text="Search" CssClass="flatButton" Width="60px"
                        OnClick="OnCompanySearchClicked"></asp:Button>               
                </td>
            </tr>
         </table>
         </asp:Panel>
         <table cellpadding="0" cellspacing="0" width="100%" style="margin-top: 0px;">
            <tr>
                <td>
                    <div>                                                                          
                        <table style="width:100%">
                            <tr>
                                <td>
                                    <telerik:RadGrid ID="gridCompany" GridLines="None" Skin="Office2007" AllowMultiRowSelection="false"
                                        runat="server" AllowPaging="True" AllowSorting="True" AllowCustomPaging="true"
                                        PageSize="10" Width="100%"  AutoGenerateColumns="false" EnableAjaxSkinRendering="true"
                                        OnPageIndexChanged="OnGridCompanyPageIndexChanged" 
                                        OnNeedDataSource="OnGridCompanyNeedDataSource"
                                        OnSortCommand="OnGridCompanySortCommand">
                                        <PagerStyle Mode="NextPrevAndNumeric">
                                        </PagerStyle>
                                        <MasterTableView DataKeyNames="CompanyID" DataMember="Company" AllowMultiColumnSorting="True"
                                            Width="100%" EditMode="PopUp">                                           
                                            <Columns>
                                                <telerik:GridBoundColumn UniqueName="CompanyName" SortExpression="CompanyName" 
                                                    HeaderText="Name" DataField="CompanyName">                                                    
                                                    <HeaderStyle></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="City" SortExpression="City" HeaderText="City" DataField="City">
                                                    <HeaderStyle></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn UniqueName="Type" SortExpression="StatusLabel" HeaderText="Type" DataField="StatusLabel">                                                    
                                                    <HeaderStyle></HeaderStyle>
                                                </telerik:GridBoundColumn>
                                                <%--<telerik:GridBoundColumn UniqueName="ContactInfo" HeaderText="Contact Info" SortExpression="Telephone" DataField="Telephone">                                                    
                                                    <HeaderStyle></HeaderStyle>
                                                </telerik:GridBoundColumn>--%>
                                                <telerik:GridBoundColumn UniqueName="Responsible" HeaderText="Néos resp." SortExpression="Responsible" DataField="Responsible">
                                                    <HeaderStyle></HeaderStyle>                                                    
                                                </telerik:GridBoundColumn>
                                                 <%--<telerik:GridBoundColumn UniqueName="Job" HeaderText="Job">                                                    
                                                    <HeaderStyle></HeaderStyle>
                                                </telerik:GridBoundColumn>--%>
                                                <telerik:GridTemplateColumn UniqueName="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDate" DataField="CreatedDate">                                                    
                                                    <HeaderStyle></HeaderStyle>
                                                </telerik:GridTemplateColumn>
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
