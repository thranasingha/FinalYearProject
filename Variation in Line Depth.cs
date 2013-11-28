using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IPLab
{
    public partial class Variation_in_Line_Depth : Form
    {
        public Variation_in_Line_Depth()
        {
            InitializeComponent();
        }

        private void cAlculateVarienceToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Bitmap image=null;

                try
                {
                    // create image document
                    image = new Bitmap(openFileDialog1.FileName);
                    pictureBox1.Image = image;
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

                }
                catch (ApplicationException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //if (imgDoc != null)
                //{
                //    imgDoc.Show(dockManager);
                //    imgDoc.Focus();

                //    // set events
                //    SetupDocumentEvents(imgDoc);
                //}
            }		
        }
    }
}
