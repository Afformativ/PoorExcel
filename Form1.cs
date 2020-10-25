using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace PoorExcel
{
    public partial class Form1 : Form
    {
        Table table = new Table();
       
        public Form1()
        {
            InitializeComponent();
            InitializeDataGridView(Table.defaultRow,Table.defaultCol);

        }
        private void InitializeDataGridView(int rows,int columns)
        {
            
            dataGridView1.ColumnHeadersVisible = true;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.ColumnCount = columns;
            for(int i = 0; i < columns; i++)
            {
                string columnName = NumberConverter.To26System(i);
                dataGridView1.Columns[i].Name = columnName;
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            for(int i = 0; i < rows; i++)
            {
                dataGridView1.Rows.Add("");
                dataGridView1.Rows[i].HeaderCell.Value = (i).ToString();
            }
            dataGridView1.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
            table.setTable(columns, rows);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int col = dataGridView1.SelectedCells[0].ColumnIndex;
            int row = dataGridView1.SelectedCells[0].RowIndex;
            string expression = Table.grid[row][col].expression;
            string value = Table.grid[row][col].value;
            textBox1.Text = expression;
            textBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int col = dataGridView1.SelectedCells[0].ColumnIndex;
            int row = dataGridView1.SelectedCells[0].RowIndex;
            string expression = textBox1.Text;
            if (expression == "") return;
            table.ChangeCellWithAllPointers(row, col, expression, dataGridView1);
            dataGridView1[col, row].Value = Table.grid[row][col].value;
        }

        private void buttonAddRow_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = new System.Windows.Forms.DataGridViewRow();
            if (dataGridView1.Columns.Count == 0)
            {
                MessageBox.Show("Нема стовбців!");
                return;
            }
            dataGridView1.Rows.Add(row);
            dataGridView1.Rows[table.rowCount].HeaderCell.Value = (table.rowCount).ToString();
            table.AddRow(dataGridView1);
        }

        private void buttonDeleteRow_Click(object sender, EventArgs e)
        {
            if (!table.DeleteRow(dataGridView1))
                return;
            dataGridView1.Rows.RemoveAt(table.rowCount);
        }

        private void buttonAddColumn_Click(object sender, EventArgs e)
        {
            string name = NumberConverter.To26System(table.colCount);
            dataGridView1.Columns.Add(name, name);
            table.AddColumn(dataGridView1);
        }

        private void buttonDeleteColumn_Click(object sender, EventArgs e)
        {
            if (!table.DeleteColumn(dataGridView1))
                return;
            dataGridView1.Columns.RemoveAt(table.colCount);
            

        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "TableFile|*.txt";
            openFileDialog.Title = "Open TableFile";
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            StreamReader sr = new StreamReader(openFileDialog.FileName);
            table.Clear();
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            int row;
            int column;
            Int32.TryParse(sr.ReadLine(), out row);
            Int32.TryParse(sr.ReadLine(), out column);
            InitializeDataGridView(row, column);
            table.Open(row, column, sr, dataGridView1);
            sr.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "TableFile|*.txt";
            saveFileDialog.Title = "save table file";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != "")
            {
                FileStream fs = (FileStream)saveFileDialog.OpenFile();
                StreamWriter sw = new StreamWriter(fs);
                table.Save(sw);
                sw.Close();
                fs.Close();
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult res = MessageBox.Show("Зберегти таблицю перед виходом?", "Message", MessageBoxButtons.YesNoCancel);
            if (res==DialogResult.Yes)
            {
                e.Cancel = false;
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "TableFile|*.txt";
                saveFileDialog.Title = "save table file";
                saveFileDialog.ShowDialog();
                if (saveFileDialog.FileName != "")
                {
                    FileStream fs = (FileStream)saveFileDialog.OpenFile();
                    StreamWriter sw = new StreamWriter(fs);
                    table.Save(sw);
                    sw.Close();
                    fs.Close();
                }
            }

            if (res == DialogResult.No)
            {
                e.Cancel = false;
            }
            if (res == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            const string helpMess = "Це програма аналог Microsoft Excel.\n" +
                                    "Щоб все правильно рахувалось потрібно вписувати вираз в ділянку зверху\n" +
                                    "Вираз має бути в дужках або починатися з пробілу \n" +
                                    "Доступні такі операції +,-,/,*,>,<,=,mmax(),mmin(),not()";
                                  
            MessageBox.Show(helpMess, "Допомога", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


    }
}
