using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CaffeBar
{
    public partial class FormNewReceipt : Form
    {
        protected DataTable dataTableReceipt;
        public FormNewReceipt()
        {
            InitializeComponent();
            dataTableReceipt = new DataTable();
        }
        #region Initialisation
        private void FormNewReceipt_Load(object sender, EventArgs e)
        {
            SuspendLayout();
            //  Initilise bill 
            dataTableReceipt.Columns.Add("Id", Type.GetType("System.Int32"));
            dataTableReceipt.Columns.Add("Item", Type.GetType("System.String"));
            dataTableReceipt.Columns.Add("Amount", Type.GetType("System.Int32"));
            dataTableReceipt.Columns.Add("Price per unit", Type.GetType("System.Decimal"));
            addItemButtons();

            BindingSource bs = new BindingSource();
            bs.DataSource = dataTableReceipt;

            dataGridView1.DataSource = bs;
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = false;
            dataGridView1.Columns[3].ReadOnly = true;
            comboBoxPaymentMethod.SelectedIndex = 0;
            ResumeLayout();
        }
        #endregion

        private void buttonCloseForm_Click(object sender, EventArgs e)
        {
            dataTableReceipt.Clear();
            this.Close();
        }

        #region Add item to receipt
        // Add item to receipt - button react function
        private void addItemButtons()
        {
            String errorMessage;
            var list = Service.getMenuItems(out errorMessage);

            if (errorMessage != "")
                MessageBox.Show(errorMessage);
            else
            {
                SuspendLayout();
                foreach (var item in list)
                {
                    AddItemButton btn = new AddItemButton();
                    btn.Name = item.Item1.ToString();
                    btn.Id = item.Item1;
                    btn.Text = item.Item2;
                    btn.Item = item.Item2;
                    btn.Click += new EventHandler(buttonAddItemToReceipt_Click);
                    flowLayoutPanelItems.Controls.Add(btn);
                }
                ResumeLayout();
            }
        }
        private void buttonAddItemToReceipt_Click(object sender, EventArgs e)
        {
            var toadd = ((AddItemButton)sender);

            //  Check if item is allredy in receipt
            DataRow[] findItem = dataTableReceipt.Select("Id = '" + toadd.Id + "'");

            if (findItem.Length != 0)
                findItem[0]["Amount"] = ((int)findItem[0]["Amount"]) + 1;
            else
            {
                DataRow newRow = dataTableReceipt.NewRow();
                String errorMessage;

                newRow["Id"] = toadd.Id;
                newRow["Item"] = toadd.Item;
                newRow["Price per unit"] = Service.getPrice(toadd.Id, out errorMessage);
                newRow["Amount"] = 1;

                if (errorMessage == "")
                    dataTableReceipt.Rows.Add(newRow);
                else
                    MessageBox.Show(errorMessage);
            }
        }
        #endregion

        #region Print receipt
        private void buttonPrint_Click(object sender, EventArgs e)
        {
            if( dataTableReceipt.Rows.Count == 0)
            {
                MessageBox.Show("No items added!");
                return;
            }

            //insert into database
            Double total;
            String errorMessage;
            Int32 receiptId;
            errorMessage = Service.newReceipt(dataTableReceipt, comboBoxPaymentMethod.SelectedItem.ToString(), out total, out receiptId);

            if (errorMessage != "")
            {
                MessageBox.Show(errorMessage);
                return;
            }
            // print
            FormReceiptPrint formPrint = new FormReceiptPrint(dataTableReceipt, total, comboBoxPaymentMethod.SelectedItem.ToString(), receiptId);
            formPrint.ShowDialog();

            //check if payed cash - print combo 
            if (comboBoxPaymentMethod.SelectedItem.ToString() == "Gotovina")
            {
                FormChange formChange = new FormChange(in total);
                formChange.ShowDialog();
            }
            dataTableReceipt.Rows.Clear();
        }
        #endregion


        #region Delete item from receipt
        private void buttonDeleteItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataRow[] findItem = dataTableReceipt.Select("Id = '" + dataGridView1.CurrentRow.Cells[0].Value + "'");
                findItem[0].Delete();
            }
            else
                MessageBox.Show("Please select a row!");
        }

        #endregion




        #region New button with properties |id,item |
        class AddItemButton : Button
        {
            public AddItemButton()
            {
                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
                this.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
                this.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(69)))), ((int)(((byte)(76)))));
                this.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(57)))), ((int)(((byte)(63)))));
                this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.ForeColor = System.Drawing.Color.Silver; 
                this.Size = new System.Drawing.Size(100, 30);
                this.Size = new System.Drawing.Size(100, 30);
                this.UseVisualStyleBackColor = false;
            }
            public int Id { get; set; }
            public String Item { get; set; }
        }
        #endregion



    }
}
