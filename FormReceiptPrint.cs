using System;
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
    /// <summary>
    /// Print receipt form.
    /// </summary>
    public partial class FormReceiptPrint : Form
    {
        String _total, _paymentMethod, _receiptId, _date, _discount;
        DataTable items;
        public FormReceiptPrint( DataTable dataTableReceipt,Double total, String paymentMethod,Int32 receiptId, String discount)
        {
            InitializeComponent();
            _total = total.ToString();
            _paymentMethod = paymentMethod.ToString();
            _receiptId = receiptId.ToString();
            _discount = discount;
            items = new DataTable( );
            items = dataTableReceipt.Copy();

        }

        private void FormReceiptPrint_Load(object sender, EventArgs e)
        {
            SuspendLayout();
            items.Columns["Price per unit"].ColumnName = "Price_per_unit";
            items.Columns.Add("Total", typeof(System.Double));
            Double amount, price;
            foreach (DataRow row in items.Rows)
            {
                Double.TryParse(row["Amount"].ToString(), out amount); Double.TryParse(row["Price_per_unit"].ToString(), out price);
                row["Total"] = amount * price;
            }

            //popravit!!!!!!!!!!!!!!!!!!!!!!
            String errorMsg;
            _date = Service.getReceiptDetails((Int32.Parse(_receiptId)), out errorMsg)[1];
            if(errorMsg!="")
                { MessageBox.Show(errorMsg); ResumeLayout(); return; }

            ReportDataSource rds = new ReportDataSource("DataSet2", items);
            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(rds);
                        
            ReportParameter[] para = new ReportParameter[]
            {
                new ReportParameter("parameterTotal",_total),
                new ReportParameter("parameterDate", _date),
                new ReportParameter("parameterDiscount", _discount),
                new ReportParameter("parameterId", _receiptId),
                new ReportParameter("parameterPaymentMethod", _paymentMethod),
                new ReportParameter("paremeterIzdao", User.name),
            };
            this.reportViewer1.LocalReport.SetParameters(para);
            this.reportViewer1.RefreshReport();
            ResumeLayout();
        }
    }
}
