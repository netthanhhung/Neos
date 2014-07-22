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
using Telerik.Charting.Styles;
using Telerik.Charting;
using System.Drawing;

public partial class StatisticsGeneral : System.Web.UI.Page
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

            //set properties for the labels of each column
            RadChartGeneralStatistics.PlotArea.Appearance.Dimensions.Margins.Left = 150;
            RadChartGeneralStatistics.PlotArea.Appearance.Dimensions.Margins.Right = 50;

            RadChartGeneralStatistics.PlotArea.XAxis.Appearance.TextAppearance.MaxLength = 1000;
            RadChartGeneralStatistics.PlotArea.XAxis.Appearance.TextAppearance.AutoTextWrap = AutoTextWrap.Auto;
            RadChartGeneralStatistics.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Font = new Font("Arial", 10);
            RadChartGeneralStatistics.PlotArea.XAxis.AutoScale = false;            
            RadChartGeneralStatistics.PlotArea.XAxis.LayoutMode = ChartAxisLayoutMode.Between;

            //set the value for the columns
            ChartSeries generalSeries = new ChartSeries();// ("General", ChartSeriesType.a);
            generalSeries.Appearance.FillStyle.MainColor = Color.LightGreen;
            generalSeries.Appearance.FillStyle.FillType = FillType.Solid;
            generalSeries.Appearance.TextAppearance.Visible = true;
            RadChartGeneralStatistics.Chart.Series.Clear();
            
            RadChartGeneralStatistics.AddChartSeries(generalSeries);            
            
            List<int> generalStatisticList = new List<int>();

            CandidateSearchCriteria criteria = new CandidateSearchCriteria();
            //Number of active candidates.
            criteria.Active = "Yes";
            RadChartGeneralStatistics.PlotArea.XAxis.AddItem(ResourceManager.GetString("lblNbrActiveCandidateText"));
            generalSeries.AddItem(NeosDAO.CountAdvancedSearchCandidates(criteria));
            //Total number of candidates.
            criteria.Active = null;
            RadChartGeneralStatistics.PlotArea.XAxis.AddItem(ResourceManager.GetString("lblTotalCandidateText"));
            generalSeries.AddItem(NeosDAO.CountAdvancedSearchCandidates(criteria));          
            
            //Number of active candidates related to the currently logged Néos user (based on [tblCandidat].[Interviewer])
            criteria.Active = "Yes";
            criteria.Interviewer = SessionManager.CurrentUser.UserID.Trim();
            RadChartGeneralStatistics.PlotArea.XAxis.AddItem(ResourceManager.GetString("lblNbrActCanOfCurUserText"));
            generalSeries.AddItem(NeosDAO.CountAdvancedSearchCandidates(criteria));            
            //Total number of candidates related to the currently logged Néos user
            criteria.Active = null;
            RadChartGeneralStatistics.PlotArea.XAxis.AddItem(ResourceManager.GetString("lblTotalCanOfCurUserText"));
            generalSeries.AddItem(NeosDAO.CountAdvancedSearchCandidates(criteria));
            CompanyRepository comRepo = new CompanyRepository();
            //Number of companies.                        
            RadChartGeneralStatistics.PlotArea.XAxis.AddItem(ResourceManager.GetString("lblNbrCompanyText"));
            generalSeries.AddItem(comRepo.CountAllCompanies());
            //Number of Active Customers 
            RadChartGeneralStatistics.PlotArea.XAxis.AddItem(ResourceManager.GetString("lblNbrActiveCustomerText"));
            generalSeries.AddItem(comRepo.CountCustomerCompanies());
            
            //Number of Prospects
            RadChartGeneralStatistics.PlotArea.XAxis.AddItem(ResourceManager.GetString("lblNbrProspectsText"));
            generalSeries.AddItem(comRepo.CountPropectCompanies());            
        }
    }

    private void FillLabelLanguage()
    {
        //lblNbrActiveCandidateText.Text = ResourceManager.GetString("lblNbrActiveCandidateText");
        //lblTotalCandidateText.Text = ResourceManager.GetString("lblTotalCandidateText");
        //lblNbrActCanOfCurUserText.Text = ResourceManager.GetString("lblNbrActCanOfCurUserText");
        //lblTotalCanOfCurUserText.Text = ResourceManager.GetString("lblTotalCanOfCurUserText");
        //lblNbrCompanyText.Text = ResourceManager.GetString("lblNbrCompanyText");
        //lblNbrActiveCustomerText.Text = ResourceManager.GetString("lblNbrActiveCustomerText");
        //lblNbrProspectsText.Text = ResourceManager.GetString("lblNbrProspectsText");
        lblStatisticTitle.Text = ResourceManager.GetString("lblRightPaneStatistics");
    }
}
