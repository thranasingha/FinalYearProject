using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IPLab.DataClass;
using System.IO;

namespace IPLab
{
    public partial class ResultForm : Form
    {
        public ResultForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResultForm_Load(object sender, EventArgs e)
        {
            List<ReportData> list = new List<ReportData>();
            list.Add(getResult(25.0));

            DataTable table = ConvertListToDataTable(list);
            dataGridView1.DataSource = table;
        }

        /// <summary>
        /// convert it to a column
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        static DataTable ConvertListToDataTable(List<ReportData> list)
        {
            // New table.
            DataTable table = new DataTable();

            // Add columns.
            table.Columns.Add("Checkerboard result");
            table.Columns.Add("Dythering result");
            table.Columns.Add("Edge varience");
            table.Columns.Add("Final value");
            table.Columns.Add("Conclution");

            // Add rows.
            int i = 0;
            foreach(ReportData data in list)
            {
                string[] array = {
                                     data.checkerboard_result.ToString(),
                                     data.dythering_result.ToString(),
                                     data.edge_varience.ToString(),
                                     data.final_value.ToString(),
                                     data.should_check.ToString()
                                 };
                table.Rows.Add(array);
            }
            return table;
        }

        /// <summary>
        /// ok btn to close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// export the table as csv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_csv_Click(object sender, EventArgs e)
        {
            List<ReportData> list = new List<ReportData>();
            list.Add(getResult(25.0));

            DataTable table = ConvertListToDataTable(list);
            DataTableExtensions.WriteToCsvFile(table, "F:/abc.csv");
        }

        /// <summary>
        /// Reads the files to generate output file
        /// </summary>
        /// <param name="singnatureName">File that are currently analysing</param>
        /// <param name="checkerboardName">file of having checkerboard result</param>
        /// <param name="dytheringName">file of having dythering result</param>
        /// <param name="edgeVerianceName">file of having edge veriance result</param>
        /// <returns></returns>
        private DataClass.ReportData getResult(double threshold)
        {
            ReportData retVal = new ReportData();

            int count = 0; // count for available results

            //get values from the main form
            MainForm mainForm = System.Windows.Forms.Application.OpenForms["MainForm"] as MainForm;
            string[] temp = mainForm.getImageAnalyse();
            retVal.fileName = temp[0];
            retVal.checkerboard_result = string.IsNullOrEmpty(temp[1]) ? 0.0 : Double.Parse(temp[1]);
            retVal.dythering_result = string.IsNullOrEmpty(temp[2]) ? 0.0 : Double.Parse(temp[2]);
            retVal.edge_varience = string.IsNullOrEmpty(temp[3]) ? 0.0 : Double.Parse(temp[3]);

            for (int i = 1; i < 4; i++)
            {
                if (!string.IsNullOrEmpty(temp[i]))
                {
                    count++;
                }
            }

            //calculate values for the final result.
            retVal.final_value = ((string.IsNullOrEmpty(temp[1]) ? 0 :((100/count) * (retVal.checkerboard_result))) + (string.IsNullOrEmpty(temp[2]) ? 0 :((100/count) * (retVal.dythering_result))) + (string.IsNullOrEmpty(temp[1]) ? 0 :((100/count) * (retVal.edge_varience)))) / 100;

            if (retVal.final_value > threshold)
            {
                retVal.should_check = "Analyze of results says this contains more than 30% of suspicious contains. So need Expert support";
            }
            else
            {
                retVal.should_check = "Analyse of results says this contains less than 30% of suspicious contains. So you may proceed";
            }

            return retVal;
        }
    }
}
