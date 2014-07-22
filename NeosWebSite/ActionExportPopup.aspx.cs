using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Outlook;
using Neos.Data;

public partial class ActionExportPopup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void OnBtnAddClicked(object sender, EventArgs e)
    {
        string emailExp = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        string email = txtEmail.Text.Trim();
        if (!string.IsNullOrEmpty(email) && Regex.IsMatch(email, emailExp))
        {
            bool isHave = false;   
            foreach (ListItem item in listEmail.Items)
            {
                if (item.Value == email)
                {
                    isHave = true;
                }
            }
            if (!isHave)
            {
                listEmail.Items.Add(new ListItem(email, email));
            }
        }
    }

    protected void OnBtnExportClicked(object sender, EventArgs e)
    {
        if (listEmail.Items.Count > 0 && !string.IsNullOrEmpty(Request.QueryString["ActionID"]))
        {
            try
            {
                int actionID = int.Parse(Request.QueryString["ActionID"]);
                ActionRepository repo = new ActionRepository();
                Neos.Data.Action action = repo.GetActionByActionID(actionID);
                if (action == null) return;
                //First thing you need to do is add a reference to Microsoft Outlook 11.0 Object Library. Then, create new instance of Outlook.Application object:             
                Application outlookApp = new Application();

                //Next, create an instance of AppointmentItem object and set the properties: 
                AppointmentItem oAppointment = (AppointmentItem)outlookApp.CreateItem(OlItemType.olAppointmentItem);

                oAppointment.Subject = "This is the subject for my appointment";
                oAppointment.Body = action.DescrAction;
                oAppointment.Location = action.LieuRDV;
                
                // Set the start date
                //if(action.DateAction.HasValue) 
                //    oAppointment.Start = action.DateAction.Value;
                // End date 
                if (action.Hour.HasValue)
                {
                    oAppointment.Start = action.Hour.Value;
                    DateTime end = oAppointment.Start.AddHours(1);
                    oAppointment.End = end;
                }
                // Set the reminder 15 minutes before start
                oAppointment.ReminderSet = true;
                oAppointment.ReminderMinutesBeforeStart = 15;

                //Setting the sound file for a reminder: 
                oAppointment.ReminderPlaySound = true;
                //set ReminderSoundFile to a filename. 

                //Setting the importance: 
                //use OlImportance enum to set the importance to low, medium or high
                oAppointment.Importance = OlImportance.olImportanceHigh;

                /* OlBusyStatus is enum with following values:
                olBusy
                olFree
                olOutOfOffice
                olTentative
                */
                oAppointment.BusyStatus = OlBusyStatus.olBusy;

                //Finally, save the appointment: 
                // Save the appointment
                //oAppointment.Save();

                // When you call the Save () method, the appointment is saved in Outlook. 
               
                string recipientsMail = string.Empty;
                foreach (ListItem item in listEmail.Items)
                {
                    if (recipientsMail == string.Empty)
                    {
                        recipientsMail += item.Value;
                    }
                    else
                    {
                        recipientsMail += "; " + item.Value;
                    }
                }
                
                //mailItem.SenderEmailAddress = "tranhung@vn.netika.com";
                //mailItem.SenderName = "Thanh Hung";
                oAppointment.RequiredAttendees = recipientsMail;

                // Another useful method is ForwardAsVcal () which can be used to send the Vcs file via email. 
                MailItem mailItem = oAppointment.ForwardAsVcal();
                mailItem.To = "tranhung@vn.netika.com";
                mailItem.Send();
                //oAppointment.Send();
            }
            catch (System.Exception ex)
            {
                string script1 = "<script type=\"text/javascript\">";
                
                script1 += " alert(\"" + ex.Message + "\")";
                script1 += " </script>";

                if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                    ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script1);
                return;
            }
        }
        string script = "<script type=\"text/javascript\">";
        script += " OnBtnExportClientClicked();";
        script += " </script>";

        if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
            ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);   
    }
}
