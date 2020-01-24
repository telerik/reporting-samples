namespace ReportLibrary1
{
    using System;
    using System.Linq;
    using Telerik.Reporting;

    /// <summary>
    /// Summary description for Report1
    /// </summary>
    public partial class Report1 : Telerik.Reporting.Report
	{
		public Report1()
		{
			//
			// Required for telerik Reporting designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

        private void table1_ItemDataBinding(object sender, EventArgs e)
        {
            var table = sender as Telerik.Reporting.Processing.Table;
            bool expand = (bool)table.Report.Parameters["Expand"].Value;

            ToggleVisibilityAction drillDownAction = (ToggleVisibilityAction)this.textBox11.Action;
            ExpandDrillDownAction(expand, drillDownAction);

            ToggleVisibilityAction drillDownAction17 = (ToggleVisibilityAction)this.textBox17.Action;
            ExpandDrillDownAction(expand, drillDownAction17);
        }

        private static void ExpandDrillDownAction(bool expand, ToggleVisibilityAction drillDownAction)
        {
            drillDownAction.DisplayExpandedMark = expand;
            foreach (var t in drillDownAction.Targets.Cast<IToggleVisibilityTarget>())
            {
                if (t is ReportItemBase)
                {
                    (t as ReportItemBase).Visible = expand;
                }

                if (t is TableGroup)
                {
                    (t as TableGroup).Visible = expand;

                }
            }
        }
    }
}