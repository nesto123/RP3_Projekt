using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaffeBar
{
    public partial class App : Form
    {
        public App()
        {
            InitializeComponent();
            SuspendLayout();
            costumizeDesign();
            ResumeLayout();
        }

        #region Toggle subMenu.Visible
        private void costumizeDesign()
        {
            panelPlaylistSubmenu.Visible = false;
            panelMediaSubmenu.Visible = false;
        }

        private void hideSubmenu()
        {
            if (panelMediaSubmenu.Visible == true)
                panelMediaSubmenu.Visible = false;
            if (panelPlaylistSubmenu.Visible == true)
                panelPlaylistSubmenu.Visible = false;
        }

        private void showSubMenu(Panel subMenu )
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

        #region MediaSubmenu
        private void buttonMedia_Click(object sender, EventArgs e)
        {
            showSubMenu(panelMediaSubmenu);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openChildForm(new Form1());
            //--
            //Kod
            //--
            hideSubmenu();// premjestit tako da u npr. buttonMedia_Click i ostale -- isto na kraj
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //--
            //Kod
            //--
            hideSubmenu();// premjestit tako da u npr. buttonMedia_Click i ostale -- isto na kraj
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //--
            //Kod
            //--
            hideSubmenu();// premjestit tako da u npr. buttonMedia_Click i ostale -- isto na kraj
        }
        #endregion
        #region PlaylistSubmenu
        private void buttonPlaylistManagment_Click(object sender, EventArgs e)
        {
            showSubMenu(panelPlaylistSubmenu);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //--
            //Kod
            //--
            hideSubmenu();// premjestit tako da u npr. buttonMedia_Click i ostale -- isto na kraj
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //--
            //Kod
            //--
            hideSubmenu();// premjestit tako da u npr. buttonMedia_Click i ostale -- isto na kraj
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //--
            //Kod
            //--
            hideSubmenu();// premjestit tako da u npr. buttonMedia_Click i ostale -- isto na kraj
        }
        #endregion


        #region Help
        private void buttonHelp_Click(object sender, EventArgs e)
        {
            //--
            //Kod
            //--
            hideSubmenu();// premjestit tako da u npr. buttonMedia_Click i ostale -- isto na kraj
        }
        #endregion

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
    }
}
