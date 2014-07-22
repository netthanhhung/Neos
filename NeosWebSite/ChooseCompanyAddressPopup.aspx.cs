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
using Telerik.Web.UI;

public partial class ChooseCompanyAddressPopup : System.Web.UI.Page
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
            if (!string.IsNullOrEmpty(Request.QueryString["CompanyId"]))
            {
                int companyID = int.Parse(Request.QueryString["CompanyId"]);
                IList<CompanyAddress> comAdrList = new CompanyAddressRepository().GetAddressesOfCompany(companyID);
                gridCompanyAddress.DataSource = comAdrList;                               
            }
            else
            {
                gridCompanyAddress.DataSource = new List<CompanyAddress>();
            }
            gridCompanyAddress.DataBind();
            if (gridCompanyAddress.Items.Count > 0)
            {
                gridCompanyAddress.Items[0].Selected = true;
            }
        }
    }

    private void FillLabelLanguage()
    {
        this.Page.Title = ResourceManager.GetString("chooseCompanyAddressPopupTitle");
        
        btnOK.Text = ResourceManager.GetString("okText");
        btnCancel.Text = ResourceManager.GetString("cancelText");        
    }
    
    protected void OnBtnOkClicked(object sender, EventArgs e)
    {
        if (gridCompanyAddress.SelectedValue != null)
        {
            int addressID = (int)gridCompanyAddress.SelectedValue;
            CompanyAddress address = new CompanyAddressRepository().FindOne(new CompanyAddress(addressID));
            string argument = address.AddressID.ToString();
            if (address.Name != null)
                argument += "/" + address.Name;
            else
                argument += "/ ";

            if (address.Address != null)
                argument += "/" + address.Address;
            else
                argument += "/ ";

            if (address.City != null)
                argument += "/" + address.City;
            else
                argument += "/ "; 
            
            if (address.VatNumber != null)
                argument += "/" + address.VatNumber;
            else
                argument += "/ ";
                
            string script = "<script type=\"text/javascript\">";
            script += " OnBtnOkClientClicked(\"" + argument + "\");";
            script += " </script>";

            if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script);
        }
    }   
}
