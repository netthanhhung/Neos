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

public partial class StatisticsCanidateInscription : System.Web.UI.Page
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
            BuildInscriptionYearChart();            
            BuildInscriptionMonthChart();
            BuildInscriptionWeekChart();
            BuildInscriptionDayChart();
        }
    }

    private void FillLabelsText()
    {
        lblTitle.Text = ResourceManager.GetString("lblRightPaneCandidateInscriptions");
    }
    
    private void BuildInscriptionYearChart()
    {
        radChartInscriptionYear.ChartTitle.TextBlock.Text = ResourceManager.GetString("lblEvolutionByYear");
        radChartInscriptionYear.Chart.Series.Clear();

        radChartInscriptionYear.PlotArea.Appearance.Dimensions.Margins.Left = 80;
        radChartInscriptionYear.PlotArea.Appearance.Dimensions.Margins.Right = 80;
        radChartInscriptionYear.PlotArea.Appearance.Dimensions.Margins.Top = 60;
        radChartInscriptionYear.PlotArea.Appearance.Dimensions.Margins.Bottom = 60;
        radChartInscriptionYear.PlotArea.YAxis.AxisLabel.TextBlock.Text = ResourceManager.GetString("lblStatisticsNbrOfCandidates");
        radChartInscriptionYear.PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Font = new Font("Arial", 12, FontStyle.Bold);        
        radChartInscriptionYear.PlotArea.YAxis.AxisLabel.Visible = true;
        radChartInscriptionYear.PlotArea.YAxis.VisibleValues = ChartAxisVisibleValues.Positive;

        radChartInscriptionYear.PlotArea.XAxis.AxisLabel.TextBlock.Text = ResourceManager.GetString("yearText");
        radChartInscriptionYear.PlotArea.XAxis.AxisLabel.TextBlock.Appearance.TextProperties.Font = new Font("Arial", 12, FontStyle.Bold);        
        radChartInscriptionYear.PlotArea.XAxis.AxisLabel.Visible = true;

        radChartInscriptionYear.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Font = new Font("Arial", 10);        
        radChartInscriptionYear.PlotArea.XAxis.AutoScale = false;
        radChartInscriptionYear.PlotArea.XAxis.LayoutMode = ChartAxisLayoutMode.Between;
        
        ChartSeries yearSeries = new ChartSeries("Year", ChartSeriesType.Bar);
        yearSeries.Appearance.FillStyle.MainColor = Color.Pink;
        yearSeries.Appearance.FillStyle.FillType = FillType.Solid;
        yearSeries.Appearance.TextAppearance.Visible = true;
        radChartInscriptionYear.AddChartSeries(yearSeries);

        int currentYear = DateTime.Today.Year;
        radChartInscriptionYear.PlotArea.XAxis.AddItem((currentYear - 4).ToString());
        radChartInscriptionYear.PlotArea.XAxis.AddItem((currentYear - 3).ToString());
        radChartInscriptionYear.PlotArea.XAxis.AddItem((currentYear - 2).ToString());
        radChartInscriptionYear.PlotArea.XAxis.AddItem((currentYear - 1).ToString());
        radChartInscriptionYear.PlotArea.XAxis.AddItem((currentYear).ToString());

        int year4No = NeosDAO.GetNumberOfCandiatesInscription(new DateTime(currentYear - 4, 1, 1, 0, 0, 0), 
            new DateTime(currentYear - 4, 12, 31, 23, 59, 59));
        int year3No = NeosDAO.GetNumberOfCandiatesInscription(new DateTime(currentYear - 3, 1, 1, 0, 0, 0),
            new DateTime(currentYear - 3, 12, 31, 23, 59, 59));
        int year2No = NeosDAO.GetNumberOfCandiatesInscription(new DateTime(currentYear - 2, 1, 1, 0, 0, 0),
            new DateTime(currentYear - 2, 12, 31, 23, 59, 59));
        int year1No = NeosDAO.GetNumberOfCandiatesInscription(new DateTime(currentYear - 1, 1, 1, 0, 0, 0),
            new DateTime(currentYear - 1, 12, 31, 23, 59, 59));
        int yearNo = NeosDAO.GetNumberOfCandiatesInscription(new DateTime(currentYear, 1, 1, 0, 0, 0),
            DateTime.Now);

        yearSeries.AddItem(year4No);
        yearSeries.AddItem(year3No);
        yearSeries.AddItem(year2No);
        yearSeries.AddItem(year1No);
        yearSeries.AddItem(yearNo);                
    }
    
    private void BuildInscriptionMonthChart()
    {
        radChartInscriptionMonth.ChartTitle.TextBlock.Text = ResourceManager.GetString("lblEvolutionByMonth");
        radChartInscriptionMonth.Chart.Series.Clear();

        radChartInscriptionMonth.PlotArea.Appearance.Dimensions.Margins.Left = 80;
        radChartInscriptionMonth.PlotArea.Appearance.Dimensions.Margins.Right = 80;
        radChartInscriptionMonth.PlotArea.Appearance.Dimensions.Margins.Top = 60;
        radChartInscriptionMonth.PlotArea.Appearance.Dimensions.Margins.Bottom = 60;
        radChartInscriptionMonth.PlotArea.YAxis.AxisLabel.TextBlock.Text = ResourceManager.GetString("lblStatisticsNbrOfCandidates");
        radChartInscriptionMonth.PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Font = new Font("Arial", 12, FontStyle.Bold);
        radChartInscriptionMonth.PlotArea.YAxis.AxisLabel.Visible = true;
        radChartInscriptionMonth.PlotArea.YAxis.VisibleValues = ChartAxisVisibleValues.Positive;

        radChartInscriptionMonth.PlotArea.XAxis.AxisLabel.TextBlock.Text = ResourceManager.GetString("monthText");
        radChartInscriptionMonth.PlotArea.XAxis.AxisLabel.TextBlock.Appearance.TextProperties.Font = new Font("Arial", 12, FontStyle.Bold);
        radChartInscriptionMonth.PlotArea.XAxis.AxisLabel.Visible = true;

        radChartInscriptionMonth.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Font = new Font("Arial", 10);
        radChartInscriptionMonth.PlotArea.XAxis.AutoScale = false;
        radChartInscriptionMonth.PlotArea.XAxis.LayoutMode = ChartAxisLayoutMode.Between;
        
        ChartSeries monthSeries = new ChartSeries("Month", ChartSeriesType.Bar);
        monthSeries.Appearance.FillStyle.MainColor = Color.Orange;
        monthSeries.Appearance.FillStyle.FillType = FillType.Solid;
        monthSeries.Appearance.TextAppearance.Visible = true;
        radChartInscriptionMonth.AddChartSeries(monthSeries);

        int currentMonth = DateTime.Today.Month;

        radChartInscriptionMonth.PlotArea.XAxis.AddItem("M - 4");
        radChartInscriptionMonth.PlotArea.XAxis.AddItem("M - 3");
        radChartInscriptionMonth.PlotArea.XAxis.AddItem("M - 2");
        radChartInscriptionMonth.PlotArea.XAxis.AddItem("M - 1");
        radChartInscriptionMonth.PlotArea.XAxis.AddItem("This month");

        DateTime beginMonth = new DateTime(DateTime.Today.Year, currentMonth, 1, 0, 0, 0);
        int month4No = NeosDAO.GetNumberOfCandiatesInscription(
            beginMonth.AddMonths(-4), beginMonth.AddMonths(-3).AddSeconds(-1));
        int month3No = NeosDAO.GetNumberOfCandiatesInscription(
            beginMonth.AddMonths(-3), beginMonth.AddMonths(-2).AddSeconds(-1));
        int month2No = NeosDAO.GetNumberOfCandiatesInscription(
            beginMonth.AddMonths(-2), beginMonth.AddMonths(-1).AddSeconds(-1));
        int month1No = NeosDAO.GetNumberOfCandiatesInscription(
            beginMonth.AddMonths(-1), beginMonth.AddSeconds(-1));
        int monthNo = NeosDAO.GetNumberOfCandiatesInscription(
            beginMonth, DateTime.Now);

        monthSeries.AddItem(month4No);
        monthSeries.AddItem(month3No);
        monthSeries.AddItem(month2No);
        monthSeries.AddItem(month1No);
        monthSeries.AddItem(monthNo);     
    }

    private void BuildInscriptionWeekChart()
    {
        radChartInscriptionWeek.ChartTitle.TextBlock.Text = ResourceManager.GetString("lblEvolutionByWeek");
        radChartInscriptionWeek.Chart.Series.Clear();

        radChartInscriptionWeek.PlotArea.Appearance.Dimensions.Margins.Left = 80;
        radChartInscriptionWeek.PlotArea.Appearance.Dimensions.Margins.Right = 80;
        radChartInscriptionWeek.PlotArea.Appearance.Dimensions.Margins.Top = 60;
        radChartInscriptionWeek.PlotArea.Appearance.Dimensions.Margins.Bottom = 60;
        radChartInscriptionWeek.PlotArea.YAxis.AxisLabel.TextBlock.Text = ResourceManager.GetString("lblStatisticsNbrOfCandidates");
        radChartInscriptionWeek.PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Font = new Font("Arial", 12, FontStyle.Bold);
        radChartInscriptionWeek.PlotArea.YAxis.AxisLabel.Visible = true;
        radChartInscriptionWeek.PlotArea.YAxis.VisibleValues = ChartAxisVisibleValues.Positive;

        radChartInscriptionWeek.PlotArea.XAxis.AxisLabel.TextBlock.Text = ResourceManager.GetString("weekText");
        radChartInscriptionWeek.PlotArea.XAxis.AxisLabel.TextBlock.Appearance.TextProperties.Font = new Font("Arial", 12, FontStyle.Bold);
        radChartInscriptionWeek.PlotArea.XAxis.AxisLabel.Visible = true;

        radChartInscriptionWeek.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Font = new Font("Arial", 10);
        radChartInscriptionWeek.PlotArea.XAxis.AutoScale = false;
        radChartInscriptionWeek.PlotArea.XAxis.LayoutMode = ChartAxisLayoutMode.Between;

        ChartSeries weekSeries = new ChartSeries("Week", ChartSeriesType.Bar);
        weekSeries.Appearance.FillStyle.MainColor = Color.LightSteelBlue;
        weekSeries.Appearance.FillStyle.FillType = FillType.Solid;
        weekSeries.Appearance.TextAppearance.Visible = true;
        radChartInscriptionWeek.AddChartSeries(weekSeries);        

        radChartInscriptionWeek.PlotArea.XAxis.AddItem("W - 4");
        radChartInscriptionWeek.PlotArea.XAxis.AddItem("W - 3");
        radChartInscriptionWeek.PlotArea.XAxis.AddItem("W - 2");
        radChartInscriptionWeek.PlotArea.XAxis.AddItem("W - 1");
        radChartInscriptionWeek.PlotArea.XAxis.AddItem("This week");


        DateTime beginWeek = Common.GetBeginDayOfWeek(DateTime.Today);
        //beginWeek = beginWeek.AddYears(-3);
        int week4No = NeosDAO.GetNumberOfCandiatesInscription(
            beginWeek.AddDays(-28), beginWeek.AddDays(-21).AddSeconds(-1));
        int week3No = NeosDAO.GetNumberOfCandiatesInscription(
            beginWeek.AddDays(-21), beginWeek.AddDays(-14).AddSeconds(-1));
        int week2No = NeosDAO.GetNumberOfCandiatesInscription(
            beginWeek.AddDays(-14), beginWeek.AddDays(-7).AddSeconds(-1));
        int week1No = NeosDAO.GetNumberOfCandiatesInscription(
            beginWeek.AddDays(-7), beginWeek.AddSeconds(-1));
        int weekNo = NeosDAO.GetNumberOfCandiatesInscription(
            beginWeek, DateTime.Now);

        weekSeries.AddItem(week4No);
        weekSeries.AddItem(week3No);
        weekSeries.AddItem(week2No);
        weekSeries.AddItem(week1No);
        weekSeries.AddItem(weekNo);     
    }

    private void BuildInscriptionDayChart()
    {
        radChartInscriptionDay.ChartTitle.TextBlock.Text = ResourceManager.GetString("lblEvolutionByDay");
        radChartInscriptionDay.Chart.Series.Clear();

        radChartInscriptionDay.PlotArea.Appearance.Dimensions.Margins.Left = 80;
        radChartInscriptionDay.PlotArea.Appearance.Dimensions.Margins.Right = 80;
        radChartInscriptionDay.PlotArea.Appearance.Dimensions.Margins.Top = 60;
        radChartInscriptionDay.PlotArea.Appearance.Dimensions.Margins.Bottom = 60;
        radChartInscriptionDay.PlotArea.YAxis.AxisLabel.TextBlock.Text = ResourceManager.GetString("lblStatisticsNbrOfCandidates");
        radChartInscriptionDay.PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Font = new Font("Arial", 12, FontStyle.Bold);
        radChartInscriptionDay.PlotArea.YAxis.AxisLabel.Visible = true;
        radChartInscriptionDay.PlotArea.YAxis.VisibleValues = ChartAxisVisibleValues.Positive;

        radChartInscriptionDay.PlotArea.XAxis.AxisLabel.TextBlock.Text = ResourceManager.GetString("dayText");
        radChartInscriptionDay.PlotArea.XAxis.AxisLabel.TextBlock.Appearance.TextProperties.Font = new Font("Arial", 12, FontStyle.Bold);
        radChartInscriptionDay.PlotArea.XAxis.AxisLabel.Visible = true;

        radChartInscriptionDay.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Font = new Font("Arial", 10);
        radChartInscriptionDay.PlotArea.XAxis.AutoScale = false;
        radChartInscriptionDay.PlotArea.XAxis.LayoutMode = ChartAxisLayoutMode.Between;

        ChartSeries daySeries = new ChartSeries("Day", ChartSeriesType.Bar);
        daySeries.Appearance.FillStyle.MainColor = Color.LightBlue;
        daySeries.Appearance.FillStyle.FillType = FillType.Solid;
        daySeries.Appearance.TextAppearance.Visible = true;
        radChartInscriptionDay.AddChartSeries(daySeries);

        radChartInscriptionDay.PlotArea.XAxis.AddItem("D - 4");
        radChartInscriptionDay.PlotArea.XAxis.AddItem("D - 3");
        radChartInscriptionDay.PlotArea.XAxis.AddItem("D - 2");
        radChartInscriptionDay.PlotArea.XAxis.AddItem("D - 1");
        radChartInscriptionDay.PlotArea.XAxis.AddItem("Today");

        DateTime currentDay = DateTime.Today;
        //currentDay = currentDay.AddYears(-3);
        int day4No = NeosDAO.GetNumberOfCandiatesInscription(
            currentDay.AddDays(-4), currentDay.AddDays(-3).AddSeconds(-1));
        int day3No = NeosDAO.GetNumberOfCandiatesInscription(
            currentDay.AddDays(-3), currentDay.AddDays(-2).AddSeconds(-1));
        int day2No = NeosDAO.GetNumberOfCandiatesInscription(
            currentDay.AddDays(-2), currentDay.AddDays(-1).AddSeconds(-1));
        int day1No = NeosDAO.GetNumberOfCandiatesInscription(
            currentDay.AddDays(-1), currentDay.AddSeconds(-1));
        int dayNo = NeosDAO.GetNumberOfCandiatesInscription(
            currentDay, DateTime.Now);
        
        daySeries.AddItem(day4No);
        daySeries.AddItem(day3No);
        daySeries.AddItem(day2No);
        daySeries.AddItem(day1No);
        daySeries.AddItem(dayNo);  
    }
}
