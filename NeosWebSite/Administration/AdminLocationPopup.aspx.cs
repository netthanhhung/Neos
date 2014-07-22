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
using Neos.Data;
using System.Collections.Generic;

public partial class AdminLocationPopup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionManager.CurrentUser == null)
        {
            Common.RedirectToLoginPage(this);
            return;
        }
        else if (!IsPostBack)
        {
            FillLabelLanguage();
            if (!string.IsNullOrEmpty(Request.QueryString["Location"]))
            {
                string locationID = Request.QueryString["Location"];
                ParamLocations location = new ParamLocationsRepository().GetLocation(locationID);
                txtLocation.Text = location.Location;
                if(location.Hierarchie.HasValue) 
                    txtHierarchy.Text = location.Hierarchie.Value.ToString();
                txtLocationUk.Text = location.LocationUk;
                txtLocationNl.Text = location.LocationNL;
                txtCode.Text = location.CodeLocation;
            }
        }
    }

    private void FillLabelLanguage()
    {
        rfvAdminLocation1.ErrorMessage = ResourceManager.GetString("messageLocationMustNotBeEmpty");
        rfvAdminLocation2.ErrorMessage = ResourceManager.GetString("messageLocationCodeMustNotBeEmpty");
    }

    protected void OnBtnSaveClicked(object sender, EventArgs e)
    {
        ParamLocationsRepository repo = new ParamLocationsRepository();

        ParamLocations saveItem = new ParamLocations();
        saveItem.Location = txtLocation.Text.Trim();
        if (!string.IsNullOrEmpty(txtHierarchy.Text))
            saveItem.Hierarchie = Int32.Parse(txtHierarchy.Text);
        else
            saveItem.Hierarchie = null;
        saveItem.LocationUk = txtLocationUk.Text.Trim();
        saveItem.LocationNL = txtLocationNl.Text.Trim();
        saveItem.CodeLocation = txtCode.Text.Trim();

        string location = Request.QueryString["Location"];
        if (string.IsNullOrEmpty(location))
        {
            //Insert new record
            ParamLocations oldItem = repo.GetLocation(txtLocation.Text.Trim());
            ParamLocations oldItem1 = repo.GetLocationByCode(txtCode.Text.Trim());

            if (oldItem == null && oldItem1 == null)
                repo.InserNewLocation(saveItem);
            else
            {
                string message = ResourceManager.GetString("itemAlreadyExist");
                string script1 = "<script type=\"text/javascript\">";
                script1 += " alert(\"" + message + "\");";
                script1 += " </script>";

                if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                    ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script1);
            }
        }
        else
        {
            location = location.Trim();
            string message = string.Empty;    
            ParamLocations oldLocation = repo.GetLocation(location);
            if (saveItem.Location.Trim() == location && saveItem.CodeLocation.Trim() == oldLocation.CodeLocation.Trim())
            {
                //Update the record.            
                repo.Update(saveItem);
            }
            else if(saveItem.Location.Trim() != location) 
            {
                if (oldLocation.NumberIDUsed > 0)
                    message = ResourceManager.GetString("messageLocationBeingUsed");                                         
            }
            else
            {
                if (oldLocation.NumberCodeUsed > 0)
                    message = ResourceManager.GetString("messageLocationCodeBeingUsed");
            }
            if (string.IsNullOrEmpty(message))
            {
                if (saveItem.Location.Trim() == location)
                    repo.Update(saveItem);
                else
                {
                    repo.Delete(new ParamLocations(location));
                    repo.InserNewLocation(saveItem);
                }
            }
            else
            {
                string scriptMes = "<script type=\"text/javascript\">";
                scriptMes += " alert(\"" + message + "\")";
                scriptMes += " </script>";

                if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                    ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", scriptMes);
                return;
            }
        }
        
        string script = "<script type=\"text/javascript\">";
        script += " OnBtnSaveClientClicked();";
        script += " </script>";

        if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
            ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);
    }
}
