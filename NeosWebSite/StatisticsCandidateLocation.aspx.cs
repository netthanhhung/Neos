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

public partial class StatisticsCandidateLocation : System.Web.UI.Page
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
            BuildCandidateLocationChart();
        }
    }

    private void FillLabelsText()
    {
        lblTitle.Text = ResourceManager.GetString("lblRightPaneCandidateLocation");
    }

    private void BuildCandidateLocationChart()
    {
        radChartCandidateLocation.ChartTitle.TextBlock.Text = ResourceManager.GetString("hypCandidateLocation");
        radChartCandidateLocation.Chart.Series.Clear();

        radChartCandidateLocation.PlotArea.Appearance.Dimensions.Margins.Left = 80;
        radChartCandidateLocation.PlotArea.Appearance.Dimensions.Margins.Right = 100;
        radChartCandidateLocation.PlotArea.Appearance.Dimensions.Margins.Top = 60;
        radChartCandidateLocation.PlotArea.Appearance.Dimensions.Margins.Bottom = 200;
        radChartCandidateLocation.PlotArea.YAxis.AxisLabel.TextBlock.Text = ResourceManager.GetString("lblStatisticsNbrOfCandidates");
        radChartCandidateLocation.PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Font = new Font("Arial", 12, FontStyle.Bold);
        radChartCandidateLocation.PlotArea.YAxis.AxisLabel.Visible = true;
        radChartCandidateLocation.PlotArea.YAxis.VisibleValues = ChartAxisVisibleValues.Positive;

        radChartCandidateLocation.PlotArea.XAxis.AxisLabel.TextBlock.Text = ResourceManager.GetString("lblStatisticsLocation");
        radChartCandidateLocation.PlotArea.XAxis.AxisLabel.TextBlock.Appearance.TextProperties.Font = new Font("Arial", 12, FontStyle.Bold);
        radChartCandidateLocation.PlotArea.XAxis.AxisLabel.Visible = true;

        radChartCandidateLocation.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Font = new Font("Arial", 10);
        radChartCandidateLocation.PlotArea.XAxis.AutoScale = false;
        radChartCandidateLocation.PlotArea.XAxis.LayoutMode = ChartAxisLayoutMode.Between;

        radChartCandidateLocation.PlotArea.XAxis.Appearance.LabelAppearance.RotationAngle = 90;
        radChartCandidateLocation.PlotArea.XAxis.Appearance.LabelAppearance.Position.AlignedPosition = AlignedPositions.Top;
        radChartCandidateLocation.PlotArea.XAxis.Appearance.LabelAppearance.Position.Auto = false;
        radChartCandidateLocation.PlotArea.XAxis.Appearance.LabelAppearance.Position.X = 0;
        radChartCandidateLocation.PlotArea.XAxis.Appearance.LabelAppearance.Position.Y = 0;
        ChartSeries locationSeries = new ChartSeries("Location", ChartSeriesType.Bar);
        locationSeries.Appearance.FillStyle.MainColor = Color.LightSteelBlue;
        locationSeries.Appearance.FillStyle.FillType = FillType.Solid;
        locationSeries.Appearance.TextAppearance.Visible = true;
        radChartCandidateLocation.AddChartSeries(locationSeries);

        IList<ParamLocations> locationList = new ParamLocationsRepository().GetAllLocations();
        foreach (ParamLocations location in locationList)
        {
            radChartCandidateLocation.PlotArea.XAxis.AddItem(location.Location.Trim());
            int number = NeosDAO.GetNumberOfCandiateByLocation(location.Location.Trim(),
                location.LocationUk.Trim(), location.LocationNL.Trim());
            locationSeries.AddItem(number);
        }                              
    }
}
