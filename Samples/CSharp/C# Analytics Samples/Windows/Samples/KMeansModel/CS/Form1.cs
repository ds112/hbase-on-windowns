using Syncfusion.PMML;
using Syncfusion.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KMeansModel
{
    public partial class Form1 : MetroForm
    {
        private Table outputTable = null;
        DataTable inputDataTable = new DataTable();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var mergeTable = MergeTable(inputDataTable, outputTable);
            this.gridGroupingControl1.DataSource = mergeTable;
            this.gridGroupingControl1.Table.DefaultCaptionRowHeight = 25;
            this.gridGroupingControl1.Table.DefaultColumnHeaderRowHeight = 30;
            this.gridGroupingControl1.Table.DefaultRecordRowHeight = 22;
            this.gridGroupingControl1.TopLevelGroupOptions.ShowAddNewRecordBeforeDetails = false;
            this.gridGroupingControl1.TopLevelGroupOptions.ShowCaption = false;
            this.gridGroupingControl1.GridVisualStyles = GridVisualStyles.Metro;
            this.gridGroupingControl1.AllowProportionalColumnSizing = true;

            for (int i = 0; i < mergeTable.Columns.Count; i++)
            {
                if (i >= inputDataTable.Columns.Count - 1)
                {
                    this.gridGroupingControl1.TableDescriptor.Columns[i].Appearance.AnyRecordFieldCell.BackColor = Color.FromArgb(214, 211, 209);
                }
            }
        }

        private DataTable MergeTable(DataTable inputDataTable, Syncfusion.PMML.Table outputTable)
        {
            var value = 0;
            string pmmlPath = "../../Model/kmeans_data.pmml";
            string inputPath = "../../Model/kmeans_data.txt";
            string[] lines = System.IO.File.ReadAllLines(inputPath);
            outputTable = PredictResult(lines, pmmlPath);
            string[] words = lines[0].Split(' ');
            value = words.Length;
            for (int i = 0; i < value; i++)
            {
                inputDataTable.Columns.Add("field_" + i);
            }
            foreach (var item in lines)
            {
                words = item.Split(' ');
                inputDataTable.Rows.Add(words);
            }
            var columnEnumarator = outputTable.ColumnNames.GetEnumerator();

            while (columnEnumarator.MoveNext())
            {
                var column = new DataColumn() { ColumnName = columnEnumarator.Current.ToString() };
                inputDataTable.Columns.Add(column);
            }

            for (int i = 0; i < inputDataTable.Rows.Count; i++)
            {
                for (int j = 0; j < outputTable.ColumnNames.Length; j++)
                {
                    inputDataTable.Rows[i].SetField(outputTable.ColumnNames[j], outputTable[i, j]);
                }
            }

            return inputDataTable;
        }

        public Table PredictResult(string[] input, string pmmlPath)
        {
            //Get PMML Evaluator instance
            PMMLEvaluator evaluator = new PMMLEvaluatorFactory().
              GetPMMLEvaluatorInstance(pmmlPath);

            string[] predictedCategories = null;
            int i = 0;
            foreach (string s in input)
            {
                string[] words = s.Split(' ');

                var record = new
                {
                    field_0 = words[0],
                    field_1 = words[1],
                    field_2 = words[2]
                };
                PredictedResult predictedResult = evaluator.GetResult(record, null);



                if (i == 0)
                {
                    //Get the predicted propability fields
                    predictedCategories = predictedResult.GetPredictedCategories();
                    //Initialize the output table
                    InitializeTable(input.Length, predictedCategories);

                }

                //Add predicted value
                outputTable[i, 0] = predictedResult.PredictedValue;
                i++;
            }

            return outputTable;
        }

        #region Initialize OutputTable

        /// <summary>
        /// Initialize the outputTable
        /// </summary>
        /// <param name="rowCount">rowCount of output table</param>
        /// <param name="predictedfield">predictedfield name</param>
        /// <param name="predictedCategories">probableFields</param>
        private void InitializeTable(int rowCount, string[] predictedCategories)
        {
            //Create instance to hold evaluated results
            outputTable = new Syncfusion.PMML.Table(rowCount, predictedCategories.Length + 1);

            //Add predicted column names
            outputTable.ColumnNames[0] = "Predicted";
        }

        #endregion Initialize OutputTable

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                this.label1.Height = 34;
                this.label1.Width = 1200;

            }
            else
            {
                this.label1.Height = 42;
                this.label1.Width = 895;

            }
        }
    }
}
