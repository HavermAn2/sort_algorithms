using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace New_Project_Algorithms
{
    public partial class Form1 : Form
    {
        private TextBox scalarTextBox;
        private Button generateButton;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private ListBox elementsListBox;
        private ListBox bubbleSortedListBox;
        private ListBox quickSortedListBox;
        private ListBox selectionSortedListBox;
        private Chart chart;

        public Form1()
        {
            InitializeComponent();

            InitializeControls();
        }

        private void InitializeControls()
        {
            scalarTextBox = new TextBox();
            scalarTextBox.Location = new Point(20, 20);
            scalarTextBox.Size = new Size(100, 20);
            scalarTextBox.Name = "scalarTextBox";
            this.Controls.Add(scalarTextBox);

            generateButton = new Button();
            generateButton.Text = "Generate and Sort";
            generateButton.Location = new Point(140, 18);
            generateButton.Click += new EventHandler(this.button1_Click);
            this.Controls.Add(generateButton);

            label1 = new Label();
            label1.Text = "Generated List";
            label1.Location = new Point(170, 75);
            this.Controls.Add(label1);
            elementsListBox = new ListBox();
            elementsListBox.Location = new Point(20, 100);
            elementsListBox.Size = new Size(400, 200);
            elementsListBox.Name = "elementsListBox";
            this.Controls.Add(elementsListBox);

            label2 = new Label();
            label2.Text = "Bubble Sort";
            label2.Location = new Point(620, 75);
            this.Controls.Add(label2);
            bubbleSortedListBox = new ListBox();
            bubbleSortedListBox.Location = new Point(470, 100);
            bubbleSortedListBox.Size = new Size(400, 200);
            bubbleSortedListBox.Name = "bubbleSortedListBox";
            this.Controls.Add(bubbleSortedListBox);

            label3 = new Label();
            label3.Text = "Quick Sort";
            label3.Location = new Point(1070, 75);
            this.Controls.Add(label3);
            quickSortedListBox = new ListBox();
            quickSortedListBox.Location = new Point(920, 100);
            quickSortedListBox.Size = new Size(400, 200);
            quickSortedListBox.Name = "quickSortedListBox";
            this.Controls.Add(quickSortedListBox);

            label4 = new Label();
            label4.Text = "Selection Sort";
            label4.Location = new Point(1520, 75);
            this.Controls.Add(label4);
            selectionSortedListBox = new ListBox();
            selectionSortedListBox.Location = new Point(1370, 100);
            selectionSortedListBox.Size = new Size(400, 200);
            selectionSortedListBox.Name = "selectionSortedListBox";
            this.Controls.Add(selectionSortedListBox);

            chart = new Chart();
            chart.Location = new Point(470, 350);
            chart.Size = new Size(800, 300);
            this.Controls.Add(chart);

            ChartArea chartArea = new ChartArea();
            chart.ChartAreas.Add(chartArea);

            Legend legend = new Legend();
            chart.Legends.Add(legend);

            Series bubbleSeries = new Series
            {
                Name = "BubbleSort",
                ChartType = SeriesChartType.Line,
                Color = Color.Red,
                LegendText = "Bubble Sort"
            };
            chart.Series.Add(bubbleSeries);

            Series quickSeries = new Series
            {
                Name = "QuickSort",
                ChartType = SeriesChartType.Line,
                Color = Color.Blue,
                LegendText = "Quick Sort"
            };
            chart.Series.Add(quickSeries);

            Series selectionSeries = new Series
            {
                Name = "SelectionSort",
                ChartType = SeriesChartType.Line,
                Color = Color.Green,
                LegendText = "Selection Sort"
            };
            chart.Series.Add(selectionSeries);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(scalarTextBox.Text, out int k))
            {
                if (k < 3)
                {
                    MessageBox.Show("Amount of elements must be more than 3.");
                    return;
                }

                List<int> elements = GenerateElements(k);
                elementsListBox.Items.Clear();
                bubbleSortedListBox.Items.Clear();
                quickSortedListBox.Items.Clear();
                selectionSortedListBox.Items.Clear();
                chart.Series["BubbleSort"].Points.Clear();
                chart.Series["QuickSort"].Points.Clear();
                chart.Series["SelectionSort"].Points.Clear();

                foreach (var element in elements)
                {
                    elementsListBox.Items.Add(element);
                }

                List<List<int>> tables = GenerateTables(elements);

                foreach (var table in tables)
                {
                    var bubbleSortedTable = new List<int>(table);
                    int bubbleOperations = BubbleSort(bubbleSortedTable);
                    bubbleSortedListBox.Items.Add(string.Join(", ", bubbleSortedTable));

                    var quickSortedTable = new List<int>(table);
                    int quickOperations = QuickSort(quickSortedTable, 0, quickSortedTable.Count - 1);
                    quickSortedListBox.Items.Add(string.Join(", ", quickSortedTable));

                    var selectionSortedTable = new List<int>(table);
                    int selectionOperations = SelectionSort(selectionSortedTable);
                    selectionSortedListBox.Items.Add(string.Join(", ", selectionSortedTable));

                    chart.Series["BubbleSort"].Points.AddXY(table.Count, bubbleOperations);
                    chart.Series["QuickSort"].Points.AddXY(table.Count, quickOperations);
                    chart.Series["SelectionSort"].Points.AddXY(table.Count, selectionOperations);
                }

                FormatChart(chart);
            }
            else
            {
                MessageBox.Show("Please enter a valid integer.");
            }
        }

        private void FormatChart(Chart chart)
        {
            foreach (var axis in chart.ChartAreas[0].Axes)
            {
                axis.MajorGrid.LineColor = Color.DarkGreen;
                axis.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            }

            chart.ChartAreas[0].AxisX.Title = "Array Size";
            chart.ChartAreas[0].AxisY.Title = "Operation Count";
        }

        static List<int> GenerateElements(int count)
        {
            List<int> elements = new List<int>();
            Random rand = new Random();
            for (int i = 0; i < count; i++)
            {
                elements.Add(rand.Next(1000));
            }
            return elements;
        }

        static List<List<int>> GenerateTables(List<int> elements)
        {
            List<List<int>> tables = new List<List<int>>();
            int tableCount = elements.Count - 3;
            for (int i = 0; i <= tableCount; i++)
            {
                List<int> table = new List<int>();
                for (int j = 0; j < i + 3; j++)
                {
                    table.Add(elements[j]);
                }
                tables.Add(table);
            }
            return tables;
        }

        static int BubbleSort(List<int> list)
        {
            int n = list.Count;
            int count = 0;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (list[j] > list[j + 1])
                    {

                        swap(list, j, j + 1);
                        count++;
                    }
                    count++;
                }
                count++;
            }
            return count;
        }

        static int QuickSort(List<int> list, int left, int right)
        {
            int count = 0;
            if (left < right)
            {
                int pivotIndex = Partition(list, left, right);
                count += QuickSort(list, left, pivotIndex - 1);
                count += QuickSort(list, pivotIndex + 1, right);
            }
            return count;
        }

        static int Partition(List<int> array, int low, int high)
        {
            int pivot = array[high];
            int i = low - 1;
            int operations = 0;

            for (int j = low; j < high; j++)
            {
                operations++;
                if (array[j] < pivot)
                {
                    i++;
                    swap(array, i, j);
                }
            }
            swap(array, i + 1, high);

            return operations;
        }

        static void swap(List<int> array, int i, int j)
        {
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }

        static int SelectionSort(List<int> list)
        {
            int n = list.Count;
            int count = 0;
            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (list[j] < list[minIndex])
                    {
                        minIndex = j;
                    }
                    count++;
                }


                swap(list, minIndex, i);
                count++;
            }
            return count;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
