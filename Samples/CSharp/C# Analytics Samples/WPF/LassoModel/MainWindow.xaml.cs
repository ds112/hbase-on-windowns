using Syncfusion.PMML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LassoModel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Syncfusion.PMML.Table outputTable = null;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var lenth = 0;
            string pmmlPath = "../../Model/lpsa.pmml";
            string inputPath = "../../Model/lpsa.data";
            string[] lines = System.IO.File.ReadAllLines(inputPath);
            outputTable = PredictResult(lines, pmmlPath);
            DataTable inputDataTable = new DataTable();
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
            var MergedDataTable = MergeTable(inputDataTable, outputTable);
            SfDataGrid.ItemsSource = MergedDataTable;
            for (int i = 0; i < MergedDataTable.Columns.Count; i++)
            {
                if (i >= lenth)
                {
                    SfDataGrid.Columns[i].CellStyle = System.Windows.Application.Current.Resources["predictedColumnColor"] as Style;
                }
            }

        }

        private DataTable MergeTable(DataTable inputDataTable, Syncfusion.PMML.Table outputTable)
        {
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

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            btnRestore.Visibility = Visibility.Visible;
            btnMaximize.Visibility = Visibility.Collapsed;
            this.WindowState = WindowState.Maximized;
        }

        private void Minimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            btnRestore.Visibility = Visibility.Collapsed;
            btnMaximize.Visibility = Visibility.Visible;
            this.WindowState = WindowState.Normal;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                textBlock1.Width = 1100;
                btnRestore.Visibility = Visibility.Visible;
                btnMaximize.Visibility = Visibility.Collapsed;
                this.WindowState = WindowState.Maximized;
            }
            else if (this.WindowState == WindowState.Normal)
            {
                textBlock1.Width = 870;
                btnRestore.Visibility = Visibility.Collapsed;
                btnMaximize.Visibility = Visibility.Visible;
                this.WindowState = WindowState.Normal;
            }
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}