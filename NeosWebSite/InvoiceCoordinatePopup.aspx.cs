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
using Telerik.Web.UI;
using System.Collections.Generic;

public partial class InvoiceCoordinatePopup : System.Web.UI.Page
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
            BindData();
        }
    }



    private void FillLabelLanguage()
    {
        this.Page.Title = ResourceManager.GetString("lblCompanyProfileInvoiceCoordinates");
        revInvCoordinate.ErrorMessage = ResourceManager.GetString("messageUserEmailNotValid");

        lblNbrCustomer.Text = ResourceManager.GetString("lblNbrCustomer");
        lblName.Text = ResourceManager.GetString("lblInvoiceAddressName");
        lblCo.Text = ResourceManager.GetString("lblInvCoorCO");
        lblAddress.Text = ResourceManager.GetString("lblInvoiceAddress");
        lblZipCode.Text = ResourceManager.GetString("lblInvoiceZipCode");
        lblCity.Text = ResourceManager.GetString("lblInvoiceCity");
        lblVatNumber.Text = ResourceManager.GetString("lblInvoiceVATNumber");
        lblTelephone.Text = ResourceManager.GetString("lblPhone");
        lblFax.Text = ResourceManager.GetString("lblFax");
        lblEmail.Text = ResourceManager.GetString("lblEmail");
        lblDefault.Text = ResourceManager.GetString("lblCoordinateDefault");

        btnSave.Text = ResourceManager.GetString("saveText");
        btnCancel.Text = ResourceManager.GetString("cancelText");  
    }

    private void BindData()
    {
        if (!string.IsNullOrEmpty(Request.QueryString["AddressId"]))
        {
            int addressID = int.Parse(Request.QueryString["AddressId"]);
            CompanyAddress address = new CompanyAddressRepository().FindOne(new CompanyAddress(addressID));
            txtNbrCustomer.Text = address.CompanyID.ToString();
            txtName.Text = address.Name;
            txtCO.Text = address.Co;
            txtAddress.Text = address.Address;
            txtCity.Text = address.City;
            txtZipCode.Text = address.ZipCode;
            txtVatNumber.Text = address.VatNumber;
            txtTelephone.Text = address.Telephone;
            txtFax.Text = address.Fax;
            txtEmail.Text = address.Email;
            chkDefault.Checked = address.IsDefault;
            txtFactoringCode.Text = address.FactoringCode;
            //if (!string.IsNullOrEmpty(address.FactoringCode))
            //{
            //    Company comp = new CompanyRepository().FindOne(new Company(address.FactoringCode.Value));
            //    ddlFactoringCode.Items.Clear();
            //    ddlFactoringCode.Items.Add(new RadComboBoxItem(comp.CompanyName, comp.CompanyID.ToString()));
            //    ddlFactoringCode.SelectedIndex = 0;

            //    hiddenCompanyId.Value = comp.CompanyID.ToString();
            //    txtFactoringCode.Text = comp.CompanyName;
            //}
        }
        else
        {
            txtNbrCustomer.Text = SessionManager.CurrentCompany.CompanyID.ToString();
        }
    }

    protected void OnBtnSaveClicked(object sender, EventArgs e)
    {
        CompanyAddressRepository repo = new CompanyAddressRepository();
        
        //Check default
        if (chkDefault.Checked)
        {
            IList<CompanyAddress> addressOfCom = repo.GetAddressesOfCompany(SessionManager.CurrentCompany.CompanyID);
            bool isDefault = false;
            foreach (CompanyAddress item in addressOfCom)
            {
                if (item.IsDefault)
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["AddressId"]))
                    {
                        if (item.AddressID != int.Parse(Request.QueryString["AddressId"]))
                        {
                            isDefault = true;
                            break;
                        }
                    }
                    else
                    {
                        isDefault = true;
                        break;
                    }
                }
            }
            if (isDefault)
            {
                string message = ResourceManager.GetString("messageAlreadyHasDefaultAddress");
                string script1 = "<script type=\"text/javascript\">";
                script1 += " alert(\"" + message + "\")";
                script1 += " </script>";

                if (!ClientScript.IsClientScriptBlockRegistered("redirectUser"))
                    ClientScript.RegisterStartupScript(this.GetType(), "redirectUser", script1);
                return;
            }
        }
        
        CompanyAddress address = GetAddress();
        if (!string.IsNullOrEmpty(Request.QueryString["AddressId"]))
        {            
            repo.Update(address);
        }
        else 
        {
            repo.Insert(address);
        }

        string script = "<script type=\"text/javascript\">";
        script += " OnBtnSaveClientClicked();";
        script += " </script>";

        if (!ClientScript.IsClientScriptBlockRegistered("saveAndCloseWindow"))
            ClientScript.RegisterStartupScript(this.GetType(), "saveAndCloseWindow", script);
    }

    private CompanyAddress GetAddress()
    {
        if (SessionManager.CurrentCompany == null)
            {
                Common.RedirectToLoginPage(this);
                return null;
        }
        CompanyAddress address = new CompanyAddress();
        if (!string.IsNullOrEmpty(Request.QueryString["AddressId"]))
        {
            address.AddressID = int.Parse(Request.QueryString["AddressId"]);
        }
        
        address.CompanyID = SessionManager.CurrentCompany.CompanyID; 
        address.Name = txtName.Text.Trim();
        address.Co = txtCO.Text.Trim();
        address.Address = txtAddress.Text.Trim();
        address.City = txtCity.Text.Trim();
        address.ZipCode = txtZipCode.Text.Trim();
        address.VatNumber = txtVatNumber.Text.Trim();
        address.Telephone = txtTelephone.Text.Trim();
        address.Fax = txtFax.Text.Trim();
        address.Email = txtEmail.Text.Trim();
        
        address.IsDefault = chkDefault.Checked;
        address.FactoringCode = txtFactoringCode.Text;
        //if (!string.IsNullOrEmpty(ddlFactoringCode.SelectedValue))
        //{
        //    address.FactoringCode = int.Parse(ddlFactoringCode.SelectedValue);            
        //}
        return address;
    }

    //protected void OnDropdownCompany_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    //{
    //    string companyName = e.Text;
    //    if (!string.IsNullOrEmpty(companyName))
    //    {
    //        List<Company> list = new CompanyRepository().FindByName(companyName);

    //        ddlFactoringCode.DataSource = list;
    //        ddlFactoringCode.DataBind();
    //    }
    //}
}
