var t;
var continueBlinking = false;
var browserWidth = 0;
var browserHeight = 0;
var isIE = true;
function getSize() {
  if( typeof( window.innerWidth ) == 'number' )
  {
    //Non-IE
    isIE = false;
    browserWidth = window.innerWidth;
    browserHeight = window.innerHeight;
  } 
  else 
    if( document.documentElement && ( document.documentElement.clientWidth || document.documentElement.clientHeight ) ) 
    {
        //IE 6+ in 'standards compliant mode'
        browserWidth = document.documentElement.clientWidth;
        browserHeight = document.documentElement.clientHeight;
    } 
    else 
        if( document.body && ( document.body.clientWidth || document.body.clientHeight ) ) {        
    //IE 4 compatible
    browserWidth = document.body.clientWidth;
    browserHeight = document.body.clientHeight; 
    
  }
}
function updateMapSize()
{
    //IE7 is 21 pixels LESS than FireFox.
    //Pretty much we want to keep the map to the maximum size alloted.
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
    
    if(mapWidth < 800)
    {
        
    }
    if(mapHeight < 600)
    {
        
    }
    
    if(document.getElementById("wrapper") != null)
    {
        document.getElementById("wrapper").style.width = (mapWidth-3) + "px";
        document.getElementById("wrapper").style.height = (mapHeight-37) + "px";
    }
    if(document.getElementById("multiPage") != null)
    {
        document.getElementById("multiPage").style.height = (mapHeight - 301) + "px";   
    }   
    //set size for page views content   
    if(document.getElementById("candidatePageContent") != null)
    {
        document.getElementById("candidatePageContent").style.height = (mapHeight - 328) + "px";
    }
    if(document.getElementById("companyPageContent") != null)
    {
        document.getElementById("companyPageContent").style.height = (mapHeight - 328) + "px";
    }
    if(document.getElementById("actionPageContent") != null)
    {
        document.getElementById("actionPageContent").style.height = (mapHeight - 328) + "px";
    }
    if(document.getElementById("jobPageContent") != null)
    {
        document.getElementById("jobPageContent").style.height = (mapHeight - 328) + "px";
    }
    if(document.getElementById("statisticPageContent") != null)
    {
        document.getElementById("statisticPageContent").style.height = (mapHeight - 328) + "px";
    }
    if(document.getElementById("administrationPageContent") != null)
    {
        document.getElementById("administrationPageContent").style.height = (mapHeight - 328) + "px";
    }
    if(document.getElementById("invoicingPageContent") != null)
    {
        document.getElementById("invoicingPageContent").style.height = (mapHeight - 328) + "px";
    }
    if(document.getElementById("notificationPageContent") != null)
    {
        document.getElementById("notificationPageContent").style.height = (mapHeight - 328) + "px";
    }
      
}


function openNeosWindow(url)
{    
    var cfg; 
    if( typeof( window.innerWidth ) == 'number' )
    {
        //Non-IE
        cfg = "fullscreen=yes, location=no, status=yes";
    } 
    else     
    {
        var wth = screen.availWidth;
        var hgt = screen.availHeight;            
        hgt = hgt - 57;
        cfg = "width=" + wth + ",height=" + hgt;
        cfg += ",left=0,top=0,toolbar=no,location=no,directories=no,status=yes,menubar=no,scrollbars=no,resizeable=yes";
    }
    window.open(url, '', config=cfg)
    return false;
}

function getQueryString(ji) 
{
    hu = window.location.search.substring(1);
    gy = hu.split("&");
    for (i=0;i<gy.length;i++) 
    {
        ft = gy[i].split("=");
        if (ft[0] == ji) 
        {
            return ft[1];
        }
    }
}

//function startBlinkingTimer(id)
//{
//    continueBlinking = true;
//    t = self.setInterval("BlinkingTheText('" + id + "')", 800);
//    if(document.getElementById('timerId') != null)
//        document.getElementById('timerId').value = t;
//}

//function stopBlinkingTimer()
//{
//    continueBlinking = false;
//    if(!t)
//    {
//        if(document.getElementById('timerId') != null)
//            t = document.getElementById('timerId').value;
//        else if(window.parent.document.getElementById('timerId') != null)
//            t = window.parent.document.getElementById('timerId').value;
//    }
//    t = window.clearInterval(t);
//}

//function BlinkingTheText(id)
//{      
//    if(document.getElementById(id) != null)
//    {
//        if(document.getElementById(id).style.fontWeight == 'bold')
//            document.getElementById(id).style.fontWeight = 'normal';
//        else
//            document.getElementById(id).style.fontWeight = 'bold';
//    }
//    else if(window.parent.document.getElementById(id) != null)
//    {
//          if(window.parent.document.getElementById(id).style.fontWeight == 'bold')
//            window.parent.document.getElementById(id).style.fontWeight = 'normal';
//        else
//            window.parent.document.getElementById(id).style.fontWeight = 'bold';
//    }
//     
//}
function resetUnreadMessagesText(id, text, isBold)
{
    if(window.parent.document.getElementById(id) != null)
    {
        window.parent.document.getElementById(id).innerHTML = text;
        if(isBold.toLowerCase() == "true")
        {
            window.parent.document.getElementById(id).style.fontWeight = 'bold';
//            window.parent.document.getElementById('flagUnreadMsg').value = "1";
        }
        else
        {            
            window.parent.document.getElementById(id).style.fontWeight = 'normal';
        }
    }
}
    
function centerLoginControl()
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
    if(document.getElementById("login") != null)
    {
        document.getElementById("login").style.left = (mapWidth/2 - 168) + "px";
        document.getElementById("login").style.top = (mapHeight/2 - 140) + "px";
    }
}

function redirectToLoginPage(loginUrl) 
{
    top.location.href = loginUrl;
}

function sessionKeepAlive(loginUrl) {
 var wRequest = new Sys.Net.WebRequest();
 wRequest.set_url(loginUrl);
 wRequest.set_httpVerb("POST");
 wRequest.add_completed(sessionKeepAlive_Callback);
 wRequest.set_body();
 wRequest.get_headers()["Content-Length"] = 0;
 wRequest.invoke();
}

function sessionKeepAliveNoUrl() {
 var wRequest = new Sys.Net.WebRequest();
 wRequest.set_url("SessionKeepAlive.aspx");
 wRequest.set_httpVerb("POST");
 wRequest.add_completed(sessionKeepAlive_Callback);
 wRequest.set_body();
 wRequest.get_headers()["Content-Length"] = 0;
 wRequest.invoke();
}

function sessionKeepAlive_Callback(executor, eventArgs)
{
        
}

function openPopUp(url)
{
    newwindow = window.open(url,'','', ''); 
    return false;
}

function OnBirthDateClientDataChanged()
{    
    var txtAge = $find('txtAge');        
    var calendar = $find('datDateOfBirth');
    var dates = calendar.get_selectedDate();
    if(dates != null)
    {
        var todaysDate = new Date();
        var age = getAge(dates);
        if(age > 0) 
        {        
            txtAge.set_value(age);
        } 
        else 
        {
            alert("Invalid day");
            calendar.clear();
        }
    }
}

//function getAge(birthdate) 
//{
//    debugger;
//     month = (birthdate.getMonth() - 1);
//     date = birthdate.getDate();
//     year = birthdate.getFullYear();
//     
//     today = new Date();
//     dateStr = today.getDate();
//     monthStr = today.getMonth();
//     yearStr = today.getFullYear();

//     theYear = yearStr - year;
//     theMonth = monthStr - month;
//     theDate = dateStr - date;

//     var days = "";
//     if (monthStr == 0 || monthStr == 2 || monthStr == 4 || monthStr == 6 || monthStr == 7 || monthStr == 9 || monthStr == 11) days = 31;
//     if (monthStr == 3 || monthStr == 5 || monthStr == 8 || monthStr == 10) days = 30;
//     if (monthStr == 1) days = 28;

//     var inYears = theYear;
//        var inMonths;
//        var inDays;
//     if (month < monthStr && date > dateStr) { inYears = parseInt(inYears) + 1;
//                                               inMonths = theMonth - 1; }
//     if (month < monthStr && date <= dateStr) { inMonths = theMonth; }
//     else if (month == monthStr && (date < dateStr || date == dateStr)) { inMonths = 0; }
//     else if (month == monthStr && date > dateStr) { inMonths = 11; }
//     else if (month > monthStr && date <= dateStr) { inYears = inYears - 1;
//                                                     inMonths = ((12 - -(theMonth)) + 1); }
//     else if (month > monthStr && date > dateStr) { inMonths = ((12 - -(theMonth))); }

//     if (date < dateStr) { inDays = theDate; }
//     else if (date == dateStr) { inDays = 0; }
//     else { inYears = inYears - 1; inDays = days - (-(theDate)); }

//    return inYears; 
//}
  
  function getAge(birthdate) 
  {
     debugger;
     monthBirth = birthdate.getMonth();
     dateBirth = birthdate.getDate();
     yearBirth = birthdate.getFullYear();
     
     today = new Date();
     dateToday = today.getDate();
     monthToday = today.getMonth();
     yearToday = today.getFullYear();
     
     var age = yearToday - yearBirth;
     if((monthToday < monthBirth) || ((monthToday == monthBirth) && (dateToday < dateBirth))) 
     {
        age = age - 1;
     }
     return age;
  }
  
function CheckItemExistInListBox (listBox, optionText, optionValue){
  var sltbool = false;
  for (i=0; i<listBox.options.length; i++)
  {
     if (optionText == listBox.options[i].text ||  optionValue == listBox.options[i].text )
     {
        sltbool = true;
        break
     }
  }
  return sltbool;
}


function AddItemToListBox(listBox, optionText, optionValue) {
    if ( !CheckItemExistInListBox (listBox, optionText, optionValue) )
    {   
        var opt = document.createElement("option");
        // Assign text and value to Option object
        opt.text = optionText;
        opt.value = optionValue;
        listBox.options.add(opt);
    }    
}

function DeleteItemInListBox(listBox, optionRank) {
    if (listBox.options.length != 0) 
    { 
        listBox.options[optionRank] = null 
    }
}
function ReAddItemToHiddenField(listBox, hiddenField) 
{
    hiddenField.value = "";
    for (var i = 0; i < listBox.options.length; i++)
    {
        if(i == 0) 
        {
            hiddenField.value =  listBox.options[i].value;
        } 
        else 
        {
            hiddenField.value = hiddenField.value + "; " + listBox.options[i].value
        }
        
    }
}



function OnButtonAddCanAreaClientClicked()
{ 
    var ddlCanArea =  $find('ddlCanArea');
    var listCanArea = document.getElementById('listCanArea');
    var hiddenCanAreaList = document.getElementById('hiddenCanAreaList');
    var selectedArea = ddlCanArea._selectedItem._text;
    AddItemToListBox(listCanArea, selectedArea, selectedArea);
    ReAddItemToHiddenField(listCanArea, hiddenCanAreaList);
    return false;
}

function OnButtonRemoveCanAreaClientClicked()
{    
    var listCanArea = document.getElementById('listCanArea');   
    var hiddenCanAreaList = document.getElementById('hiddenCanAreaList'); 
    for (var i = 0; i < listCanArea.options.length; i++)
    {
        if ((listCanArea.options[i].selected == true))
        {
            listCanArea.options[i] = null;
            i--;
        }
    }
    ReAddItemToHiddenField(listCanArea, hiddenCanAreaList);
    return false;
}

function onClientCanContactDetailWindowClosed(window)
{
    if (window.argument != null && window.argument != undefined)
    {        
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("MyAjaxManager").ajaxRequest("RebindCanContactGrid");
        }
    }
    onLoadCandidateProfilePage();
}
function onClientCanStudyDetailWindowClosed(window)
{    
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("MyAjaxManager").ajaxRequest("RebindCanStudyGrid");
        }
    }
    onLoadCandidateProfilePage();
}

function onClientCanExperienceDetailWindowClosed(window)
{        
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("MyAjaxManager").ajaxRequest("RebindCanExperienceGrid");
        }
    }
    onLoadCandidateProfilePage();
}
function onClientCanActionDetailWindowClosed(window)
{
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("MyAjaxManager").ajaxRequest("RebindCanActionGrid");
        }
    }
    onLoadCandidateProfilePage();
}

function onClientRadDocumentWindowClosed(window)
{        
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {            
            var grid = $find("grdDocuments");            
            $find("MyAjaxManager").ajaxRequest("RebindCanDocumentGrid");
        }
    }
    onLoadCandidateProfilePage();
}

function onClientRadComDocumentWindowClosed(window)
{        
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {            
            var grid = $find("grdDocuments");            
            $find("CompanyProfileAjaxManager").ajaxRequest("RebindComDocumentGrid");
        }
    }
    onLoadCompanyProfilePage();
}

function onClientRadPreComDocumentWindowClosed(window)
{        
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {            
            var grid = $find("grdDocuments");            
            $find("MyAjaxManager").ajaxRequest("RebindComDocumentGrid");
        }
    }    
}

function onSendPresentationEmailWindowClosed(window)
{        
    
}

function OnBtnShowSendPresentationEmailClicked()
{        
    var radWindow = $find('radWinSendPresentation');
    var url = "SendPresentationEmail.aspx";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}      

function OnDocumentEditClientClicked(candID,docID)
{
    var radWindow = $find('radWindowDocument');
    var url;
    if(docID != "")
        url = "DocumentPopup.aspx?docID=" + docID;
    else
        url = "DocumentPopup.aspx?candID=" + candID;
    radWindow.setSize(400,250);
    radWindow.moveTo(300,30);
    radWindow.setUrl(url);    
    radWindow.show();
    
    return false;
}

function OnComDocumentEditClientClicked(compID,docID)
{
    var radWindow = $find('radWindowComDocument');
    var url;
    if(docID != "")
        url = "CompanyDocumentPopup.aspx?docID=" + docID;
    else
        url = "CompanyDocumentPopup.aspx?compID=" + compID;
    radWindow.setSize(400,250);
    radWindow.moveTo(300,30);
    radWindow.setUrl(url);    
    radWindow.show();
    
    return false;
}


function OnCandidateContactEditClientClicked(telephoneID)
{    
    var canTelephonID = telephoneID;
    var radWindow = $find('radWindowCanContact');
    var url = "CandidateTelephonePopup.aspx?TelePhoneId=" + canTelephonID;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OncandidateContactAddClientClicked(candID)
{    
    
    var radWindow = $find('radWindowCanContact');
    var url = "CandidateTelephonePopup.aspx?candidateID=" + candID;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnCandidateStudyEditClientClicked(candidateFormationID) 
{
    var canStudyID = candidateFormationID;
    var radWindow = $find('radWindowCanStudy');
    var url = "CandidateStudyPopup.aspx?CandidateFormationID=" + canStudyID;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnAddNewCanStudyClientClicked()
{    
    var radWindow = $find('radWindowCanStudy');
    var url = "CandidateStudyPopup.aspx?";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnCandidateExperieceEditClientClicked(experienceID)
{
    var canExperienceID = experienceID;
    var radWindow = $find('radWindowCanExperience');
    var url = "CandidateExperiencePopup.aspx?ExperienceID=" + canExperienceID;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnAddNewCanExperienceClientClicked()
{    
    var radWindow = $find('radWindowCanExperience');
    var url = "CandidateExperiencePopup.aspx?";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnCandidateActionEditClientClicked(actionID)
{
    var canActionID = actionID;
    var radWindow = $find('radWindowCanAction');
    var url = "ComCanActionPopup.aspx?ActionID=" + canActionID + "&type=candidate";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnAddNewCanActionClientClicked()
{    
    var radWindow = $find('radWindowCanAction');
    var url = "ComCanActionPopup.aspx?" + "type=candidate";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnCompanyActionEditClientClicked(actionID)
{
    var canActionID = actionID;
    var radWindow = $find('radWindowComAction');
    var url = "ComCanActionPopup.aspx?ActionID=" + canActionID + "&type=company";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnAddNewComActionClientClicked()
{    
    var radWindow = $find('radWindowComAction');
    var url = "ComCanActionPopup.aspx?" + "type=company";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnInvoiceCoordinateEditClientClicked(addressId)
{    
    var radWindow = $find('radWindowInvoiceCoordinate');
    var url = "InvoiceCoordinatePopup.aspx?AddressId=" + addressId;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnAddNewInvoiceCoordinateClientClicked(companyId)
{    
    var radWindow = $find('radWindowInvoiceCoordinate');
    var url = "InvoiceCoordinatePopup.aspx";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnCandidateAdvancedSearchClick()
{
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "CandidateAdvancedSearch.aspx";
    pane.set_contentUrl(url);    
    return false;
}

function OnLast5CandidateItemClicked(candidatId)
{
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "CandidateProfile.aspx?CandidateID=" + candidatId + "&mode=edit";
    pane.set_contentUrl(url);    
    return false;
}

function OnLast5CompanyItemClicked(companyId)
{
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "CompanyProfile.aspx?CompanyId=" + companyId + "&mode=edit";
    pane.set_contentUrl(url);    
    return false;
}

function OnSearchActionClick(type) 
{    
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "Actions.aspx?type=" + type;
    pane.set_contentUrl(url);    
    return false;
}

function OnSearchJobClick(mode) 
{    
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "Jobs.aspx?mode=" + mode;
    pane.set_contentUrl(url);    
    return false;
}

function OnUserAndPermissionClick() 
{    
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "Administration/AdminUserAndPermission.aspx";
    pane.set_contentUrl(url);    
    return false;
}

function OnButtonAddPermissionClientClicked()
{     
    var ddlPermission =  $find('ddlPermission');
    var listPermission = document.getElementById('listPermission');
    var hiddenPermissionList = document.getElementById('hiddenPermissionList');
    var selectedPermission = ddlPermission._selectedItem._text;
    AddItemToListBox(listPermission, selectedPermission, selectedPermission);
    ReAddItemToHiddenField(listPermission, hiddenPermissionList);
    return false;
}

function OnButtonRemovePermissionClientClicked()
{      
    var listPermission = document.getElementById('listPermission');   
    var hiddenPermissionList = document.getElementById('hiddenPermissionList'); 
    for (var i = 0; i < listPermission.options.length; i++)
    {
        if ((listPermission.options[i].selected == true))
        {
            listPermission.options[i] = null;
            i--;
        }
    }
    ReAddItemToHiddenField(listPermission, hiddenPermissionList);
    return false;
}

function OnActionEditClientClicked(actionID)
{    
    var radWindow = $find('radWindowAction');
    var url = "ComCanActionPopup.aspx?ActionID=" + actionID + "&type=action";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnAddNewActionClientClicked()
{    
    var radWindow = $find('radWindowAction');
    var url = "ComCanActionPopup.aspx?" + "type=action";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function onClientActionDetailWindowClosed(window)
{
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("ActionAjaxManager").ajaxRequest("RebindActionGrid");
        }
    }
}

function OnAddNewUserClientClicked()
{    
    var radWindow = $find('radWindowUser');
    var url = "AdminUserPopup.aspx";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnUserEditClientClicked(userID)
{  
    var radWindow = $find('radWindowUser');
    var url = "AdminUserPopup.aspx?UserID=" + userID;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnPermissionsClick() 
{    
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "Administration/AdminPermissions.aspx";
    pane.set_contentUrl(url);    
    return false;
}

function OnAddNewPermissionClientClicked()
{    
    var radWindow = $find('radWindowPermission');
    var url = "AdminPermissionPopup.aspx";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnPermissionEditClientClicked(permission)
{        
    var radWindow = $find('radWindowPermission');
    var url = "AdminPermissionPopup.aspx?Code=" + permission;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function onClientPermissionDetailWindowClosed(window)
{
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("MyAjaxManager").ajaxRequest("RebindPermissionGrid");
        }
    }
}

function OnLocationsClick() 
{    
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "Administration/AdminLocations.aspx";
    pane.set_contentUrl(url);    
    return false;
}

function OnAddNewLocationClientClicked()
{    
    var radWindow = $find('radWindowLocation');
    var url = "AdminLocationPopup.aspx";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnLocationEditClientClicked(location)
{        
    var radWindow = $find('radWindowLocation');
    var url = "AdminLocationPopup.aspx?Location=" + location;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function onClientLocationDetailWindowClosed(window)
{
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("MyAjaxManager").ajaxRequest("RebindLocationGrid");
        }
    }
}

function OnHypUnitsClick() 
{        
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "Administration/AdminUnits.aspx";
    pane.set_contentUrl(url);    
    return false;
}

function OnAddNewUnitClientClicked()
{    
    var radWindow = $find('radWindowUnit');
    var url = "AdminUnitPopup.aspx";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnUnitEditClientClicked(unit)
{        
    var radWindow = $find('radWindowUnit');
    var url = "AdminUnitPopup.aspx?TypeID=" + unit;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function onClientUnitDetailWindowClosed(window)
{
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("MyAjaxManager").ajaxRequest("RebindUnitGrid");
        }
    }
}

function OnHypProfilesClick() 
{        
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "Administration/AdminProfiles.aspx";
    pane.set_contentUrl(url);    
    return false;
}

function OnAddNewProfileClientClicked()
{    
    var radWindow = $find('radWindowProfile');
    var url = "AdminProfilePopup.aspx";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnProfileEditClientClicked(profileID)
{        
    var radWindow = $find('radWindowProfile');
    var url = "AdminProfilePopup.aspx?ProfileID=" + profileID;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function onClientProfileDetailWindowClosed(window)
{
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("MyAjaxManager").ajaxRequest("RebindProfileGrid");
        }
    }
}

function OnHypFunctionFamClick() 
{        
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "Administration/AdminFunctionFam.aspx";
    pane.set_contentUrl(url);    
    return false;
}

function OnAddNewFunctionFamClientClicked()
{    
    var radWindow = $find('radWindowFunctionFam');
    var url = "AdminFunctionFamPopup.aspx";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnFunctionFamEditClientClicked(functionFamID)
{        
    var radWindow = $find('radWindowFunctionFam');
    var url = "AdminFunctionFamPopup.aspx?FunctionFamID=" + functionFamID;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function onClientFunctionFamDetailWindowClosed(window)
{
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("MyAjaxManager").ajaxRequest("RebindFunctionFamGrid");
        }
    }
}

function OnHypFunctionClick() 
{        
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "Administration/AdminFunction.aspx";
    pane.set_contentUrl(url);    
    return false;
}

function OnAddNewFunctionClientClicked()
{    
    var radWindow = $find('radWindowFunction');
    var url = "AdminFunctionPopup.aspx";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnFunctionEditClientClicked(functionID)
{        
    var radWindow = $find('radWindowFunction');
    var url = "AdminFunctionPopup.aspx?FunctionID=" + functionID;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function onClientFunctionDetailWindowClosed(window)
{
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("MyAjaxManager").ajaxRequest("RebindFunctionGrid");
        }
    }
}

function OnHypKnowledgeFamClick() 
{        
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "Administration/AdminKnowledgeFam.aspx";
    pane.set_contentUrl(url);    
    return false;
}

function OnAddNewKnowledgeFamClientClicked()
{    
    var radWindow = $find('radWindowKnowledgeFam');
    var url = "AdminKnowledgeFamPopup.aspx";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnKnowledgeFamEditClientClicked(knowledgeFamID)
{        
    var radWindow = $find('radWindowKnowledgeFam');
    var url = "AdminKnowledgeFamPopup.aspx?KnowledgeFamID=" + knowledgeFamID;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function onClientKnowledgeFamDetailWindowClosed(window)
{
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("MyAjaxManager").ajaxRequest("RebindKnowledgeFamGrid");
        }
    }
}

function OnHypKnowledgeClick() 
{        
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "Administration/AdminKnowledge.aspx";
    pane.set_contentUrl(url);    
    return false;
}

function OnAddNewKnowledgeClientClicked()
{    
    var radWindow = $find('radWindowKnowledge');
    var url = "AdminKnowledgePopup.aspx";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnKnowledgeEditClientClicked(knowledgeID)
{        
    var radWindow = $find('radWindowKnowledge');
    var url = "AdminKnowledgePopup.aspx?KnowledgeID=" + knowledgeID;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function onClientKnowledgeDetailWindowClosed(window)
{
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("MyAjaxManager").ajaxRequest("RebindKnowledgeGrid");
        }
    }
}

function OnHypStudyLevelClick() 
{        
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "Administration/AdminStudyLevel.aspx";
    pane.set_contentUrl(url);    
    return false;
}

function OnAddNewStudyLevelClientClicked()
{    
    var radWindow = $find('radWindowStudyLevel');
    var url = "AdminStudyLevelPopup.aspx";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnStudyLevelEditClientClicked(studyLevelID)
{        
    var radWindow = $find('radWindowStudyLevel');
    var url = "AdminStudyLevelPopup.aspx?StudyLevelID=" + studyLevelID;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function onClientStudyLevelDetailWindowClosed(window)
{
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("MyAjaxManager").ajaxRequest("RebindStudyLevelGrid");
        }
    }
}

function OnHypStudyClick() 
{        
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "Administration/AdminStudy.aspx";
    pane.set_contentUrl(url);    
    return false;
}

function OnAddNewStudyClientClicked()
{    
    var radWindow = $find('radWindowStudy');
    var url = "AdminStudyPopup.aspx";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnStudyEditClientClicked(studyID)
{        
    var radWindow = $find('radWindowStudy');
    var url = "AdminStudyPopup.aspx?StudyID=" + studyID;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function onClientStudyDetailWindowClosed(window)
{
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("MyAjaxManager").ajaxRequest("RebindStudyGrid");
        }
    }
}

function OnHypTypeActionClick() 
{        
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "Administration/AdminTypeAction.aspx";
    pane.set_contentUrl(url);    
    return false;
}

function OnAddNewTypeActionClientClicked()
{    
    var radWindow = $find('radWindowTypeAction');
    var url = "AdminTypeActionPopup.aspx";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnTypeActionEditClientClicked(typeActionID)
{        
    var radWindow = $find('radWindowTypeAction');
    var url = "AdminTypeActionPopup.aspx?TypeActionID=" + typeActionID;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function onClientTypeActionDetailWindowClosed(window)
{
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("MyAjaxManager").ajaxRequest("RebindTypeActionGrid");
        }
    }
}

function OnHypLanguageClick() 
{        
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "Administration/AdminLanguage.aspx";
    pane.set_contentUrl(url);    
    return false;
}

function OnAddNewLanguageClientClicked()
{    
    var radWindow = $find('radWindowLanguage');
    var url = "AdminLanguagePopup.aspx";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnLanguageEditClientClicked(languageID)
{        
    var radWindow = $find('radWindowLanguage');
    var url = "AdminLanguagePopup.aspx?LanguageID=" + languageID;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function onClientLanguageDetailWindowClosed(window)
{
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("MyAjaxManager").ajaxRequest("RebindLanguageGrid");
        }
    }
}

function OnHypNationalityClick() 
{        
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "Administration/AdminNationality.aspx";
    pane.set_contentUrl(url);    
    return false;
}

function OnAddNewNationalityClientClicked()
{    
    var radWindow = $find('radWindowNationality');
    var url = "AdminNationalityPopup.aspx";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnNationalityEditClientClicked(nationalityID)
{        
    var radWindow = $find('radWindowNationality');
    var url = "AdminNationalityPopup.aspx?NationalityID=" + nationalityID;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function onClientNationalityDetailWindowClosed(window)
{
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("MyAjaxManager").ajaxRequest("RebindNationalityGrid");
        }
    }
}

function OnHypSituationCivilClick() 
{        
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "Administration/AdminSituationCivil.aspx";
    pane.set_contentUrl(url);    
    return false;
}

function OnAddNewSituationCivilClientClicked()
{    
    var radWindow = $find('radWindowSituationCivil');
    var url = "AdminSituationCivilPopup.aspx";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnSituationCivilEditClientClicked(code)
{        
    var radWindow = $find('radWindowSituationCivil');
    var url = "AdminSituationCivilPopup.aspx?Code=" + code;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function onClientSituationCivilDetailWindowClosed(window)
{
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("MyAjaxManager").ajaxRequest("RebindSituationCivilGrid");
        }
    }
}

function OnHypClientStatusClick() 
{        
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "Administration/AdminClientStatus.aspx";
    pane.set_contentUrl(url);    
    return false;
}

function OnAddNewClientStatusClientClicked()
{    
    var radWindow = $find('radWindowClientStatus');
    var url = "AdminClientStatusPopup.aspx";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnClientStatusEditClientClicked(statusID)
{        
    var radWindow = $find('radWindowClientStatus');
    var url = "AdminClientStatusPopup.aspx?StatusID=" + statusID;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function onClientClientStatusDetailWindowClosed(window)
{
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("MyAjaxManager").ajaxRequest("RebindClientStatusGrid");
        }
    }
}

function OnHypLegalFormClick() 
{        
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "Administration/AdminLegalForm.aspx";
    pane.set_contentUrl(url);    
    return false;
}

function OnAddNewLegalFormClientClicked()
{    
    var radWindow = $find('radWindowLegalForm');
    var url = "AdminLegalFormPopup.aspx";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnLegalFormEditClientClicked(formID)
{        
    var radWindow = $find('radWindowLegalForm');
    var url = "AdminLegalFormPopup.aspx?FormID=" + formID;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function onClientLegalFormDetailWindowClosed(window)
{
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("MyAjaxManager").ajaxRequest("RebindLegalFormGrid");
        }
    }
}

function OnHypContactFunctionClick() 
{        
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "Administration/AdminContactFunction.aspx";
    pane.set_contentUrl(url);    
    return false;
}

function OnAddNewContactFunctionClientClicked()
{    
    var radWindow = $find('radWindowContactFunction');
    var url = "AdminContactFunctionPopup.aspx";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnContactFunctionEditClientClicked(contactFunctionID)
{        
    var radWindow = $find('radWindowContactFunction');
    var url = "AdminContactFunctionPopup.aspx?ContactFunctionID=" + contactFunctionID;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}


function OnInvoiceDetailEditClientClicked(invoiceDetailId) 
{    
    var radWindow = $find('radWinInvoiceDetail');
    var url = "InvoiceDetailPopup.aspx?InvoiceDetailId=" + invoiceDetailId;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnAddNewInvoiceDetailClientClicked(invoiceIdPK) 
{
    var radWindow = $find('radWinInvoiceDetail');
    var url = "InvoiceDetailPopup.aspx?InvoiceIdPK=" + invoiceIdPK;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnInvoicePaymentEditClientClicked(idPayment) 
{
    var radWindow = $find('radWinInvoicePayment');
    var url = "InvoicePaymentPopup.aspx?IdPayment=" + idPayment;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function OnAddNewInvoicePaymentClientClicked(invoiceIdPK) 
{
    var radWindow = $find('radWinInvoicePayment');
    var url = "InvoicePaymentPopup.aspx?InvoiceIdPK=" + invoiceIdPK;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}
function onClientContactFunctionDetailWindowClosed(window)
{
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("MyAjaxManager").ajaxRequest("RebindContactFunctionGrid");
        }
    }
}

function onClientUserDetailWindowClosed(window)
{
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("MyAjaxManager").ajaxRequest("RebindUserGrid");
        }
    }
}

function OnHypStudyLevelStatisticsClick() 
{        
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "StudyLevelStatistics.aspx";
    pane.set_contentUrl(url);    
    return false;
}

function OnHypCanInscriptionStatisticsClick()
{
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "StatisticsCanidateInscription.aspx";
    pane.set_contentUrl(url);    
    return false;
}

function OnHypCanLocationStatisticsClick()
{
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "StatisticsCandidateLocation.aspx";
    pane.set_contentUrl(url);    
    return false;
}

function OnHypGeneralStatisticsClick()
{
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "StatisticsGeneral.aspx";
    pane.set_contentUrl(url);    
    return false;
}

function OnUpaidInvoiceClick() 
{        
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "InvoiceUnpaidPage.aspx?type=unpaid";
    pane.set_contentUrl(url);    
    return false;
}

function OnFutureInvoiceClick() 
{        
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "InvoicesPage.aspx?type=future";
    pane.set_contentUrl(url);    
    return false;
}

function OnTurnoverInvoiceClick() 
{        
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    var url = "InvoiceTurnover.aspx";
    pane.set_contentUrl(url);    
    return false;
}


//function addCompanyClientClick()
//{    
//    var radWindow = $find('radWindowCompany');
//    var url = "CompanyProfile.aspx";
//    radWindow.setUrl(url);
//    radWindow.show();
//    
//    return false;
//}

function onClientComActionDetailWindowClosed(window)
{
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("CompanyProfileAjaxManager").ajaxRequest("RebindComActionGrid");
        }
    }
}

function OnSearchAllCompanyClick()
{
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
        pane.set_contentUrl("Companies.aspx?type=all");
      return false;
}
function OnSearchCustomerCompanyClick()
{
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
        pane.set_contentUrl("Companies.aspx?type=client");
      return false;
}
function OnSearchProspectCompanyClick()
{
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
        pane.set_contentUrl("Companies.aspx?type=prospect");
      return false;
}
function OnSearchInactiveCompanyClick()
{
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
        pane.set_contentUrl("Companies.aspx?type=inactive");
      return false;
}

function OnAddNewCompanyContactClientClicked(companyID)
{
    var radWindow = $find('radWindowCompanyContact');
    var url = "CompanyContactPopup.aspx?companyID=" + companyID;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}
function OnEditCompanyContactClientClicked(contactId)
{
    var radWindow = $find('radWindowCompanyContact');
    var url = "CompanyContactPopup.aspx?contactID=" + contactId;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}
function OnAddNewContactInfoClientClicked(contactID)
{
    var radWindow = $find('radWindowContactInfo');
    var url = "ContactInfoPopup.aspx?contactId=" + contactID;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}
function OnEditContactInfoClientClicked(contactInfoId, contactId)
{    
    var radWindow = $find('radWindowContactInfo');
    var url = "ContactInfoPopup.aspx?contactInfoId=" + contactInfoId + "&contactId=" + contactId;
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function onClientContactInfoWindowClosed(window)
{
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("CompanyProfileAjaxManager").ajaxRequest("RebindContactInfoGrid");
        }
    }
}
function onClientComContactWindowClosed(window)
{

    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("CompanyProfileAjaxManager").ajaxRequest("RebindCompanyContactGrid");
        }
    }
}

function onClientInvoiceCoordinateDetailWindowClosed(window) 
{
    if (window.argument != null && window.argument != undefined)
    {
        var isReload = window.argument;
        if(isReload == "Yes") 
        {                                  
            $find("CompanyProfileAjaxManager").ajaxRequest("RebindInvoiceCoordinateGrid");
        }
    }
}

function OnAddNewJobClick()
{    
    var radWindow = $find('radWindowJob');
    var url = "JobPopup.aspx?mode=add";
    radWindow.setUrl(url);
    radWindow.show();
    
    return false;
}

function onToolBar_ClientClicking(sender, args)
            {
                
                 var splitter = $find('RadSplitterBrowser');
                 var pane = splitter.GetPaneById('radPaneContent');
                 if (!pane) return;
                
                 var toolBar = sender;
                 var button = args.get_item();
                 
                 switch(button.get_value())
                 {
                    ////candidate toolbar
                    case "newcandidateDefault":
                    case "newcandidate":                        
                        pane.set_contentUrl("CandidateProfile.aspx?mode=edit");
                        break;
                    case "newcompanyDefault":
                    case "newcompany":
                        pane.set_contentUrl("CompanyProfile.aspx?mode=edit");
                        break;
                    case "newactionDefault":
                    case "newaction":
                        pane.set_contentUrl("ActionDetails.aspx?type=action&mode=edit");                
                        break;
                    case "newjobDefault":
                    case "newjob":
                        pane.set_contentUrl("JobProfile.aspx?mode=edit");
                        break;
                    case "newinvoicingDefault":
                    case "newinvoicing":
                        pane.set_contentUrl("InvoiceProfile.aspx?mode=edit");
                        break;                
                    case "opencandidate":
                        processCandidate(pane, "OpenSelectedCandidate","CandidateAjaxManager");
                    break;
                    case "deletecandidate":
                        var confirm_msg = document.getElementById('ConfirmDeleteCandidate').value;
                        if(confirm(confirm_msg))
                        {
                            processCandidate(pane, "DeletedSelectedCandidate","CandidateAjaxManager");
                        }
                    break;
                    //MyAjaxManager: ajax manager control in candidateprofile.aspx page
                    case "vieweditcandidate":
                        processCandidate(pane, "ViewEditCandidateProfile", "MyAjaxManager");
                    break;
                    case "advancesearch":
                        pane.set_contentUrl("CandidateAdvancedSearch.aspx");
                    break;
                    case "savecandidate":
                        processCandidate(pane, "SaveCandidateProfile", "MyAjaxManager");
                    break;
                    case "viewcandidateactions":
                        if(processCandidate(pane, "ViewCandidateActions", "MyAjaxManager") == null)
                            processCandidate(pane, "ViewCandidateActions", "CandidateAjaxManager")
                    break;
                    case "addcandidateactions":
                        processCandidate(pane, "AddCandidateActions", "MyAjaxManager");
                    break;                                        
                    //company toolbar
                    case "opencompany":
                        processCandidate(pane, "OpenSeletectedCompany", "CompanyAjaxManager");
                    break;
                    case "deletecompany":
                        var confirm_msg = document.getElementById('ConfirmDeleteCompany').value;
                        if(confirm(confirm_msg))
                        {
                            processCandidate(pane, "DeleteSelectedCompany", "CompanyAjaxManager");
                        }
                    break;
                    case "vieweditcompany":
                        processCandidate(pane, "ViewEditCompanyProfile", "CompanyProfileAjaxManager");
                    break;
                    case "savecompany":
                        processCandidate(pane, "SaveCompanyProfile", "CompanyProfileAjaxManager");
                    break;
                    case "viewcompanyactions":
                        if(processCandidate(pane, "ViewCompanyActions", "CompanyAjaxManager") == null)
                            processCandidate(pane, "ViewCompanyActions", "CompanyProfileAjaxManager");
                    break;
                    case "addcompanyaction":
                        processCandidate(pane, "AddCompanyAction", "CompanyProfileAjaxManager");
                    break;
                    case "viewcompanyjobs":                        
                         if(processCandidate(pane, "ViewCompanyJobs", "CompanyAjaxManager") == null)
                            processCandidate(pane, "ViewCompanyJobs", "CompanyProfileAjaxManager");
                    break;
                    case "addcompanyjobs":
                        processCandidate(pane, "AddCompanyJob", "CompanyProfileAjaxManager");
                    break;
                    case "viewcompanyinvoices":
                        if(processCandidate(pane, "ViewCompanyInvoices", "CompanyAjaxManager") == null)
                            processCandidate(pane, "ViewCompanyInvoices", "CompanyProfileAjaxManager");
                    break;
                    case "addcompanyinvoice":
                        processCandidate(pane, "AddCompanyInvoice", "CompanyProfileAjaxManager");
                    break;
                    //action toolbar
                    case "openaction":
                        processCandidate(pane, "OpenSelectedAction", "ActionAjaxManager");
                    break;
                    case "deleteaction":
                        var confirm_msg = document.getElementById('ConfirmDeleteAction').value;
                        if(confirm(confirm_msg))
                        {
                            processCandidate(pane, "DeleteSelectedAction", "ActionAjaxManager");
                        }
                    break;    
                    case "vieweditaction":
                        processCandidate(pane, "ViewEditActionDetail", "ActionDetailAjaxManager");
                    break;
                    case "saveaction":
                        processCandidate(pane, "SaveActionDetail", "ActionDetailAjaxManager");
                    break;
                    //job toolbar
                      case "openjob":
                        processCandidate(pane, "OpenSelectedJob", "JobAjaxManager");
                    break;
                    case "deletejob":
                        var confirm_msg = document.getElementById('ConfirmDeleteJob').value;
                        if(confirm(confirm_msg))
                        {
                            processCandidate(pane, "DeleteSelectedJob", "JobAjaxManager");
                        }
                    break;
                    case "vieweditjob":
                        processCandidate(pane, "ViewEditJobProfile", "JobProfileAjaxManager");
                    break;
                    case "savejob":
                        processCandidate(pane, "SaveJob", "JobProfileAjaxManager");
                    break;
                     case "previewjob":
                        if(processCandidate(pane, "PreviewJob", "JobProfileAjaxManager") == null)
                        {
                            processCandidate(pane, "PreviewJob", "JobAjaxManager");
                        }
                    break;
                    //Invoice ToolBar
                    case "openinvoice":
                        processCandidate(pane, "OpenSelectedInvoice", "InvoiceAjaxManager");
                    break;
                    case "deleteinvoice":
                        var confirm_msg = document.getElementById('ConfirmDeleteInvoice').value;
                        if(confirm(confirm_msg))
                        {
                            processCandidate(pane, "DeleteSelectedInvoice", "InvoiceAjaxManager");
                        }
                    break;
                    case "vieweditinvoice":
                        processCandidate(pane, "ViewEditInvoice", "invoiceProfileAjaxManager");
                    break;
                    case "saveinvoice":
                        processCandidate(pane, "SaveInvoice", "invoiceProfileAjaxManager");
                    break;
                    case "printinvoice":
                        if(processCandidateWidthTargetControl(pane, "PrintInvoice", "InvoiceAjaxManager", 'btnPrintSelection') == null)
                            processCandidateWidthTargetControl(pane, "PrintInvoice", "invoiceProfileAjaxManager", 'btnExport');
                    break;
                    case "emailinvoice":
                        if(processCandidate(pane, "EmailInvoice", "InvoiceAjaxManager") == null)
                            processCandidate(pane, "EmailInvoice", "invoiceProfileAjaxManager")
                    break;
                    case "copyinvoice":
                        if(processCandidate(pane, "CopyInvoice", "InvoiceAjaxManager") == null)
                            processCandidate(pane, "CopyInvoice", "invoiceProfileAjaxManager")
                    break;
                    
                 }
                 return false;
            }
            function processCandidate(pane, args, ajaxcontrol)
            {
                var iframe = pane.getExtContentElement();
                var contentWindow = iframe.contentWindow;
                var radAjaxManagerObject = contentWindow[ajaxcontrol];
                if(!radAjaxManagerObject) return null;
                radAjaxManagerObject.ajaxRequest(args);
            }
            
            function processCandidateWidthTargetControl(pane, args, ajaxcontrol, targetControl)
            {
                
                var iframe = pane.getExtContentElement();
                var contentWindow = iframe.contentWindow;
                var radAjaxManagerObject = contentWindow[ajaxcontrol];
                if(!radAjaxManagerObject) return null;
                radAjaxManagerObject.ajaxRequestWithTarget(targetControl, args);
            }
            
            

function processCandidateToolBar(msg)
{
    var toolbar = window.parent.$find("CandidateToolBar");
    if(!toolbar) return;
    switch(msg)
    {
        case "CandidateGridSelected":
            toolbar.findItemByValue("opencandidate").set_enabled(true);
            toolbar.findItemByValue("deletecandidate").set_enabled(true);
            toolbar.findItemByValue("viewcandidateactions").set_enabled(true);
            toolbar.findItemByValue("addcandidateactions").set_enabled(false);
        break;
        case "CandidateGridDeSelected":
            toolbar.findItemByValue("opencandidate").set_enabled(false);
            toolbar.findItemByValue("deletecandidate").set_enabled(false);
            toolbar.findItemByValue("viewcandidateactions").set_enabled(false);
        break;
        
        case "ViewCandidateProfile":
            toolbar.findItemByValue("vieweditcandidate").set_enabled(true);
            toolbar.findItemByValue("savecandidate").set_enabled(false);
            toolbar.findItemByValue("addcandidateactions").set_enabled(false);
            toolbar.findItemByValue("viewcandidateactions").set_enabled(true);
        break;
        
        case "AddCandidateProfile":
            toolbar.findItemByValue("vieweditcandidate").set_enabled(true);
            toolbar.findItemByValue("savecandidate").set_enabled(true);
            toolbar.findItemByValue("addcandidateactions").set_enabled(false);
            toolbar.findItemByValue("viewcandidateactions").set_enabled(false);
        break;
        
        case "EditCandidateProfile":            
            toolbar.findItemByValue("vieweditcandidate").set_enabled(true);
            toolbar.findItemByValue("savecandidate").set_enabled(true);
            toolbar.findItemByValue("addcandidateactions").set_enabled(true);
            toolbar.findItemByValue("viewcandidateactions").set_enabled(true);
        break;
        
        case "UnLoadCandidateProfilePage":
            toolbar.findItemByValue("vieweditcandidate").set_enabled(false);
            toolbar.findItemByValue("savecandidate").set_enabled(false);
            toolbar.findItemByValue("addcandidateactions").set_enabled(false);
            toolbar.findItemByValue("viewcandidateactions").set_enabled(false);
        break;
        
        
    }   
}
function processCompanyToolBar(msg)
{
    var toolbar = window.parent.$find("CompanyToolBar");
    if(!toolbar) return;
    switch(msg)
    {
        case "CompanyGridSelected":
            toolbar.findItemByValue("opencompany").set_enabled(true);
            toolbar.findItemByValue("deletecompany").set_enabled(true);
            toolbar.findItemByValue("viewcompanyactions").set_enabled(true);
            toolbar.findItemByValue("viewcompanyjobs").set_enabled(true);
            toolbar.findItemByValue("viewcompanyinvoices").set_enabled(true);
            
            toolbar.findItemByValue("addcompanyaction").set_enabled(false);            
            toolbar.findItemByValue("addcompanyjobs").set_enabled(false);            
            toolbar.findItemByValue("addcompanyinvoice").set_enabled(false);
        break;
        case "CompanyGridDeSelected":
            toolbar.findItemByValue("opencompany").set_enabled(false);
            toolbar.findItemByValue("deletecompany").set_enabled(false);
            toolbar.findItemByValue("viewcompanyactions").set_enabled(false);
            toolbar.findItemByValue("viewcompanyjobs").set_enabled(false);
            toolbar.findItemByValue("viewcompanyinvoices").set_enabled(false);
        break;
        case "EditCompanyProfile":
            toolbar.findItemByValue("vieweditcompany").set_enabled(true);
            toolbar.findItemByValue("savecompany").set_enabled(true);
            toolbar.findItemByValue("viewcompanyactions").set_enabled(true);
            toolbar.findItemByValue("addcompanyaction").set_enabled(true);
            
            toolbar.findItemByValue("viewcompanyjobs").set_enabled(true);
            toolbar.findItemByValue("addcompanyjobs").set_enabled(true);
            
            toolbar.findItemByValue("viewcompanyinvoices").set_enabled(true);
            toolbar.findItemByValue("addcompanyinvoice").set_enabled(true);
        break;
        case "ViewCompanyProfile":
            toolbar.findItemByValue("vieweditcompany").set_enabled(true);
            toolbar.findItemByValue("savecompany").set_enabled(false);
            toolbar.findItemByValue("viewcompanyactions").set_enabled(true);
            toolbar.findItemByValue("addcompanyaction").set_enabled(false);
            
            toolbar.findItemByValue("viewcompanyjobs").set_enabled(true);
            toolbar.findItemByValue("addcompanyjobs").set_enabled(false);
            
            toolbar.findItemByValue("viewcompanyinvoices").set_enabled(true);
            toolbar.findItemByValue("addcompanyinvoice").set_enabled(false);
        break;
        case "AddCompanyProfile":
            toolbar.findItemByValue("vieweditcompany").set_enabled(true);
            toolbar.findItemByValue("savecompany").set_enabled(true);
            toolbar.findItemByValue("viewcompanyactions").set_enabled(false);
            toolbar.findItemByValue("addcompanyaction").set_enabled(false);
            
            toolbar.findItemByValue("viewcompanyjobs").set_enabled(false);
            toolbar.findItemByValue("addcompanyjobs").set_enabled(false);
            
            toolbar.findItemByValue("viewcompanyinvoices").set_enabled(false);
            toolbar.findItemByValue("addcompanyinvoice").set_enabled(false);
        break;
        case "UnLoadCompanyProfilePage":
            toolbar.findItemByValue("vieweditcompany").set_enabled(false);
            toolbar.findItemByValue("savecompany").set_enabled(false);
            toolbar.findItemByValue("viewcompanyactions").set_enabled(false);            
            toolbar.findItemByValue("viewcompanyjobs").set_enabled(false);            
            toolbar.findItemByValue("viewcompanyinvoices").set_enabled(false);            
        break;
        
        
    }    
}
function processActionToolBar(msg)
{
    var toolbar = window.parent.$find("ActionToolBar");
    if(!toolbar) return;
    switch(msg)
    {
        case "ActionGridSelected":
            toolbar.findItemByValue("openaction").set_enabled(true);
            toolbar.findItemByValue("deleteaction").set_enabled(true);
        break;
        case "ActionGridDeSelected":
            toolbar.findItemByValue("openaction").set_enabled(false);
            toolbar.findItemByValue("deleteaction").set_enabled(false);
        break;
        case "ViewActionDetail":
            toolbar.findItemByValue("vieweditaction").set_enabled(true);
            toolbar.findItemByValue("saveaction").set_enabled(false);
        break;
        case "EditActionDetail":
            toolbar.findItemByValue("vieweditaction").set_enabled(true);
            toolbar.findItemByValue("saveaction").set_enabled(true);
        break;
        case "UnLoadActionDetailPage":
            toolbar.findItemByValue("vieweditaction").set_enabled(false);
            toolbar.findItemByValue("saveaction").set_enabled(false);
        break;
    }
}
function processJobToolBar(msg)
{
    var toolbar = window.parent.$find("JobToolBar");
    if(!toolbar) return;
    switch(msg)
    {
        case "JobGridSelected":
            toolbar.findItemByValue("openjob").set_enabled(true);
            toolbar.findItemByValue("deletejob").set_enabled(true);
            toolbar.findItemByValue("previewjob").set_enabled(true);
        break;
        case "JobGridDeSelected":
            toolbar.findItemByValue("openjob").set_enabled(false);
            toolbar.findItemByValue("deletejob").set_enabled(false);
            toolbar.findItemByValue("previewjob").set_enabled(false);
        break;
        case "ViewJobProfile":
            toolbar.findItemByValue("vieweditjob").set_enabled(true);
            toolbar.findItemByValue("savejob").set_enabled(false);
            toolbar.findItemByValue("previewjob").set_enabled(true);
        break;
        case "EditJobProfile":
            toolbar.findItemByValue("vieweditjob").set_enabled(true);
            toolbar.findItemByValue("savejob").set_enabled(true);
            toolbar.findItemByValue("previewjob").set_enabled(true);
        break;
        case "UnloadJobProfilePage":
            toolbar.findItemByValue("vieweditjob").set_enabled(false);
            toolbar.findItemByValue("savejob").set_enabled(false);
            toolbar.findItemByValue("previewjob").set_enabled(false);
        break;
        
    }       
}

function processInvoiceToolBar(msg)
{
    var toolbar = window.parent.$find("InvoicingToolBar");
    if(!toolbar) return;
    switch(msg)
    {
        case "InvoiceGridSelected":
            toolbar.findItemByValue("openinvoice").set_enabled(true);
            toolbar.findItemByValue("deleteinvoice").set_enabled(true);
            toolbar.findItemByValue("printinvoice").set_enabled(true);
            toolbar.findItemByValue("emailinvoice").set_enabled(true);
            toolbar.findItemByValue("copyinvoice").set_enabled(true);
        break;
        case "InvoiceGridSelectedSameReceiptionEmail":
            toolbar.findItemByValue("openinvoice").set_enabled(false);
            toolbar.findItemByValue("deleteinvoice").set_enabled(true);
            toolbar.findItemByValue("printinvoice").set_enabled(false);
            toolbar.findItemByValue("emailinvoice").set_enabled(true);
            toolbar.findItemByValue("copyinvoice").set_enabled(false);
        break;
        case "InvoiceGridMultiSelected":
            toolbar.findItemByValue("openinvoice").set_enabled(false);
            toolbar.findItemByValue("deleteinvoice").set_enabled(true);
            toolbar.findItemByValue("printinvoice").set_enabled(false);
            toolbar.findItemByValue("emailinvoice").set_enabled(false);
            toolbar.findItemByValue("copyinvoice").set_enabled(false);
        break;
        case "InvoiceGridDeSelected":
            toolbar.findItemByValue("openinvoice").set_enabled(false);
            toolbar.findItemByValue("deleteinvoice").set_enabled(false);
            toolbar.findItemByValue("printinvoice").set_enabled(false);
            toolbar.findItemByValue("copyinvoice").set_enabled(false);
        break;
        case "EditInvoiceProfile":
            toolbar.findItemByValue("vieweditinvoice").set_enabled(true);
            toolbar.findItemByValue("saveinvoice").set_enabled(true);
            toolbar.findItemByValue("printinvoice").set_enabled(false);
            toolbar.findItemByValue("emailinvoice").set_enabled(false);
            toolbar.findItemByValue("copyinvoice").set_enabled(false);
        break;
        case "ViewInvoiceProfile":
            toolbar.findItemByValue("vieweditinvoice").set_enabled(true);
            toolbar.findItemByValue("saveinvoice").set_enabled(false);
            toolbar.findItemByValue("printinvoice").set_enabled(true);
            toolbar.findItemByValue("emailinvoice").set_enabled(true);
            toolbar.findItemByValue("copyinvoice").set_enabled(true);
        break;
        case "AddInvoiceProfile":
            toolbar.findItemByValue("vieweditinvoice").set_enabled(true);
            toolbar.findItemByValue("saveinvoice").set_enabled(true);
            toolbar.findItemByValue("printinvoice").set_enabled(false);
            toolbar.findItemByValue("emailinvoice").set_enabled(false);
            toolbar.findItemByValue("copyinvoice").set_enabled(false);
        break;
        case "UnLoadInvoiceProfilePage":
            toolbar.findItemByValue("vieweditinvoice").set_enabled(false);
            toolbar.findItemByValue("saveinvoice").set_enabled(false);
            toolbar.findItemByValue("printinvoice").set_enabled(false);
            toolbar.findItemByValue("emailinvoice").set_enabled(false);
            toolbar.findItemByValue("copyinvoice").set_enabled(false);
        break;
        
     }
}

function OpenOutlookSendMail(subject, receiver, attachFileName) 
{ 
    try
    {
        var outlookApp = new ActiveXObject("Outlook.Application");
        var nameSpace = outlookApp.getNameSpace("MAPI");
        mailFolder = nameSpace.getDefaultFolder(6);
        mailItem = mailFolder.Items.add('IPM.Note.FormA');
        mailItem.Subject=subject;
        mailItem.To = receiver;
        mailItem.Attachements.Add(attachFileName);
        //mailItem.HTMLBody = "<b>bold</b>";
        mailItem.display(0);
    }
    catch(e)
    {
        alert(e);     
    } 
} 

function OpenMail()

{
    location.href = 'mailto:tranhung@vn.netika.com?attach=""C:\\Temp\\AspPDF-Test\\invoices-Test.pdf"" ';
}

function onActionSearch()
{
    
    var actionSearchUrl = "Actions.aspx?type=search";
    //radActiveActionYes
    if(document.getElementById("radActiveActionYes") != null)
    {
        if(document.getElementById("radActiveActionYes").checked)
            actionSearchUrl += "&active=Yes";
    }
    if(document.getElementById("radActiveActionNo") != null)
    {
        if(document.getElementById("radActiveActionNo").checked)
            actionSearchUrl += "&active=No";
    }
    var dateBetweenPicker = $find('datDateBetweenAction');
    var dateAndPicker = $find('datDateAndAction');
    
    if(dateBetweenPicker.get_selectedDate() != null)
    {
        var date1 = dateBetweenPicker.get_selectedDate();
        actionSearchUrl += "&dateFrom=" + date1.format('dd/MM/yyyy');
    }
    if(dateAndPicker.get_selectedDate() != null)
    {
        var date2 = dateAndPicker.get_selectedDate();
        actionSearchUrl += "&dateTo=" + date2.format('dd/MM/yyyy');
    }
    
    //&candidate=
    var candidateTextBox = $find('txtCandidateAction');    
    if(candidateTextBox.get_value() != null && candidateTextBox.get_value() != "")    
        actionSearchUrl += "&candidate=" + candidateTextBox.get_value();
    //&company=
    var companyTextBox =  $find('txtCompanyAction');
    if(companyTextBox.get_value() != null && companyTextBox.get_value() != "")
        actionSearchUrl += "&company=" + companyTextBox.get_value();
    //&typeAction=
    var actionTypeDropdown =  $find('ddlTypeAction');
    if(actionTypeDropdown.get_value() != null && actionTypeDropdown.get_value() != ""  && actionTypeDropdown.get_value() != "-1")
        actionSearchUrl += "&typeAction=" + actionTypeDropdown.get_value();
    
    //&description=
    var descriptionTextBox =  $find('txtDescriptionAction');
    if(descriptionTextBox.get_value() != null && descriptionTextBox.get_value() != "")
        actionSearchUrl += "&description=" + descriptionTextBox.get_value();
    //&responsible=
    var responsibleDropdown =  $find('ddlResponsibleAction');
    if(responsibleDropdown.get_value() != null && responsibleDropdown.get_value() != "")
        actionSearchUrl += "&responsible=" + responsibleDropdown.get_value();
        
    setRadPaneContentUrl(actionSearchUrl);
    return false;
}

function onJobSearch()
{
    var jobSearchUrl = "Jobs.aspx?mode=search";
    //&title=
    var titleTextBox = $find('txtJobTitle');
    if(titleTextBox.get_value() != null && titleTextBox.get_value() != "")
        jobSearchUrl += "&title=" + titleTextBox.get_value();
    //&active=Yes
    if(document.getElementById("radActiveJobYes") != null)
    {
        if(document.getElementById("radActiveJobYes").checked)
            jobSearchUrl += "&active=Yes";
    }
    //&active=No
    if(document.getElementById("radActiveJobNo") != null)
    {
        if(document.getElementById("radActiveJobNo").checked)
            jobSearchUrl +=  "&active=No";
    }   
    
    //&createdMin=
    var calCreatedDate1 = $find('calCreatedDate1');
    if(calCreatedDate1.get_selectedDate() != null)
    {
        var date = calCreatedDate1.get_selectedDate();
        jobSearchUrl += "&createdMin=" + date.format('dd/MM/yyyy');
    }
    //&createdMax=
    var calCreatedDate2 = $find('calCreatedDate2');
    if(calCreatedDate2.get_selectedDate() != null)
    {
        var date = calCreatedDate2.get_selectedDate();
        jobSearchUrl += "&createdMax=" + date.format('dd/MM/yyyy');
    }
    //&activatedMin=
    var calActivationDate1 = $find('calActivationDate1');
    if(calActivationDate1.get_selectedDate() != null)
    {
        var date = calActivationDate1.get_selectedDate();
        jobSearchUrl += "&activatedMin=" + date.format('dd/MM/yyyy');
    }
    //&activatedMax=
    var calActivationDate2 = $find('calActivationDate2');
    if(calActivationDate2.get_selectedDate() != null)
    {
        var date = calActivationDate2.get_selectedDate();
        jobSearchUrl += "&activatedMax=" + date.format('dd/MM/yyyy');
    }
    //&expiredMin=
    var calExpired1 = $find('calExpired1');
    if(calExpired1.get_selectedDate() != null)
    {
        var date = calExpired1.get_selectedDate();
        jobSearchUrl += "&expiredMin=" + date.format('dd/MM/yyyy');
    }
    //&expiredMax=
    var calExpired2 = $find('calExpired2');
    if(calExpired2.get_selectedDate() != null)
    {
        var date = calExpired2.get_selectedDate();
        jobSearchUrl += "&expiredMax=" + date.format('dd/MM/yyyy');
    }
    //&profile=
    var profileDropdown =  $find('ddlProfile');
    if(profileDropdown.get_value() != null && profileDropdown.get_value() != ""  && profileDropdown.get_value() != "-1")
        jobSearchUrl += "&profile=" + profileDropdown.get_value();
    //&functionFam=
    var functionDropdown =  $find('ddlFunctionFam');
    if(functionDropdown.get_value() != null && functionDropdown.get_value() != ""  && functionDropdown.get_value() != "-1")
        jobSearchUrl += "&functionFam=" + functionDropdown.get_value();
    //&location=
    var locations = getSelectedItemInListBox('lbxLocation');
    if(locations != null && locations != "")
        jobSearchUrl += "&location=" + locations;
    //&responsible=
     var responsibleDropdown =  $find('ddlResponsible');
    if(responsibleDropdown.get_value() != null && responsibleDropdown.get_value() != ""  && responsibleDropdown.get_value() != "-1")
        jobSearchUrl += "&responsible=" + responsibleDropdown.get_value();
    //&company=
    if(document.getElementById('txtCompany') != null)
    {
        if(document.getElementById('txtCompany').value != "")
            jobSearchUrl += "&company=" + document.getElementById('txtCompany').value;
    }
    setRadPaneContentUrl(jobSearchUrl);
    return false;   
}

function onInvoiceSearch()
{    
    var invoiceSearchUrl = "InvoicesPage.aspx?type=search";
    //&invoiceNumberFrom
    var fromInvoiceNumber = $find('txtInvoiceNumberFrom');
    if(fromInvoiceNumber.get_value() != null && fromInvoiceNumber.get_value() != "")
        invoiceSearchUrl += "&invoiceNumberFrom=" + fromInvoiceNumber.get_value();
    //&invoiceNumberTo=
    var toInvoiceNumber = $find('txtInvoiceNumberTo');
    if(toInvoiceNumber.get_value() != null && toInvoiceNumber.get_value() != "")
        invoiceSearchUrl += "&invoiceNumberTo=" + toInvoiceNumber.get_value();
    //&fiscalYear=
    var yearDropdown =  $find('ddlFiscalYear');
    if(yearDropdown.get_value() != null && yearDropdown.get_value() != ""  && yearDropdown.get_value() != "-1")
        invoiceSearchUrl += "&fiscalYear=" + yearDropdown.get_value();
    //&dateFrom=
    var datInvoiceDateFrom = $find('datInvoiceDateFrom');
    if(datInvoiceDateFrom.get_selectedDate() != null)
    {
        var date = datInvoiceDateFrom.get_selectedDate();
        invoiceSearchUrl += "&dateFrom=" + date.format('dd/MM/yyyy');
    }
    //&dateTo=
    var datInvoiceDateTo = $find('datInvoiceDateTo');
    if(datInvoiceDateTo.get_selectedDate() != null)
    {
        var date = datInvoiceDateTo.get_selectedDate();
        invoiceSearchUrl += "&dateTo=" + date.format('dd/MM/yyyy');
    }
    //&invoiceType= 
    var invoiceTypeDropdown =  $find('ddlInvoiceType');
    if(invoiceTypeDropdown.get_value() != null && invoiceTypeDropdown.get_value() != ""  && invoiceTypeDropdown.get_value() != "-1")
        invoiceSearchUrl += "&invoiceType=" + invoiceTypeDropdown.get_value();
    //&customer=
    var ddlCompany = $find('ddlCompany');
    if(ddlCompany != null)
    {        
        if(ddlCompany.get_value() != null && ddlCompany.get_value() != "")
        {
            invoiceSearchUrl += "&customer=" + ddlCompany.get_value();
        }       
        
    }
    /*if(document.getElementById('hiddenCompanyId') != null)
    {
        if(document.getElementById('hiddenCompanyId').value != "")
        {
             var arguments = document.getElementById('hiddenCompanyId').value.split('/');             
             if(arguments.length == 2)                
                invoiceSearchUrl += "&customer=" + arguments[0];
        }
    }*/
    
    setRadPaneContentUrl(invoiceSearchUrl);
    return false;
}
        
function getSelectedItemInListBox(listbox) 
{
    var str = "";
    var selOpts = document.getElementById(listbox);// listbox.options;
    for(var i = 0, max = selOpts.length; i < max; i++) 
    {
        if(selOpts[i].selected) 
        {
            str += selOpts[i].value + ";";
        }
    }
    return rtrim(str, ';');
}   

function trim(str, chars) 
{
    return ltrim(rtrim(str, chars), chars);
}

function ltrim(str, chars) 
{
    chars = chars || "\\s";
    return str.replace(new RegExp("^[" + chars + "]+", "g"), "");
}

function rtrim(str, chars) 
{
    chars = chars || "\\s";
    return str.replace(new RegExp("[" + chars + "]+$", "g"), "");
}

function setRadPaneContentUrl(url)
{
    var multipage=$find('RadMultiPage1');
    
    var splitter = $find('RadSplitterBrowser');
    var pane = splitter.GetPaneById('radPaneContent');
    if (!pane) return;
    pane.set_contentUrl(url);
}