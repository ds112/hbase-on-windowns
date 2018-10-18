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

namespace LinearRegressionModel
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
            var lenth = 0;
            string pmmlPath = "../../Model/lpsa.pmml";
            string inputPath = "../../Model/lpsa.data";
            string[] lines = System.IO.File.ReadAllLines(inputPath);
            outputTable = PredictResult(lines, pmmlPath);
            string[] inputValues = lines[0].Split(' ');
            string[] fieldAndTarget = inputValues[0].Split(',');
            lenth = inputValues.Length;
            for (int i = 0; i < lenth; i++)
            {
                if (i == 0)
                {
                    inputDataTable.Columns.Add("target");
                }
                inputDataTable.Columns.Add("field_" + i);
            }
            foreach (var item in lines)
            {
                fieldAndTarget = item.Split(',');
                var inputList = new List<string>();
                inputList.Add(fieldAndTarget[0]);
                inputValues = fieldAndTarget[1].Split(' ');
                foreach (var word in inputValues)
                {
                    inputList.Add(word);
                }

                inputValues = inputList.ToArray<string>();
                lenth = inputList.Count;
                inputDataTable.Rows.Add(inputValues);
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

        /// <summary>
        /// Predicts the results for given PMML and CSV file and serialize the results in a CSV file
        /// </summary>
        public Syncfusion.PMML.Table PredictResult(string[] input, string pmmlPath)
        {
            //Get PMML Evaluator instance
            PMMLEvaluator evaluator = new PMMLEvaluatorFactory().
              GetPMMLEvaluatorInstance(pmmlPath);

            string[] predictedCategories = null;
            int i = 0;
            foreach (string s in input)
            {
                string[] predictedandInput = s.Split(',');

                string[] inputField = predictedandInput[1].Split(' ');

                var record = new
                {
                    field_0 = inputField[0],
                    field_1 = inputField[1],
                    field_2 = inputField[2],
                    field_3 = inputField[3],
                    field_4 = inputField[4],
                    field_5 = inputField[5],
                    field_6 = inputField[6],
                    field_7 = inputField[7]
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
                outputTable[i, 0] = predictedResult.PredictedDoubleValue;
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
