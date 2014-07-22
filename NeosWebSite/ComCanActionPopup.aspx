<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ComCanActionPopup.aspx.cs" Inherits="ComCanActionPopup" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Action</title>
    <link href="Styles/Neos.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="script/utils.js"></script>
    <telerik:RadScriptBlock runat="server" ID="scriptBlock">
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
            var isReload = "No";
            currentWindow.argument = isReload;
            currentWindow.close();
            return false;
        }
        
        function OnBtnSaveClientClicked()         
        {
            var currentWindow = GetRadWindow();
            var isReload = "Yes";
            currentWindow.argument = isReload;
            currentWindow.close();
        }
        
        function OnButtonChooseCandidateClientClicked()
        {    
            var radWindow = $find('radWinActionCandidateChoose');
            var url = "ChooseCandidatePopup.aspx?";
            radWindow.setUrl(url);
            radWindow.show();
            
            return false;
        }
        
        function onClientChooseCandidateWindowClosed(window)
        {                   
            if (window.argument != undefined && window.argument != null && window.argument != "")
            {
                var argument = window.argument;
                var argument_array = argument.split("/");                
                var hiddenCandidateId = document.getElementById('hiddenCandidateId');
                hiddenCandidateId.value = argument_array[0];
                
                var ddlCandidate = $find('ddlCandidate');
                ddlCandidate.set_value("-1");
                ddlCandidate.set_text("");
                ddlCandidate.clearItems();  
                
                var candidateItem = new Telerik.Web.UI.RadComboBoxItem();
                candidateItem.set_text(argument_array[1]);
                candidateItem.set_value(argument_array[0]);
                ddlCandidate.get_items().add(candidateItem);
                candidateItem.select();
                ddlCandidate.commitChanges();
            }
        }
        
        function OnButtonChooseCompanyClientClicked()
        {    
            var radWindow = $find('radWinActionCompanyChoose');
            var url = "ChooseCompanyPopup.aspx?";
            radWindow.setUrl(url);
            radWindow.show();
            
            return false;
        }
        
        function onClientChooseCompanyWindowClosed(window)
        {                        
            if (window.argument != undefined && window.argument != null && window.argument != "")
            {                
                var argument = window.argument;
                var argument_array = argument.split("/");                
                var hiddenCompanyId = document.getElementById('hiddenCompanyId');
                hiddenCompanyId.value = argument_array[0];
                               
                var ddlContact = $find('ddlContact');
                ddlContact.set_value("-1");
                ddlContact.set_text("");
                ddlContact.clearItems();      
                  
                var ddlCompany = $find('ddlCompany');
                ddlCompany.clearItems();                
                ddlCompany.trackChanges();
                
                var companyItem = new Telerik.Web.UI.RadComboBoxItem();
                companyItem.set_text(argument_array[1]);
                companyItem.set_value(argument_array[0]);
                ddlCompany.get_items().add(companyItem);
                companyItem.select();
                ddlCompany.commitChanges();
                
                $find("ActionDetailAjaxManager").ajaxRequest("RebindContactListByCompany-" + argument_array[0]);                                 
            }
        }
        
        function onLoadActionDetailPage()
        {
            var actionID = getQueryString("ActionID");
            if(actionID != null && actionID != "") //in edit mode
            {
                var mode = document.getElementById('<%=hidMode.ClientID %>').value;
                if(mode == "edit")
                {
                    processActionToolBar("EditActionDetail");
                }                    
                else if(mode == "view")
                {
                    processActionToolBar("ViewActionDetail");
                }
            }
            return false;
        }
        
        function onform_unload()
        {
            processActionToolBar("UnLoadActionDetailPage");
        }          
            
        function onDropDownCompany_ClientIndexChanged(sender, eventArgs)
        {
             var item = eventArgs.get_item();
             $find("ActionDetailAjaxManager").ajaxRequest("RebindContactListByCompany-" + item.get_value());
        }
        
        function onDropdownCandidate_ClientRequesting(sender, eventArgs)
        {
            var combo = $find("<%= ddlCandidate.ClientID %>");
            var text = combo.get_text();
            if(text.length >= 3)
            {                
                eventArgs.set_cancel(false);
            }
            else
            {
                eventArgs.set_cancel(true);
            }
        }
        
        function onDropdownCompany_ClientRequesting(sender, eventArgs)
        {
            var combo = $find("<%= ddlCompany.ClientID %>");
            var text = combo.get_text();
            if(text.length >0)
            {                
                eventArgs.set_cancel(false);
            }
            else
            {
                eventArgs.set_cancel(true);
            }
        }
                
    </script>
    </telerik:RadScriptBlock>
</head>
<body onbeforeunload="onform_unload()">
    <form id="comActionPopup" runat="server">
    <asp:HiddenField runat="server" ID="urlReferrer" />
    <telerik:RadScriptManager ID="ScriptManager" runat="server" /> 
    <div>
        <telerik:RadAjaxManager EnableAJAX="true" runat="server" ID="ActionDetailAjaxManager" OnAjaxRequest="OnMyAjaxManagerAjaxRequest">                              
        </telerik:RadAjaxManager>
        <table width="100%">
            <tr>
                <td style="width:10%">
                    <asp:Label ID="lblTaskNbr" runat="server" Text="Task nbr"></asp:Label>
                </td>
                <td style="width:20%">
                    <telerik:RadTextBox ID="txtTaskNbr" runat="server" Width="140px" MaxLength="10" ReadOnly="true" Skin="Office2007" BackColor="White"></telerik:RadTextBox>
                </td>
                <td style="width:13%">
                    <asp:Label ID="lblCompany" runat="server" Text="Company"></asp:Label>
                </td>
                <td style="width:15%">                    
                    <telerik:RadComboBox runat="server" ID="ddlCompany" EnableAjaxSkinRendering="true" Skin="Office2007" 
                    Width="148" Height="300" AllowCustomText="true" EnableLoadOnDemand="True" 
                     DataValueField="CompanyID" DataTextField="CompanyName" OnClientSelectedIndexChanged="onDropDownCompany_ClientIndexChanged"
                    OnItemsRequested="OnDropdownCompany_ItemsRequested" OnClientItemsRequesting="onDropdownCompany_ClientRequesting"
                    ></telerik:RadComboBox>
                </td>
                <td style="text-align:left; width:5%">
                    <telerik:RadTextBox ID="txtCompany" runat="server" Width="144px" Skin="Office2007" ReadOnly="true" BackColor="White" Visible="false"></telerik:RadTextBox>
                    <asp:Button ID="btnCompany" runat="server" Width="30px" Text="..." OnClientClick="return OnButtonChooseCompanyClientClicked();"/>
                </td>
                <td  style="width:15%">
                    <asp:Label ID="lblResponsible" runat="server" Text="Responsible"></asp:Label>
                </td>
                <td style="text-align:left;">
                    <telerik:RadComboBox ID="ddlResponsible" runat="server" Width="99%" Skin="Office2007" Height="300"></telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblTypeAction" runat="server" Text="Type"></asp:Label>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlTypeAction" runat="server" Width="144px" Skin="Office2007"></telerik:RadComboBox>
                </td>
                <td>
                    <asp:Label ID="lblCandidate" runat="server" Text="Candidate"></asp:Label>
                </td>
                <td>
                    <telerik:RadComboBox runat="server" ID="ddlCandidate" EnableAjaxSkinRendering="true" Skin="Office2007" 
                        Width="148" Height="300" AllowCustomText="true" EnableLoadOnDemand="True" OnClientItemsRequesting="onDropdownCandidate_ClientRequesting"
                        OnItemsRequested="OnDropdownCandidate_ItemsRequested">
                        </telerik:RadComboBox>
                        <telerik:RadTextBox ID="txtCandidate" runat="server" Width="144px" Visible="false" Enabled="false" Skin="Office2007" BackColor="White"></telerik:RadTextBox>
                </td>
                <td style="text-align:left;">
                    
                    <asp:Button ID="btnCandidate" runat="server" Width="30px" Text="..." OnClientClick="return OnButtonChooseCandidateClientClicked();"/>
                </td>
                 <td>
                    <asp:Label ID="lblAppointmentPlace" runat="server" Text="Appointment place"></asp:Label>
                </td>                
                <td  style="text-align:left">
                    <telerik:RadTextBox ID="txtAppointmentPlace" runat="server" Width="96%" MaxLength="10" Skin="Office2007" BackColor="White"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblCreationDate" runat="server" Text="Creation date"></asp:Label>
                </td>
                <td>
                    <telerik:RadDatePicker ID="datCreationDate" runat="server" Width="145px"  Skin="Office2007"  Calendar-CultureInfo="en-US">
                        <DateInput ID="dateInputCreationDate" runat="server" BackColor="White"
                              DateFormat="dd/MM/yyyy"
                              DisplayDateFormat="dd/MM/yyyy">
                        </DateInput>
                    </telerik:RadDatePicker>
                </td>
                <td>
                    <asp:Label ID="lblContact" runat="server" Text="Contact"></asp:Label>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlContact" runat="server" Width="148px" Skin="Office2007" ></telerik:RadComboBox>
                </td>       
                <td></td>
                 <td>
                    <asp:Label ID="lblActive" runat="server" Text="Active"></asp:Label>
                </td>
                <td>
                    <asp:CheckBox ID="chkActive" runat="server"  Checked="true"></asp:CheckBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblDateAction" runat="server" Text="Action date"></asp:Label>
                </td>
                <td>
                    <telerik:RadDatePicker ID="datDateAction" runat="server" Width="145px"  Skin="Office2007"  Calendar-CultureInfo="en-US">
                        <DateInput ID="dateInputDateAction" runat="server" BackColor="White"
                              DateFormat="dd/MM/yyyy"
                              DisplayDateFormat="dd/MM/yyyy">
                        </DateInput>
                    </telerik:RadDatePicker>
                </td>
                <td>
                    <asp:Label ID="lblCompanyResult" runat="server" Text="Company result"></asp:Label>
                </td>
                <td colspan="4" rowspan="3">
                    <telerik:RadTextBox ID="txtCompanyResult" runat="server" Width="99%" Rows="3" TextMode="multiLine" Skin="Office2007" BackColor="White"></telerik:RadTextBox>
                </td>
            </tr>           
            <tr>
                <td>
                    <asp:Label ID="lblHour" runat="server" Text="Hour"></asp:Label>
                </td>
                <td>
                    <telerik:RadTimePicker runat="server" ID="radTimeHour" Skin="Office2007">
                        <TimeView ID="TimeView1" runat="server" TimeFormat="hh:mm tt">
                        </TimeView>
                    </telerik:RadTimePicker>
                    <%--<telerik:RadDateTimePicker ID="datHour" runat="server" Width="167px"  Skin="Office2007">
                        <DateInput ID="dateInputHour" runat="server" BackColor="White"
                              DateFormat="dd/MM/yyyy hh:mm tt"
                              DisplayDateFormat="dd/MM/yyyy hh:mm tt">
                        </DateInput>
                        <TimeView ID="timeInput" runat="server" TimeFormat="hh:mm tt">
                        </TimeView>
                    </telerik:RadDateTimePicker>--%>
                </td>
            </tr>
             <tr></tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                    <asp:Label ID="lblCandidateResult" runat="server" Text="Candidate result"></asp:Label>
                </td>
                <td colspan="4" rowspan="3">
                    <telerik:RadTextBox ID="txtCandidateResult" runat="server" Width="99%" Rows="3" TextMode="multiLine" Skin="Office2007" BackColor="White"></telerik:RadTextBox>
                </td>
            </tr>
            <tr></tr>
            <tr></tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                <td>
                    <asp:Label ID="lblActionDescription" runat="server" Text="Description"></asp:Label>
                </td>
                <td colspan="4" rowspan="3">
                    <telerik:RadTextBox ID="txtDescription" runat="server" Width="99%" Rows="3" TextMode="multiLine" Skin="Office2007" BackColor="White"></telerik:RadTextBox>
                </td>
            </tr>
            <tr></tr>
            <tr></tr>
            <tr>
                <td>
                </td>
                <td>
                </td>
                 <td>
                    <asp:Label ID="lblExportToOulook" runat="server" Text="Export to outlook"></asp:Label>
                </td>
                <td colspan="4" rowspan="3">
                    <asp:CheckBox ID="chkExportToOutlook" runat="server" Checked="false" />
                </td>
            </tr>
            
        </table>
        <br />
        <asp:HiddenField ID="hiddenCompanyId" runat="server" Value="-1" />
        <asp:HiddenField ID="hiddenCandidateId" runat="server" Value="-1" />        
         <telerik:RadWindow runat="server" ID="radWinActionCompanyChoose" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
            Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="ChooseCompanyPopup.aspx"
            Title="Choose company" Height="400px" Width="750px" OnClientClose="onClientChooseCompanyWindowClosed">
        </telerik:RadWindow>
        <telerik:RadWindow runat="server" ID="radWinActionCandidateChoose" Skin="Office2007" VisibleOnPageLoad="false" VisibleStatusbar="false"
            Modal="true" OffsetElementID="offsetElement" Top="30" Left="30" NavigateUrl="ChooseCandidatePopup.aspx"
            Title="Choose candidate" Height="400px" Width="750px" OnClientClose="onClientChooseCandidateWindowClosed">
        </telerik:RadWindow>
        <div style="text-align:center">
            <asp:Button runat="server" ID="btnEdit" Text="" CssClass="flatButton" Width="60" OnClick="OnBtnEdit_Clicked"/>&nbsp;
            <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="flatButton" Width="60" OnClick="OnBtnSave_Clicked"/>&nbsp;
            <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="flatButton" Width="60" OnClick="OnBtnCancel_Clicked" /> <%--OnClientClick="return OnBtnCancelClientClicked();"--%>
        </div>
    </div>
    
    <asp:HiddenField ID="hidMode" runat="server" />
    </form>
</body>
</html>
