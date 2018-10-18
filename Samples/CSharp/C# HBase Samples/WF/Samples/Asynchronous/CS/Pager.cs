#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using Syncfusion.Windows.Forms.Grid.Grouping;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using Syncfusion.Grouping;
using Syncfusion.Windows.Forms;
using System.Globalization;
using System.Collections;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.ThriftHBase.Base;
namespace Asynchronous
{
    #region Pager class
    /// <summary>
    /// A Paging helper that can be wired with GridGroupingControl to enable data paging with IEnumerable type(DataTable) data source.
    /// </summary>
    class Pager
    {
        int _currentPage;
        private HBaseResultSet ResultSet;
        //private HqlDataReader Reader;
        private readonly Label _myLabel = new Label();
        private GridGroupingControl _grid;
        private string tableName;
        private HBaseConnection con;
        private Panel commonLoaderPic;
        /// <summary>
        /// Wires the paging helper with GridGroupingControl.
        /// </summary>
        /// <param name="groupingGrid">GridGroupingControl</param>
        /// <param name="table">DataTable</param>
        public async void Wire(GridGroupingControl groupingGrid, String tableName, HBaseConnection con,Panel loaderPic)
        {
            commonLoaderPic = loaderPic;
            _grid = groupingGrid;
            this.tableName = tableName;
            this.con = con;
            ResultSet = await HBaseOperation.ScanTableAsync(tableName, con);
            InitializePager();
        }


        /// <summary>
        /// A method that initializes page settings and loads the default page.
        /// </summary>
        private void InitializePager()
        {
            // Initial seeings
            _currentPage = 1;

            _myLabel.Location =
                new Point(
                    _grid.RecordNavigationControl.NavigationBar.ButtonBarChild.Buttons[2].Bounds.X, 0);
            _myLabel.Width =
                _grid.RecordNavigationControl.NavigationBar.ButtonBarChild.Buttons[5].Bounds.X -
                _myLabel.Location.X;
            _myLabel.TextAlign = ContentAlignment.MiddleCenter;

            _grid.RecordNavigationControl.NavigationBar.Controls.Add(_myLabel);


            _grid.ShowNavigationBar = true;
            _grid.RecordNavigationBar.Label = String.Empty;
            _grid.RecordNavigationBar.AllowAddNew = false;
            _grid.RecordNavigationBar.AllowStepIncrease = false;
            _grid.RecordNavigationBar.ButtonLook = ButtonLook.Flat;
            var displayArrow = DisplayArrowButtons.None;
            if (HBaseOperation.HasRows)
             displayArrow = DisplayArrowButtons.Single;
            _grid.RecordNavigationBar.DisplayArrowButtons = displayArrow;
            _grid.RecordNavigationBar.CurrentRecordChanging += RecordNavigationBar_CurrentRecordChanging;
            _grid.RecordNavigationBar.ArrowButtonClicked += RecordNavigationBar_ArrowButtonClicked;
            FillPage(ResultSet);
            
        }

        void RecordNavigationBar_CurrentRecordChanged(object sender, CurrentRecordEventArgs e)
        {
            RecordNavigationScrollBar bar = (RecordNavigationScrollBar)sender;
            bar.EnableButtonFlags = arrowType;
        }

         //<summary>
         //A method that fills the temporary table with paged data to display in grid.
         //</summary>
         //<param name="table">DataTable with latest view applied.</param>
        private void FillPage(HBaseResultSet table)
        {
            DisplayPageInfo();
            if (!resultCollection.ContainsKey(_currentPage))
                resultCollection.Add(_currentPage, table);
            _grid.DataSource = table;

            //Declaring table properties
            commonLoaderPic.Visible = false;
           

        }
        ArrowType arrowType = ArrowType.None;
        /// <summary>
        /// A method to display the page information in pager control at bottom.
        /// </summary>
        private void DisplayPageInfo()
        {
            _myLabel.Text = "Page " + _currentPage.ToString(CultureInfo.InvariantCulture);
            arrowType = ArrowType.None;
            if (_currentPage > 1)
                arrowType |= ArrowType.Previous;
            if (HBaseOperation.HasRows || resultCollection.ContainsKey(_currentPage + 1))
                arrowType |= ArrowType.Next;

            int topRow = _grid.TableControl.TopRowIndex;
            this._grid.TableControl.CurrentCell.Activate(topRow, 1);
            this._grid.RecordNavigationBar.EnableButtonFlags = arrowType;
        }

        /// <summary>
        /// A method that get triggered on current record change in grid.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An <see cref="T:Syncfusion.Windows.Forms.CurrentRecordEventArgs">CurrentRecordEventArgs</see> that contains the event data.</param>
        void RecordNavigationBar_CurrentRecordChanging(object sender, CurrentRecordEventArgs e)
        {
            RecordNavigationScrollBar bar = (RecordNavigationScrollBar)sender;
            bar.EnableButtonFlags = arrowType;
            //if(e.Record!=null)

            e.Cancel = true;
        }


        private Dictionary<int, HBaseResultSet> resultCollection = new Dictionary<int, HBaseResultSet>();
         //<summary>
         //A method that was invoked on clicking arrow buttons in pager control.
         //</summary>
         //<param name="sender">The source of the event.</param>
         //<param name="e">An <see cref="T:Syncfusion.Windows.Forms.ArrowButtonEventArgs">ArrowButtonEventArgs</see> that contains the event data.</param>
        async void RecordNavigationBar_ArrowButtonClicked(object sender, ArrowButtonEventArgs e)
        {
            switch (e.Arrow)
            {
                case ArrowType.Previous:

                    _currentPage -= 1;
                    if (resultCollection.ContainsKey(_currentPage))
                    {
                        ResultSet = resultCollection[_currentPage];
                    }
                    FillPage(ResultSet);
                    break;
                case ArrowType.Next:
                    {
                        _currentPage += 1;
                        if (!HBaseOperation.HasRows && !resultCollection.ContainsKey(_currentPage))
                        {
                            _currentPage -= 1;
                        }
                        else if (resultCollection.ContainsKey(_currentPage))
                        {
                            ResultSet = resultCollection[_currentPage];
                        }
                        else
                        {
                            if (HBaseOperation.HasRows)
                            {
                                _grid.Enabled = false;                                
                                commonLoaderPic.Visible = true;
                                HBaseOperation.FetchSize = 5000;
                                ResultSet = await HBaseOperation.ScanTableAsync(tableName, con);
                                commonLoaderPic.Visible = false;
                                _grid.Enabled = true;
                            }
                        }

                        FillPage(ResultSet);
                        DisplayPageInfo();
                    }
                    break;
            }
        }
    }
    #endregion

    #region Filter Model class - customized for paged grid filtering.
    /// <summary>
    /// A Filter model that applies filter to the Engine.
    /// </summary>
    /// <remarks>This helps to apply filtering across all records in every page.</remarks>
    public class GridTableFilterBarGridListCellModelExt : GridTableFilterBarGridListCellModel
    {
        readonly Engine _engine;
        RecordFilterDescriptorCollection _recordFilters;
        TableDescriptor _tableDescriptor;
        GridTableCellStyleInfoIdentity _identity;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FilterBar.GridTableFilterBarGridListCellModelExt">GridTableFilterBarGridListCellModelExt</see> class. 
        /// </summary>
        /// <param name="grid">The Grid Model</param>
        /// <param name="engine">The Engine</param>
        public GridTableFilterBarGridListCellModelExt(GridModel grid, Engine engine)
            : base(grid)
        {
            _engine = engine;
        }

        /// <summary>
        /// A method that returns the filter values.
        /// </summary>
        /// <param name="column">Column in which filter is being applied.</param>
        /// <returns>Set of filter values.</returns>
        private object[] GetFilterValues(GridColumnDescriptor column)
        {
            var sd = new SummaryDescriptor(
                                column.Name + "FilterBarChoices",
                                column.MappingName,
                                FilterBarChoicesSummary.CreateSummaryMethod) { IgnoreRecordFilterCriteria = true };
            if (!_engine.Table.TableDescriptor.Summaries.Contains(sd.Name))
                _engine.Table.TableDescriptor.Summaries.Add(sd);
            var summaries = _engine.Table.GetSummaries();
            var summaryIndex = _engine.Table.TableDescriptor.Summaries.IndexOf(sd);
            var filtersummary = (FilterBarChoicesSummary)summaries[summaryIndex];
            var values = filtersummary.Values;
            _engine.Table.TableDescriptor.Summaries.Remove(sd);
            _engine.Table.SummariesDirty = true;
            return values;
        }

        /// <summary>
        /// An overridden method that gets called when the filter dropdown is opened.
        /// </summary>
        /// <param name="tableCellIdentity">StyleInfoIdentity</param>
        /// <returns>Set of filter items.</returns>
        public override object[] GetFilterBarChoices(GridTableCellStyleInfoIdentity tableCellIdentity)
        {
            _identity = tableCellIdentity;
            return GetFilterValues(_identity.Column);
        }

        /// <summary>
        /// A method that returns filter collection applied so far to the respective column.
        /// </summary>
        /// <param name="recordFilters">Filtercollection</param>
        /// <param name="tableCellIdentity">StyleInfoIdentity</param>
        /// <returns>Collection of filters.</returns>
        private IEnumerable<RecordFilterDescriptor> GetFilters(RecordFilterDescriptorCollection recordFilters, GridTableCellStyleInfoIdentity tableCellIdentity)
        {
            var rfdc = recordFilters.GetRecordFilters(tableCellIdentity.Column.MappingName);
            if (rfdc != null)
            {
                var uniqueId = tableCellIdentity.DisplayElement.ParentGroup.UniqueGroupId;
                var rfdList = new ArrayList();
                foreach (var rfd in rfdc)
                {
                    if (rfd.CompareUniqueId(uniqueId))
                    {
                        rfdList.Add(rfd);
                    }
                }

                if (rfdList.Count > 0)
                {
                    return (RecordFilterDescriptor[])rfdList.ToArray(typeof(RecordFilterDescriptor));
                }
            }
            return null;
        }

        /// <summary>
        /// A method that applies (All) type filtering to the column.
        /// </summary>
        /// <remarks>Resets the filter applied to that column.</remarks>
        public void SelectAll()
        {
            _tableDescriptor = _engine.TableDescriptor;//identity.Table.TableDescriptor;
            _recordFilters = new RecordFilterDescriptorCollection();
            _recordFilters.InitializeFrom(_tableDescriptor.RecordFilters);
            var removefdc = GetFilters(_recordFilters, _identity);
            if (removefdc == null) return;
            foreach (var rfd in removefdc)
            {
                _recordFilters.Remove(rfd);
            }
        }

        /// <summary>
        /// A method that applies specific item filtering to the column.
        /// </summary>
        /// <param name="index">selected index</param>
        public void SelectItem(int index)
        {
            var filterName = GetUniqueColumnGroupId(_identity);

            _tableDescriptor = _engine.TableDescriptor;//identity.Table.TableDescriptor;
            _recordFilters = new RecordFilterDescriptorCollection();
            _recordFilters.InitializeFrom(_tableDescriptor.RecordFilters);

            var items = GetFilterBarChoices(_identity);

            ////check to see if list was shortened after index was set
            if (index >= items.GetLength(0))
            {
                return;
            }

            var value = items[index];

            var removefdc = GetFilters(_recordFilters, _identity);
            if (removefdc != null)
            {
                foreach (var rfd in removefdc)
                {
                    _recordFilters.Remove(rfd);
                }
            }

            var newFilter = new RecordFilterDescriptor
            {
                Name = filterName,
                MappingName = _identity.Column.MappingName,
                UniqueGroupId = GetUniqueGroupId(_identity)
            };
            newFilter.Conditions.Add(new FilterCondition(FilterCompareOperator.Equals, value));
            _recordFilters.Add(newFilter);
        }

        /// <summary>
        /// Applies the filter to the Engine.
        /// </summary>
        public void Apply()
        {
            if (_tableDescriptor != null && _recordFilters != null)
            {
                _tableDescriptor.RecordFilters.InitializeFrom(_recordFilters);
            }
            _recordFilters = null;
        }
    }
    #endregion
}
