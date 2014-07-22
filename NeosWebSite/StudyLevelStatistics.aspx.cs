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

public partial class StudyLevelStatistics : System.Web.UI.Page
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (SessionManager.CurrentUser == null)
        {
            Common.RedirectToLoginPage(this);
            return;
        }
        else if (!IsPostBack)
        {
            FillLabelsText();
            BuildStudyLevelChart();
        }
    }

    private void FillLabelsText()
    {
        lblTitle.Text = ResourceManager.GetString("lblRightPaneStudyLevel");
    }

    private void BuildStudyLevelChart()
    {
        IList<StatisticsStudyLevel> stydyLevelList = NeosDAO.GetStatisticsStudyLevels();
        radChartStudyLevel.ChartTitle.TextBlock.Text = ResourceManager.GetString("hypStudyLevelStatistics");
        radChartStudyLevel.Chart.Series.Clear();

        radChartStudyLevel.PlotArea.Appearance.Dimensions.Margins.Left = 100;
        radChartStudyLevel.PlotArea.Appearance.Dimensions.Margins.Right = 100;
        radChartStudyLevel.PlotArea.Appearance.Dimensions.Margins.Top= 100;
        radChartStudyLevel.PlotArea.Appearance.Dimensions.Margins.Bottom = 100;

        radChartStudyLevel.PlotArea.YAxis.AxisLabel.TextBlock.Text = ResourceManager.GetString("lblStatisticsNbrOfCandidates");
        radChartStudyLevel.PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Font = new Font("Arial", 12, FontStyle.Bold);        
        radChartStudyLevel.PlotArea.YAxis.AxisLabel.Visible = true;

        radChartStudyLevel.PlotArea.XAxis.AxisLabel.TextBlock.Text = ResourceManager.GetString("lblStatisticsStudyLevel");
        radChartStudyLevel.PlotArea.XAxis.AxisLabel.TextBlock.Appearance.TextProperties.Font = new Font("Arial", 12, FontStyle.Bold);        
        radChartStudyLevel.PlotArea.XAxis.AxisLabel.Visible = true;

        radChartStudyLevel.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Font = new Font("Arial", 10);        
        radChartStudyLevel.PlotArea.XAxis.AutoScale = false;
        radChartStudyLevel.PlotArea.XAxis.LayoutMode = ChartAxisLayoutMode.Between;
        IList<YearNumber> maxYearNumberList = new List<YearNumber>();
        foreach (StatisticsStudyLevel item in stydyLevelList)
        {
            radChartStudyLevel.PlotArea.XAxis.AddItem(item.StudyLevelString.Trim());
            if (maxYearNumberList.Count < item.YearNumberList.Count)
            {
                maxYearNumberList = item.YearNumberList;
            }
        }
        
        foreach (YearNumber yearNumber in maxYearNumberList)
        {
            ChartSeries yearSeries = null;
            if (yearNumber.Year != -1)
            {
                yearSeries = new ChartSeries(yearNumber.Year.ToString(), ChartSeriesType.Bar);
            }
            else
            {
                yearSeries = new ChartSeries("(" + ResourceManager.GetString("blankText") + ")", ChartSeriesType.Bar);
            }
            yearSeries.Appearance.FillStyle.FillType = FillType.Solid;
            yearSeries.Appearance.TextAppearance.Visible = false;            
            radChartStudyLevel.AddChartSeries(yearSeries);
            int i = 0;
            foreach (StatisticsStudyLevel studyItem in stydyLevelList)
            {
                int number = 0;
                foreach (YearNumber yearNumber2 in studyItem.YearNumberList)
                {
                    if (yearNumber2.Year == yearNumber.Year)
                    {
                        number = yearNumber2.Number;
                    }
                }
                yearSeries.AddItem(number);
                yearSeries[i++].ActiveRegion.Tooltip = number.ToString();

            }

        }                
    }
}
