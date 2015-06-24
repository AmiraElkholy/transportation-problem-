using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        Int32 numOfColumns = 0;
        Int32 numOfRows = 0;

        List<string> _names = new List<string>();
        List<string> _names1 = new List<string>();
        List<string> _names3 = new List<string>();

        List<double[]> _dataArray = new List<double[]>();
        List<double[]> _dataArray1 = new List<double[]>();
        List<double[]> _dataArray3 = new List<double[]>();

        DataTable d = new DataTable();
        DataTable d1 = new DataTable();
        DataTable d2 = new DataTable();
        DataTable d3 = new DataTable();

        double totalCost = 0;
        int numOfUnits = 0;


        List<int> _iList = new List<int>();
        List<int> _jList = new List<int>();
        List<int> _UnitsList = new List<int>();

        // string smax;
        // string snum;
        // string smin;


        public Form1()
        {
            InitializeComponent();
        }

        private void btnGen_Click_1(object sender, EventArgs e)
        {
            numOfColumns = Decimal.ToInt32(numColumn.Value);
            numOfRows = Decimal.ToInt32(numRows.Value);

            for (int j = 1; j <= numOfColumns; j++)
            {
                _names.Add("D" + j);
                _dataArray.Add(new Double[numOfRows]);
            }

            for (int i = 0; i < this._dataArray.Count; i++)
            {
                // The current process name.
                string name = this._names[i];

                // Add the program name to our columns.
                d.Columns.Add(name);

                // Add all of the memory numbers to an object list.
                List<object> objectNumbers = new List<object>();

                //Put every column's numbers in this List.
                foreach (double number in this._dataArray[i])
                {
                    objectNumbers.Add((object)number);
                }

                // Keep adding rows until we have enough.
                while (d.Rows.Count < objectNumbers.Count)
                {
                    d.Rows.Add();
                }

                dgv.DataSource = d;

            }

            // new table 

            for (int j = 1; j <= numOfColumns; j++)
            {
                _names1.Add("V" + j);
                _dataArray1.Add(new Double[1]);
            }

            for (int i = 0; i < this._dataArray1.Count; i++)
            {
                // The current process name.
                string name1 = this._names1[i];

                // Add the program name to our columns.
                d3.Columns.Add(name1);

                // Add all of the memory numbers to an object list.
                List<object> objectNumbers = new List<object>();

                //Put every column's numbers in this List.
                foreach (double number in this._dataArray1[i])
                {
                    objectNumbers.Add((object)number);
                }

                // Keep adding rows until we have enough.
                while (d3.Rows.Count < objectNumbers.Count)
                {
                    d3.Rows.Add();
                }

                dgvDemand.DataSource = d3;

            }


            d1.Columns.Add();

            for (int t = 0; t < numOfRows; t++)
            {
                d1.Rows.Add();
                d1.Rows[t][0] = "S" + (t + 1);
            }

            dgv1.DataSource = d1;


            d2.Columns.Add();

            for (int t = 0; t < numOfRows; t++)
            {
                d2.Rows.Add();
                // d2.Rows[t][0] = "S" + (t + 1);
            }

            dgvSupply.DataSource = d2;

            pnlTable.Visible = true;
            btnGen.Enabled = false;
            btnClr.Enabled = true;
            //pnlCIE.Visible = true;
            //lblName.Visible = true;

            btnCalcMUC.Visible = true;

            numRows.Enabled = false;
            numColumn.Enabled = false;

        }

        private void btnClr_Click(object sender, EventArgs e)
        {

            numOfColumns = 0;
            numOfRows = 0;

            _names = new List<string>();
            _names1 = new List<string>();
            _names3 = new List<string>();

            _dataArray = new List<double[]>();
            _dataArray1 = new List<double[]>();
            _dataArray3 = new List<double[]>();

            d = new DataTable();
            d1 = new DataTable();
            d2 = new DataTable();
            d3 = new DataTable();

            pnlTable.Visible = false;

            btnGen.Enabled = true; ;
            btnClr.Enabled = false;
            btnCalcMUC.Visible = false;
            //pnlCIE.Visible = false;
            //lblName.Visible = false;

            lbl.Visible = false;
            lblTransCost.Visible = false;
            rTxtBoxAns.Visible = false;

            numRows.Enabled = true;
            numColumn.Enabled = true;
        }

        private void btnCalcMUC_Click(object sender, EventArgs e)
        {

            bool Empty = true;

            for (int r = 0; r < numOfRows; r++)
            {
                for (int c = 0; c < numOfColumns; c++)
                {
                    if (d.Rows[r][c] != null && d.Rows[r][c].ToString().Trim() != "")
                    {
                        Empty = false;
                    }
                    else
                    {
                        Empty = true;
                        break;
                    }
                }
            }

            if (Empty == true)
            {
                MessageBox.Show("PLEASE, DON'T LEAVE EMPTY CELLS !", "ERROR , Emtpy Cells !", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (Empty == false)
            {
                _iList = new List<int>();
                _jList = new List<int>();
                _UnitsList = new List<int>();
                totalCost = 0;
                numOfUnits = 0;
                int n = 0;
                int m = 0;
                int def = 0;

                double SumDj = 0;
                double SumSi = 0;

                // Calculate summation Dj
                for (int c = 0; c < numOfColumns; c++)
                {
                    SumDj += double.Parse(d3.Rows[0][c].ToString());
                }
                // Calculate summation Si
                for (int r = 0; r < numOfRows; r++)
                {
                    SumSi += double.Parse(d2.Rows[r][0].ToString());
                }

                // Add Dummy Supply
                if (SumDj > SumSi)
                {
                    d.Rows.Add();
                    for (int c = 0; c < numOfColumns; c++){
                    d.Rows[numOfRows][c] = 0; }
                    d1.Rows.Add();
                    d1.Rows[numOfRows][0] = "S" + (numOfRows+1) + "(Dummy)";
                    d2.Rows.Add();
                    d2.Rows[numOfRows][0] = SumDj - SumSi;
                    numOfRows++;
                }
                // Add Dummy Destination
                else if (SumDj < SumSi)
                {
                    _names3.Add("D" + (numOfColumns+1) + "(Dummy)");
                    _dataArray3.Add(new Double[numOfRows]);

                    for (int i = 0; i < this._dataArray3.Count; i++)
                    {
                        // The current process name.
                        string name = this._names3[i];

                        // Add the program name to our columns.
                        d.Columns.Add(name);

                        // Add all of the memory numbers to an object list.
                        List<object> objectNumbers = new List<object>();

                        //Put every column's numbers in this List.
                        foreach (double number in this._dataArray3[i])
                        {
                            objectNumbers.Add((object)number);
                        }

                        // Keep adding rows until we have enough.
                        while (d.Rows.Count < objectNumbers.Count)
                        {
                            d.Rows.Add();
                        }
                    }  
                    for(int r=0;r<numOfRows;r++){
                    d.Rows[r][numOfColumns]=0; }
                    d3.Columns.Add();
                    d3.Rows[0][numOfColumns] = SumSi - SumDj;
                    numOfColumns++;
                }
                // Solution ..
                    while (n <= numOfRows - 1)
                    {
                        while (m <= numOfColumns - 1)
                        {
                            def = int.Parse(d2.Rows[n][0].ToString()) - int.Parse(d3.Rows[0][m].ToString());
                            if (def > 0)
                            {
                                d2.Rows[n][0] = int.Parse(d2.Rows[n][0].ToString()) - int.Parse(d3.Rows[0][m].ToString());
                                numOfUnits = int.Parse(d3.Rows[0][m].ToString());
                                totalCost += numOfUnits * double.Parse(d.Rows[n][m].ToString());
                                _iList.Add(n + 1);
                                _jList.Add(m + 1);
                                _UnitsList.Add(numOfUnits);
                                //d.Rows[n][m] = 999;
                                d3.Rows[0][m] = 0;
                                m++;
                            }
                            else if (def == 0)
                            {
                                numOfUnits = int.Parse(d3.Rows[0][m].ToString());
                                totalCost += numOfUnits * double.Parse(d.Rows[n][m].ToString());
                                d3.Rows[0][m] = 0;
                                d2.Rows[n][0] = 0;
                                _iList.Add(n + 1);
                                _jList.Add(m + 1);
                                _UnitsList.Add(numOfUnits);
                                //d.Rows[n][m] = 999;
                                n++;
                                m++;
                            }
                            else if (def < 0)
                            {
                                d3.Rows[0][m] = int.Parse(d3.Rows[0][m].ToString()) - int.Parse(d2.Rows[n][0].ToString());
                                numOfUnits = int.Parse(d2.Rows[n][0].ToString());
                                totalCost += numOfUnits * double.Parse(d.Rows[n][m].ToString());
                                _iList.Add(n + 1);
                                _jList.Add(m + 1);
                                _UnitsList.Add(numOfUnits);
                                //d.Rows[n][m] = 999;
                                d2.Rows[n][0] = 0;
                                n++;
                            }
                        }
                    }

                rTxtBoxAns.Text = "\n";

                for (int i = 0; i < _iList.Count; i++)
                {
                    rTxtBoxAns.Text += "  * " + _UnitsList[i].ToString() + " Unit(s) are transported from [ S" + _iList[i].ToString() + " ] to Destination [ D" + _jList[i] + " ] \n\n";
                }

                lblTransCost.Text = "Total Transportation Cost = " + totalCost.ToString() + " .";

                // lblTransPlan.Visible = true;
                //  lblTransPlan1.Visible = true;
                lbl.Visible = true;
                lblTransCost.Visible = true;
                rTxtBoxAns.Visible = true;
            }

        }
        //
        // .... * * Validation Events * * .... 
        //
        private void dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int RowIndex = e.RowIndex;
            int columnIndex = e.ColumnIndex;

            for (int i = 0; i < numOfColumns; i++)
            {
                bool validation = true;
                if (e.ColumnIndex == i)
                {
                    if (dgv.Rows[RowIndex].Cells[columnIndex].Value != null && dgv.Rows[RowIndex].Cells[columnIndex].Value.ToString().Trim() != "")
                    {
                        string DataToValidate = dgv.Rows[RowIndex].Cells[columnIndex].Value.ToString();
                        foreach (char c in DataToValidate)
                        {
                            if (!char.IsDigit(c))
                            {
                                validation = false;
                                break;
                            }

                            else if (char.IsSymbol(c))
                            {
                                validation = false;
                                break;
                            }
                        }

                        if (validation == false)
                        {
                            //dgv.Rows[RowIndex].Cells[columnIndex].ErrorText = "Sorry! This isn't a valid number.";
                            MessageBox.Show("Sorry! This isn't a valid number.", "ERROR , Invalid Value !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            dgv.Rows[RowIndex].Cells[columnIndex].Value = "";
                        }
                    }
                }
            }
        }

        private void dgvSupply_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int RowIndex = e.RowIndex;
            int columnIndex = e.ColumnIndex;

            for (int i = 0; i < numOfColumns; i++)
            {
                bool validation = true;
                if (e.ColumnIndex == i)
                {
                    if (dgvSupply.Rows[RowIndex].Cells[columnIndex].Value != null && dgvSupply.Rows[RowIndex].Cells[columnIndex].Value.ToString().Trim() != "")
                    {
                        string DataToValidate = dgvSupply.Rows[RowIndex].Cells[columnIndex].Value.ToString();
                        foreach (char c in DataToValidate)
                        {
                            if (!char.IsDigit(c))
                            {
                                validation = false;
                                break;
                            }

                            else if (char.IsSymbol(c))
                            {
                                validation = false;
                                break;
                            }
                        }

                        if (validation == false)
                        {
                            //dgv.Rows[RowIndex].Cells[columnIndex].ErrorText = "Sorry! This isn't a valid number.";
                            MessageBox.Show("Sorry! This isn't a valid number.", "ERROR , Invalid Value !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            dgvSupply.Rows[RowIndex].Cells[columnIndex].Value = "";
                        }
                    }
                }
            }
        }

        private void dgvDemand_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int RowIndex = e.RowIndex;
            int columnIndex = e.ColumnIndex;

            for (int i = 0; i < numOfColumns; i++)
            {
                bool validation = true;
                if (e.ColumnIndex == i)
                {
                    if (dgvDemand.Rows[RowIndex].Cells[columnIndex].Value != null && dgvDemand.Rows[RowIndex].Cells[columnIndex].Value.ToString().Trim() != "")
                    {
                        string DataToValidate = dgvDemand.Rows[RowIndex].Cells[columnIndex].Value.ToString();
                        foreach (char c in DataToValidate)
                        {
                            if (!char.IsDigit(c))
                            {
                                validation = false;
                                break;
                            }

                            else if (char.IsSymbol(c))
                            {
                                validation = false;
                                break;
                            }
                        }

                        if (validation == false)
                        {
                            //dgv.Rows[RowIndex].Cells[columnIndex].ErrorText = "Sorry! This isn't a valid number.";
                            MessageBox.Show("Sorry! This isn't a valid number.", "ERROR , Invalid Value !", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            dgvDemand.Rows[RowIndex].Cells[columnIndex].Value = "";
                        }
                    }
                }
            }
        }

    }
}