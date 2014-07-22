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

public partial class Messages : System.Web.UI.Page
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
    public int PageSize
    {
        get { return MessageGrid.PageSize; }
        set { MessageGrid.PageSize = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionManager.CurrentUser == null)
        {
            Common.RedirectToLoginPage(this);
            return;
        }

        if (!IsPostBack)
        {
            FillLabelsText();
            InitControls();
            BindData();
        }
    }

    private void InitControls()
    {
        HttpCookie messageGridPageSizeCookie = Request.Cookies.Get("usrmsggrdps");
        if (messageGridPageSizeCookie != null)
        {
            if (!string.IsNullOrEmpty(messageGridPageSizeCookie.Value))
                MessageGrid.PageSize = Convert.ToInt32(messageGridPageSizeCookie.Value) > 0 ? Convert.ToInt32(messageGridPageSizeCookie.Value) : MessageGrid.PageSize;
        }
    }

    private void FillLabelsText()
    {
        lblTitle.Text = ResourceManager.GetString("lblRightPaneNotification");
    }

    private void BindData()
    {
        UserMessageRepository messageRepo = new UserMessageRepository();
        MessageGrid.DataSource = messageRepo.GetUnreadJobRemindMessagesToday(SessionManager.CurrentUser.UserID);
        MessageGrid.DataBind();
    }

    #region grid events
    protected void OnMessageGridPageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        MessageGrid.CurrentPageIndex = e.NewPageIndex;
        MessageGrid.Rebind();
    }

    protected void OnMessageGridItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem || e.Item.ItemType == Telerik.Web.UI.GridItemType.Item)
        {
            UserMessage message = e.Item.DataItem as UserMessage;
            if (message != null)
            {
                e.Item.Font.Bold = message.IsUnread;
            }
        }
    }

    protected void OnMessageGridNeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (SessionManager.CurrentUser != null)
        {
            UserMessageRepository messageRepo = new UserMessageRepository();
            MessageGrid.DataSource = messageRepo.GetUnreadJobRemindMessagesToday(SessionManager.CurrentUser.UserID);
        }
    }

    protected void OnMessageGrid_PageSizeChanged(object source, Telerik.Web.UI.GridPageSizeChangedEventArgs e)
    {
        HttpCookie messageGridPageSizeCookie = new HttpCookie("usrmsggrdps");
        messageGridPageSizeCookie.Expires.AddDays(30);
        messageGridPageSizeCookie.Value = e.NewPageSize.ToString();
        Response.Cookies.Add(messageGridPageSizeCookie);
    }
    #endregion

    protected void OnMyAjaxManagerAjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        if (e.Argument.IndexOf("MarkAsUnread") != -1 || e.Argument.IndexOf("MarkAsRead") != -1)
        {
            //MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, ddlActions);            
            //MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, lblScript);

            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, MessageGrid);            
            UpdateMessagesStatus(e.Argument);
        }
        else if (e.Argument.IndexOf("DeleteMessage") != -1)
        {
            MyAjaxManager.AjaxSettings.AddAjaxSetting(MyAjaxManager, MessageGrid);
            DeleteMessages();
        }

        UserMessageRepository messageRepo = new UserMessageRepository();
        int count = messageRepo.CountUnreadJobRemindMessagesToday(SessionManager.CurrentUser.UserID);
        bool isBold = (count > 0);
        string script = string.Format("resetUnreadMessagesText('{0}',\"{1}\",'{2}');", "lnkUnreadMessage", string.Format(ResourceManager.GetString("lblUnreadMessage"), count), isBold);

        MyAjaxManager.ResponseScripts.Add(script);
    }

    private void DeleteMessages()
    {
        UserMessageRepository messageRepo = new UserMessageRepository();
        foreach (GridDataItem selectedItem in MessageGrid.SelectedItems)
        {
            TableCell messageIDCell = selectedItem["MessageID"];
            if (messageIDCell != null)
            {
                int messageID = Convert.ToInt32(messageIDCell.Text);
                UserMessage message = messageRepo.FindOne(new UserMessage(messageID));
                if (message != null)
                {                    
                    messageRepo.Delete(message);
                }

            }
        }
        MessageGrid.Rebind();
    }

    private void UpdateMessagesStatus(string status)
    {
        UserMessageRepository messageRepo = new UserMessageRepository();
        foreach (GridDataItem selectedItem in MessageGrid.SelectedItems)
        {
            TableCell messageIDCell = selectedItem["MessageID"];
            if (messageIDCell != null)
            {
                int messageID = Convert.ToInt32(messageIDCell.Text);
                UserMessage message = messageRepo.FindOne(new UserMessage(messageID));
                if (message != null)
                {
                    if (status == "MarkAsRead")
                    {
                        message.IsUnread = false;
                    }
                    else if (status == "MarkAsUnread")
                    {
                        message.IsUnread = true;
                    }
                    messageRepo.Update(message);
                }

            }
        }
        MessageGrid.Rebind();
    }
}
