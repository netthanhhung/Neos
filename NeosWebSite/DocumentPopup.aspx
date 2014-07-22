<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentPopup.aspx.cs" Inherits="DocumentPopup" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Document</title>
    <link href="Styles/Neos.css" rel="stylesheet" type="text/css" />
    <link href="Styles/radupload.css" rel="stylesheet" type="text/css" />

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
        
        function addSingleFile()
        {
            var radWindow = GetRadWindow();
            radWindow.setSize(420,250);
        }
        
        function addMultiFile()
        {
            var radWindow = GetRadWindow();
            radWindow.setSize(450,600);
            radWindow.moveTo(300,30);
            
        }
        function addTitle(radUpload, args)
            {
                var curLiEl = args.get_row();
                var firstInput = curLiEl.getElementsByTagName("input")[0];
                
                //Create a simple HTML template.
                var table = document.createElement("table");
                table.className = 'AdditionalInputs';
                table.style.width='100%';
                
                //A new row for a Title field
                row = table.insertRow(-1);
                cell = row.insertCell(-1);
                cell.style.width='20%';
                cell.style.align='right';
                
                var input = CreateInput("Legend", "text");
                input.className = "TextField";
                input.id = input.name = radUpload.getID(input.name);
                var label = CreateLabel("Legend",input.id);
                cell.appendChild(label);
                cell = row.insertCell(-1);
                cell.appendChild(input);
                
                //Add a File label in front of the file input
                //row2 = table.insertRow(-1);
                //cell2 = row2.insertCell(-1);               
                var fileInputSpan = curLiEl.getElementsByTagName("span")[0];
                var firstNode = curLiEl.childNodes[0];
                label = CreateLabel("File",radUpload.getID());
      
                curLiEl.insertBefore(label, firstNode);                
                curLiEl.insertBefore(table, label);
            }
            
            function CreateLabel(text, associatedControlId)
            {
                var label = document.createElement("label");
                label.innerHTML = text;
                label.setAttribute("for", associatedControlId);
                label.style.fontSize = 12;
                
                return label;
            }
            
            function CreateInput(inputName, type)
            {
                var input = document.createElement("input");
                input.type = type;
                input.name = inputName;
                
                return input;
            }
            
    </script>

</head>
<body>
<script type="text/javascript">
    function checkUploadFiles(msg, uploadControl)
    {
        if(areFilesSelected(uploadControl))
        {
            return true;    
        }
        alert(msg);
        return false;
    }
    
    function areFilesSelected(control)
    {
        var upload = $find(control);
        var fileInputs = upload.getFileInputs();
        for (var i=0; i<fileInputs.length; i++)
        {
            if (fileInputs[i].value && fileInputs[i].value.length > 0)
            {
                return true;
            }
        }
        return false;
    }
</script>
    <form id="form1" runat="server">
        <div> 
            <telerik:RadScriptManager ID="ScriptManager" runat="server">
            </telerik:RadScriptManager>
            <telerik:RadProgressManager ID="Radprogressmanager1" runat="server" />
            <div style="text-align: center">
                <asp:Button runat="server" ID="btnAddSingleFile" CssClass="flatButton" CausesValidation="false"
                    OnClientClick="addSingleFile()" OnClick="OnAddSingleFileClick" />&nbsp;
                <asp:Button runat="server" ID="btnAddMultiFile" CssClass="flatButton" CausesValidation="false"
                    OnClientClick="addMultiFile()" OnClick="OnAddMultiFileClick" />
            </div>
            <asp:MultiView runat="server" ID="MViewUploadDoc" ActiveViewIndex="0">
                <asp:View runat="server" ID="SingleUploadView">
                    <table width="100%">
                        <tr>
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 30%; text-align: right;">
                                <asp:Literal runat="server" ID="lblDocumentLegend" Text="Legend"></asp:Literal>:</td>
                            <td style="text-align: left;">
                                <telerik:RadTextBox runat="server" ID="txtDocumentLegend" Width="145"></telerik:RadTextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 20%; text-align: right;">
                                <asp:Literal runat="server" ID="lblFileUpload" Text="File"></asp:Literal>:</td>
                            <td style="text-align: left;">
                                <telerik:RadUpload runat="server" ID="radUploadSingle"
                                    Width="240" Height="20" BackColor="White" ControlObjectsVisibility="None" MaxFileInputsCount="1"
                                    InitialFileInputsCount="1" Skin="Office2007" />
                                <telerik:RadProgressArea runat="server" ID="RadProgressArea1" ProgressIndicators="TotalProgressBar, FilesCountBar, TimeEstimated, TransferSpeed">
                                </telerik:RadProgressArea>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:CheckBox runat="server" ID="chkIsCV" Text="Curriculum Vitae" Checked="true" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%; text-align: right;">
                            </td>
                            <td style="text-align: left;">
                                <asp:Button runat="server" ID="btnSave" CssClass="flatButton" OnClick="OnButtonSaveClick" />&nbsp;
                                <asp:Button runat="server" ID="btnCancel" CssClass="flatButton" OnClientClick="return OnBtnCancelClientClicked();"
                                    CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View runat="server" ID="MultiUpoadView">
                    <table width="100%">
                        <tr>
                            <td>
                                <telerik:RadUpload runat="server" ID="radUploadMulti" Width="400" ControlObjectsVisibility="AddButton"
                                    OnClientAdded="addTitle" InitialFileInputsCount="1" Skin="Office2007" />
                                <telerik:RadProgressArea runat="server" ID="radProgress" ProgressIndicators="TotalProgressBar, FilesCountBar, TimeEstimated, TransferSpeed">
                                </telerik:RadProgressArea>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Button runat="server" ID="btnMultiUploadSave" CssClass="flatButton" OnClick="OnUploadMutiSaveClick" />&nbsp;
                                <asp:Button runat="server" ID="btnMultiUploadCancel" CssClass="flatButton" OnClientClick="return OnBtnCancelClientClicked();"
                                    CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
        </div>
    </form>
</body>
</html>
