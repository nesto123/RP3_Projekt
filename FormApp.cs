using System;
using System.Windows.Forms;

namespace CaffeBar
{
    public partial class FormApp : Form
    {
        //[/ <summary>
        /// Main window for application.
        /// </summary>
        public FormApp()
        {
            InitializeComponent();
            SuspendLayout();
            costumizeDesign();
            ResumeLayout();
        }

        #region Toggle subMenu.Visible
        private void costumizeDesign()
        {
            panelManagmentSubmenu.Visible = false;
            panelRegisterSubmenu.Visible = false;
        }

        private void hideSubmenu()
        {
            if (panelRegisterSubmenu.Visible == true)
                panelRegisterSubmenu.Visible = false;
            if (panelManagmentSubmenu.Visible == true)
                panelManagmentSubmenu.Visible = false;
        }

        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                hideSubmenu();
                subMenu.Visible = true;
            }
            else
                subMenu.Visible = false;
        }
        #endregion

        #region Submenu
        private void buttonMedia_Click(object sender, EventArgs e)
        {
            showSubMenu(panelRegisterSubmenu);
        }

        private void buttonNewReceipt_Click(object sender, EventArgs e)
        {
            openChildForm(new FormNewReceipt());
            hideSubmenu();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openChildForm(new FormConsumption());
            hideSubmenu();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openChildForm(new FromReceipts());
            hideSubmenu();
        }
        #endregion
        #region ManagmentSubmenu
        private void buttonPlaylistManagment_Click(object sender, EventArgs e)
        {
            showSubMenu(panelManagmentSubmenu);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            openChildForm(new FormBackStorage());
            hideSubmenu();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            openChildForm(new FormAddToHappyHour());
            hideSubmenu();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            hideSubmenu();
        }
        #endregion


        #region Help
        private void buttonHelp_Click(object sender, EventArgs e)
        {
            openChildForm(new FormHelp());
            hideSubmenu();// premjestit tako da u npr. buttonMedia_Click i ostale -- isto na kraj
        }
        #endregion

        private void buttonAccounts_Click(object sender, EventArgs e)
        {
            hideSubmenu();
        }

        private Form activeForm = null;

        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelChieldForm.Controls.Add(childForm);
            panelChieldForm.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void FormApp_Load(object sender, EventArgs e)
        {
            //User.name = "konobar";
            //User.authorisation = "admin";
            //User.id = 4;
            User.showNotification = true;
            buttonUser.Text = " " + User.name;
            if (User.authorisation.Contains("Konobar"))
            {
                this.buttonAccounts.Dispose();
            }
        }

        private void buttonAccounts_Click_1(object sender, EventArgs e)
        {
            openChildForm(new FormAccounts());
            hideSubmenu();
        }

        private void FormApp_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }


    }
}
