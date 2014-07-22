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
using System.Collections.Generic;
using Neos.Data;
using Telerik.Web.UI;

public partial class Companies : System.Web.UI.Page
{
    PageStatePersister _pers;

    protected override PageStatePersister PageStatePersister
    {
        get
        {
            if (_pers == null)
            {
                _pers = new SessionPageStatePersister(this);
            }
            return _pers;
        }
    }
    public int pageSize
    {
        get { return CompanyGrid.PageSize; }
        set { CompanyGrid.PageSize = value; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionManager.CurrentUser == null)
        {
            Common.RedirectToLoginPage(this);
            return;
        }
        else if (!IsPostBack)
        {
            InitControls();
            BindData();
            SessionManager.BackUrl = Request.Url.ToString();
        }
    }

    private void InitControls()
    {
        HttpCookie companyGridPageSizeCookie = Request.Cookies.Get("comgrdps");
        if (companyGridPageSizeCookie != null)
        {
            if (!string.IsNullOrEmpty(companyGridPageSizeCookie.Value))
                CompanyGrid.PageSize = Convert.ToInt32(companyGridPageSizeCookie.Value) > 0 ? Convert.ToInt32(companyGridPageSizeCookie.Value) : CompanyGrid.PageSize;
        }

    }



    private void BindData()
    {
        FillLabelLanguage();
        BindGridData();
    }

    private void FillLabelLanguage()
    {
        lblCompanyTitle.Text = ResourceManager.GetString("lblRightPaneCompanyTitle");
        //lblCompany.Text = ResourceManager.GetString("lblCompanyPresent");
        lblContacts.Text = ResourceManager.GetString("lblContactsPresent");
        lblContactInfo.Text = ResourceManager.GetString("columnContactInfoCandidateGrid");
        btnAddContact.Text = ResourceManager.GetString("lblAdd");
        btnAddContact.OnClientClick = string.Format("return onAddContactClientClick(\"{0}\");", ResourceManager.GetString("msgSelectACompany"));

        btnAddContactInfo.Text = ResourceManager.GetString("lblAdd");
        btnAddContactInfo.OnClientClick = string.Format("return onAddContactInfoClientClick(\"{0}\");", ResourceManager.GetString("msgSelectAContact"));

        CompanyGrid.Columns[1].HeaderText = ResourceManager.GetString("lblCompanyName");
        CompanyGrid.Columns[2].HeaderText = ResourceManager.GetString("lblCity");
        CompanyGrid.Columns[3].HeaderText = ResourceManager.GetString("lblType");
        CompanyGrid.Columns[4].HeaderText = ResourceManager.GetString("lblContactInfo");
        CompanyGrid.Columns[5].HeaderText = ResourceManager.GetString("lblNeosResp");
        CompanyGrid.Columns[6].HeaderText = ResourceManager.GetString("lblJob");
        CompanyGrid.Columns[7].HeaderText = ResourceManager.GetString("columnCreatedDateAttachedDocPresent");
        CompanyGrid.PagerStyle.PagerTextFormat = ResourceManager.GetString("gridCompanyPagerTextFormat");
        lnkAddCompany.Text = ResourceManager.GetString("lblAddNewCompany");

        CompanyContactGrid.Columns[1].HeaderText = ResourceManager.GetString("columnLastNameCandidateGrid");
        CompanyContactGrid.Columns[2].HeaderText = ResourceManager.GetString("columnFirstNameCandidateGrid");
        CompanyContactGrid.Columns[3].HeaderText = ResourceManager.GetString("lblCanFunction");

        ContactInfoGrid.Columns[0].HeaderText = ResourceManager.GetString("columnTypeActionCan");
        ContactInfoGrid.Columns[1].HeaderText = ResourceManager.GetString("lblPhoneArea");
        ContactInfoGrid.Columns[2].HeaderText = ResourceManager.GetString("lblInfo");
        ContactInfoGrid.Columns[3].HeaderText = ResourceManager.GetString("columnPlaceCandidateContact");

    }

    private void BindGridData()
    {
        GetCompanyGridDataSource(null);
        CompanyGrid.DataBind();

        //init contact grid
        CompanyContactGrid.DataSource = new ArrayList();
        CompanyContactGrid.DataBind();
        ContactInfoGrid.DataSource = new ArrayList();
        ContactInfoGrid.DataBind();
        BindContactInfoType();

    }

    private void BindContactInfoType()
    {
        ddlType.Items.Add(new RadComboBoxItem(ResourceManager.GetString("candidateContactPhone"), "T"));
        ddlType.Items.Add(new RadComboBoxItem(ResourceManager.GetString("candidateContactFax"), "F"));
        ddlType.Items.Add(new RadComboBoxItem(ResourceManager.GetString("candidateContactMobile"), "G"));
        ddlType.Items.Add(new RadComboBoxItem(ResourceManager.GetString("candidateContactEmail"), "E"));
    }

    private void BindFunctionData()
    {
        ddlFunction.DataTextField = "FunctionName";
        ddlFunction.DataValueField = "ContactFunctionID";
        ddlFunction.DataSource = new ParamContactFunctionRepository().FindAll();
        ddlFunction.DataBind();
    }

    protected void OnDropdownFunctionItemRequested(object o, RadComboBoxItemsRequestedEventArgs e)
    {
        BindFunctionData();
    }

    private void GetCompanyGridDataSource(GridSortCommandEventArgs sortEventArgs)
    {
        
        int pageNumber = CompanyGrid.CurrentPageIndex + 1;
        string sortExpress = string.Empty;
        string sortExpressInvert = string.Empty;
        foreach (GridSortExpression item in CompanyGrid.MasterTableView.SortExpressions)
        {
            GridSortOrder newSortOrder = item.SortOrder;
            if (sortEventArgs != null && item.FieldName == sortEventArgs.SortExpression)
            {
                newSortOrder = sortEventArgs.NewSortOrder;
            }

            if (!string.IsNullOrEmpty(sortExpress) && newSortOrder != GridSortOrder.None)
            {
                sortExpress += ", ";
                sortExpressInvert += ", ";
            }
            if (newSortOrder == GridSortOrder.Ascending)
            {
                sortExpress += item.FieldName + " ASC";
                sortExpressInvert += item.FieldName + " DESC";
            }
            else if (newSortOrder == GridSortOrder.Descending)
            {
                sortExpress += item.FieldName + " DESC";
                sortExpressInvert += item.FieldName + " ASC";
            }
        }

        if (sortEventArgs != null && !sortExpress.Contains(sortEventArgs.SortExpression))
        {
            if (!string.IsNullOrEmpty(sortExpress) && sortEventArgs.NewSortOrder != GridSortOrder.None)
            {
                sortExpress += ", ";
                sortExpressInvert += ", ";
            }
            if (sortEventArgs.NewSortOrder == GridSortOrder.Ascending)
            {
                sortExpress += sortEventArgs.SortExpression + " ASC";
                sortExpressInvert += sortEventArgs.SortExpression + " DESC";
            }
            else if (sortEventArgs.NewSortOrder == GridSortOrder.Descending)
            {
                sortExpress += sortEventArgs.SortExpression + " DESC";
                sortExpressInvert += sortEventArgs.SortExpression + " ASC";
            }
        }

        if (!string.IsNullOrEmpty(sortExpress))
        {
            if (sortExpress.Contains("CompanyName"))
            {
                sortExpress = sortExpress.Replace("CompanyName", "SocNom");
                sortExpressInvert = sortExpressInvert.Replace("CompanyName", "SocNom");
            }
            if (sortExpress.Contains("City"))
            {
                sortExpress = sortExpress.Replace("City", "Commune");
                sortExpressInvert = sortExpressInvert.Replace("City", "Commune");
            }
            if (sortExpress.Contains("StatusLabel"))
            {
                sortExpress = sortExpress.Replace("StatusLabel", "StatusLabel");
                sortExpressInvert = sortExpressInvert.Replace("StatusLabel", "StatusLabel");
            }
            if (sortExpress.Contains("Responsible"))
            {
                sortExpress = sortExpress.Replace("Responsible", "Responsable");
                sortExpressInvert = sortExpressInvert.Replace("Responsible", "Responsable");
            }
            if (sortExpress.Contains("CreatedDate"))
            {
                sortExpress = sortExpress.Replace("CreatedDate", "DateCreation");
                sortExpressInvert = sortExpressInvert.Replace("CreatedDate", "DateCreation");
            }            
        }
        else
        {
            sortExpress = "SocNom ASC";
            sortExpressInvert = "SocNom DESC";
        }


        CompanyRepository companyRepo = new CompanyRepository();
        List<Company> companyList = new List<Company>();
        /*if (!string.IsNullOrEmpty(Request.QueryString["type"])) //search by type
        {
            switch (Request.QueryString["type"])
            {
                case "all":
                    CompanyGrid.VirtualItemCount = companyRepo.CountAllCompanies();
                    companyList = companyRepo.GetAllCompanies(pageSize, pageNumber, sortExpress, sortExpressInvert);
                    break;
                case "client":
                    CompanyGrid.VirtualItemCount = companyRepo.CountCustomerCompanies();
                    companyList = companyRepo.GetCustomerCompanies(pageSize, pageNumber, sortExpress, sortExpressInvert);
                    break;
                case "prospect":
                    CompanyGrid.VirtualItemCount = companyRepo.CountPropectCompanies();
                    companyList = companyRepo.GetPropectCompanies(pageSize, pageNumber, sortExpress, sortExpressInvert);
                    break;
                case "inactive":
                    CompanyGrid.VirtualItemCount = companyRepo.CountInactiveCompanies();
                    companyList = companyRepo.GetInactiveCompanies(pageSize, pageNumber, sortExpress, sortExpressInvert);
                    break;
                default:
                    CompanyGrid.VirtualItemCount = companyRepo.CountAllCompanies();
                    companyList = companyRepo.GetAllCompanies(pageSize, pageNumber, sortExpress, sortExpressInvert);
                    break;
            }
        }
        else*/

        //if ((!string.IsNullOrEmpty(Request.QueryString["cname"]))) // search by name
        //{
            if (!string.IsNullOrEmpty(Request.QueryString["ctype"]))
            {
                switch(Request.QueryString["ctype"])
                {
                    case "all":
                        CompanyGrid.VirtualItemCount = companyRepo.CountSearchCompaniesByName(Request.QueryString["cname"]);
                        companyList = companyRepo.FindByName(Request.QueryString["cname"], pageSize, pageNumber, sortExpress, sortExpressInvert);
                        break;
                    case "client":
                        CompanyGrid.VirtualItemCount = companyRepo.CountSearchCompaniesByNameAndType(Request.QueryString["cname"], "1");
                        companyList = companyRepo.FindByNameAndType(Request.QueryString["cname"],"1", pageSize, pageNumber, sortExpress, sortExpressInvert);
                        break;
                    case "prospect":
                        CompanyGrid.VirtualItemCount = companyRepo.CountSearchCompaniesByNameAndType(Request.QueryString["cname"], "2");
                        companyList = companyRepo.FindByNameAndType(Request.QueryString["cname"], "2", pageSize, pageNumber, sortExpress, sortExpressInvert);
                        break;
                    case "inactive":
                        CompanyGrid.VirtualItemCount = companyRepo.CountSearchCompaniesByNameAndType(Request.QueryString["cname"], "0");
                        companyList = companyRepo.FindByNameAndType(Request.QueryString["cname"], "0", pageSize, pageNumber, sortExpress, sortExpressInvert);
                        break;
                }                
            }
            else
            {
                //CompanyGrid.VirtualItemCount = companyRepo.CountSearchCompaniesByName(Request.QueryString["cname"]);
                //companyList = companyRepo.FindByName(Request.QueryString["cname"], pageSize, pageNumber, sortExpress, sortExpressInvert);
                companyList = companyRepo.GetTopCompany(pageSize);
            }
        //}
        //else //view a number of recent company
        //{
        //    companyList = companyRepo.GetTopCompany(pageSize);
        //}
        CompanyGrid.DataSource = companyList;
    }

    #region Company Grid events
    protected void OnCompanyGrid_PageSizeChanged(object source, GridPageSizeChangedEventArgs e)
    {
        HttpCookie companyGridPageSizeCookie = new HttpCookie("comgrdps");
        companyGridPageSizeCookie.Expires.AddDays(30);
        companyGridPageSizeCookie.Value = e.NewPageSize.ToString();
        Response.Cookies.Add(companyGridPageSizeCookie);
    }

    protected void OnCompanyGridItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem || e.Item.ItemType == Telerik.Web.UI.GridItemType.Item)
        {
            Company com = (Company)e.Item.DataItem;
            if (com != null)
            {
                /* Literal lblCompanyStatus = (Literal)e.Item.FindControl("lblCompanyStatus");
                 if (lblCompanyStatus != null)
                 {
                     lblCompanyStatus.Text = (status != null) ? status.Status : string.Empty;
                 }*/
                RadComboBox ddlContactInfo = (RadComboBox)e.Item.FindControl("ddlContactInfo");
                if (ddlContactInfo != null)
                {

                    ddlContactInfo.ItemDataBound += new RadComboBoxItemEventHandler(OnDropDownContactInfo_ItemDataBound);

                    List<string> contactInfoList = new List<string>();
                    if (!string.IsNullOrEmpty(com.Telephone))
                        contactInfoList.Add(ResourceManager.GetString("lblTelephone") + ": " + com.Telephone);
                    if (!string.IsNullOrEmpty(com.Fax))
                        contactInfoList.Add(ResourceManager.GetString("lblFax") + ": " + com.Fax);
                    if (!string.IsNullOrEmpty(com.Email))
                        contactInfoList.Add(ResourceManager.GetString("columnEmailContactsPresent") + ": " + com.Email);

                    ddlContactInfo.DataSource = contactInfoList;
                    ddlContactInfo.DataBind();

                    /*if (!string.IsNullOrEmpty(com.Telephone))
                    {
                        RadComboBoxItem telephoneItem = new RadComboBoxItem(ResourceManager.GetString("lblTelephone") + ": " + com.Telephone, "tel");
                        ddlContactInfo.Items.Add(telephoneItem);
                    }
                    if (!string.IsNullOrEmpty(com.Fax))
                    {
                        RadComboBoxItem faxItem = new RadComboBoxItem(ResourceManager.GetString("lblFax") + ": " + com.Fax, "fax");
                        ddlContactInfo.Items.Add(faxItem);
                    }
                    if (!string.IsNullOrEmpty(com.Email))
                    {
                        RadComboBoxItem emailItem = new RadComboBoxItem(ResourceManager.GetString("columnEmailContactsPresent") + ": " + com.Email, "email");
                        ddlContactInfo.Items.Add(emailItem);
                    }*/
                }

                Literal lblJobCount = (Literal)e.Item.FindControl("lblJobCount");
                if (lblJobCount != null)
                {
                    lblJobCount.Text = new JobRepository().GetJobsOfCompany(com.CompanyID).Count.ToString();
                }


                Literal lblCreatedDate = (Literal)e.Item.FindControl("lblCreatedDate");
                if (lblCreatedDate != null)
                {
                    lblCreatedDate.Text = com.CreatedDate.HasValue ? com.CreatedDate.Value.ToString("dd/MM/yyyy") : "n/a";
                }
            }
        }
    }

    protected void OnDropDownContactInfo_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        Literal lblContactInfo = (Literal)e.Item.FindControl("lblContactInfo");
        if (lblContactInfo != null)
        {
            lblContactInfo.Text += e.Item.DataItem as string;
        }
    }

    protected void OnCompanyGridNeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        GetCompanyGridDataSource(null);
    }

    protected void OnAddNewCompanyClick(object sender, EventArgs e)
    {
        SessionManager.NewCompanyContactList = new List<CompanyContact>();
        Response.Redirect("CompanyProfile.aspx?mode=edit&backurl=visible", false);
    }

    protected void OnCompanyGridPageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        CompanyGrid.CurrentPageIndex = e.NewPageIndex;
        BindGridData();

    }

    protected void OnCompanyGridSortCommand(object source, GridSortCommandEventArgs e)
    {
        GetCompanyGridDataSource(e);
    }
    #endregion

    #region contact grid events
    protected void OnCompanyContactGridNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        if (CompanyGrid.SelectedItems.Count == 1)
        {
            GridDataItem dataItem = ((GridDataItem)CompanyGrid.SelectedItems[0]);
            if (dataItem != null)
            {
                int companyID = Int32.Parse(dataItem["CompanyID"].Text);
                List<CompanyContact> contactList = new CompanyContactRepository().GetContactOfCompany(companyID);
                CompanyContactGrid.DataSource = contactList;
            }
        }
        else
        {
            CompanyContactGrid.DataSource = null;
        }
    }

    #endregion

    #region contact info grid events
    protected void OnContactInfoGridNeedDataSource(object source, GridNeedDataSourceEventArgs e)
    {
        if (CompanyContactGrid.SelectedItems.Count == 1)
        {
            GridDataItem dataItem = ((GridDataItem)CompanyContactGrid.SelectedItems[0]);
            if (dataItem != null)
            {
                int contactID = Int32.Parse(dataItem["ContactID"].Text);
                List<CompanyContactTelephone> contactInfoList = new CompanyContactTelephoneRepository().GetContactInfo(contactID);
                ContactInfoGrid.DataSource = contactInfoList;
            }
        }
        else
        {
            ContactInfoGrid.DataSource = null;
        }
    }

    protected void OnContactInfoGridItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            CompanyContactTelephone contactInfo = e.Item.DataItem as CompanyContactTelephone;
            if (contactInfo != null)
            {
                TableCell type = ((GridDataItem)e.Item)["Type"] as TableCell;
                if (type != null)
                {
                    if (!string.IsNullOrEmpty(contactInfo.Type))
                    {
                        switch (contactInfo.Type.ToUpper())
                        {
                            case "T":
                                type.Text = ResourceManager.GetString("candidateContactPhone");
                                break;
                            case "F":
                                type.Text = ResourceManager.GetString("candidateContactFax");
                                break;
                            case "G":
                                type.Text = ResourceManager.GetString("candidateContactMobile");
                                break;
                            case "E":
                                type.Text = ResourceManager.GetString("candidateContactEmail");
                                break;
                        }
                    }
                }
            }
        }
    }
    #endregion


    #region ajax manager events
    protected void OnMyAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("RebindContactGrid") != -1)
        {
            //System.Threading.Thread.Sleep(1000);
            //divContact.Visible = true;
            //divContact.Style.Add("display", "block");
            CompanyAjaxManager.AjaxSettings.AddAjaxSetting(CompanyAjaxManager, CompanyContactGrid);
            CompanyAjaxManager.AjaxSettings.AddAjaxSetting(CompanyAjaxManager, pnlAddContact);
            string[] args = e.Argument.Split('/');
            if (args.Length == 3)
            {
                try
                {
                    int companyID = Int32.Parse(args[1]);
                    int rowIndex = Int32.Parse(args[2]);
                    CompanyGrid.MasterTableView.ClearSelectedItems();
                    CompanyGrid.MasterTableView.Items[rowIndex - 1].Selected = true;
                }
                catch (Exception ex) { throw ex; }
                CompanyContactGrid.Rebind();
            }
        }
        else if (e.Argument.IndexOf("RebindContactInfoGrid") != -1)
        {
            //divContactInfo.Style.Add("display","block");
            CompanyAjaxManager.AjaxSettings.AddAjaxSetting(CompanyAjaxManager, ContactInfoGrid);
            CompanyAjaxManager.AjaxSettings.AddAjaxSetting(CompanyAjaxManager, pnlAddContactInfo);
            string[] args = e.Argument.Split('/');
            if (args.Length == 3)
            {
                try
                {
                    int contactID = Int32.Parse(args[1]);
                    int rowIndex = Int32.Parse(args[2]);
                    CompanyContactGrid.MasterTableView.ClearSelectedItems();
                    CompanyContactGrid.MasterTableView.Items[rowIndex - 1].Selected = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                ContactInfoGrid.Rebind();
            }

        }
        else if (e.Argument.IndexOf("AddCompanyContact") != -1)
        {
            CompanyAjaxManager.AjaxSettings.AddAjaxSetting(CompanyAjaxManager, CompanyContactGrid);
            string[] args = e.Argument.Split('/');
            if (args.Length == 2)
            {
                try
                {
                    int companyID = Int32.Parse(args[1]);
                    CompanyContactRepository contactRepo = new CompanyContactRepository();
                    CompanyContact contact = new CompanyContact();
                    contact.CompanyID = companyID;
                    contact.LastName = txtLastName.Text.Trim();
                    contact.FirstName = txtFirstName.Text.Trim();
                    contact.Position = ddlFunction.Text;

                    contactRepo.Insert(contact);

                    CompanyContactGrid.Rebind();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        else if (e.Argument.IndexOf("AddContactInfo") != -1)
        {
            CompanyAjaxManager.AjaxSettings.AddAjaxSetting(CompanyAjaxManager, ContactInfoGrid);
            string[] args = e.Argument.Split('/');
            if (args.Length == 2)
            {
                try
                {
                    int contactID = Int32.Parse(args[1]);
                    CompanyContactTelephoneRepository contactInfoRepo = new CompanyContactTelephoneRepository();
                    CompanyContactTelephone contactInfo = new CompanyContactTelephone();
                    contactInfo.ContactID = contactID;
                    contactInfo.Type = ddlType.SelectedValue;
                    contactInfo.TelephoneZone = txtPhoneZone.Text.Trim();
                    contactInfo.Tel = txtInfo.Text.Trim();
                    contactInfo.Location = txtPlace.Text.Trim();

                    contactInfoRepo.Insert(contactInfo);

                    ContactInfoGrid.Rebind();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        else if (e.Argument.IndexOf("DeleteSelectedCompany") != -1)
        {
            if (CompanyGrid.SelectedItems.Count == 1)
            {
                CompanyAjaxManager.AjaxSettings.AddAjaxSetting(CompanyAjaxManager, CompanyGrid);
                CompanyRepository companyRepo = new CompanyRepository();
                companyRepo.Delete(new Company(GetSelectedCompanyID()));
                CompanyGrid.Rebind();
            }
        }
        else if (e.Argument.IndexOf("OpenSeletectedCompany") != -1)
        {
            if (CompanyGrid.SelectedItems.Count == 1)
            {
                Response.Redirect(string.Format("~/CompanyProfile.aspx?CompanyId={0}&mode=edit&backurl=visible", GetSelectedCompanyID()), true);
            }
        }
        else if (e.Argument.IndexOf("ViewCompanyActions") != -1)
        {
            if (CompanyGrid.SelectedItems.Count == 1)
            {
                Response.Redirect(string.Format("~/CompanyProfile.aspx?CompanyId={0}&tab=action&mode=edit&backurl=visible", GetSelectedCompanyID()), true);
            }
        }
        else if (e.Argument.IndexOf("ViewCompanyJobs") != -1)
        {
            if (CompanyGrid.SelectedItems.Count == 1)
            {
                Response.Redirect(string.Format("~/CompanyProfile.aspx?CompanyId={0}&tab=job&mode=edit&backurl=visible", GetSelectedCompanyID()), true);
            }
        }
        else if (e.Argument.IndexOf("ViewCompanyInvoices") != -1)
        {
            if (CompanyGrid.SelectedItems.Count == 1)
            {
                Response.Redirect(string.Format("~/InvoicesPage.aspx?type=search&customer={0}", GetSelectedCompanyID()), true);
            }
        }
    }

    private int GetSelectedCompanyID()
    {
        //
        GridDataItem selectedItem = CompanyGrid.SelectedItems[0] as GridDataItem;
        TableCell companyIDCell = selectedItem["CompanyID"];
        if (!string.IsNullOrEmpty(companyIDCell.Text))
            return Convert.ToInt32(companyIDCell.Text);
        return 0;
    }
    #endregion


}
