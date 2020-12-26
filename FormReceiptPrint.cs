﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
namespace CaffeBar
{
    public partial class FormReceiptPrint : Form
    {
        string _total, _paymentMethod, _receiptId, _date;
        DataTable items;
        public FormReceiptPrint(in DataTable dataTableReceipt,Double total, String paymentMethod,Int32 receiptId)
        {
            InitializeComponent();
            _total = total.ToString();
            _paymentMethod = paymentMethod.ToString();
            _receiptId = receiptId.ToString();
            items = dataTableReceipt.Copy();

        }

        private void FormReceiptPrint_Load(object sender, EventArgs e)
        {
            items.Columns["Price per unit"].ColumnName = "Price_per_unit";
            items.Columns.Add("Total", typeof(System.Double));
            Double amount, price;
            foreach (DataRow row in items.Rows)
            {
                Double.TryParse(row["Amount"].ToString(), out amount); Double.TryParse(row["Price_per_unit"].ToString(), out price);
                row["Total"] = amount * price;
            }

            //
            _date = "1";

            ReportDataSource rds = new ReportDataSource("DataSet2", items);
            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(rds);
            
            //this.reportViewer1.LocalReport.Refresh();
            
            ReportParameter[] para = new ReportParameter[]
            {
                new ReportParameter("parameterTotal",_total),
                new ReportParameter("parameterDate", _date),
                new ReportParameter("parameterDiscount", "0"),
                new ReportParameter("parameterId", _receiptId),
                new ReportParameter("parameterPaymentMethod", _paymentMethod),
            };
            this.reportViewer1.LocalReport.SetParameters(para);
            this.reportViewer1.RefreshReport();
        }
    }
}