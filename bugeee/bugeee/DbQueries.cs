using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace bugeee
{
    public partial class DbQueries : Form
    {
        List<String> listOfTables = new List<string>();
        public DbQueries(List<String> tablesList)
        {
            InitializeComponent();
            listOfTables = tablesList;
            this.Load += new EventHandler(DbQueries_Load);
        }

        void DbQueries_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(listOfTables.ToArray());
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {

        }

        private void btnFetch_Click(object sender, EventArgs e)
        {

        }

        private void btnNext_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)//Insert
        {
            btnFetch.Hide();
            btnNext.Hide();
            btnPrevious.Hide();
            btnUpdate.Hide();
            button3.Hide();
        }

        private void button2_Click(object sender, EventArgs e)//Update
        {
            btnFetch.Show();
            btnNext.Show();
            btnPrevious.Show();
            btnUpdate.Show();
            button3.Show();
        }
    }
}
