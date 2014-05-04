using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace bugeee
{
    public partial class DbQueries : Form
    {
        List<String> listOfTables = new List<string>();
        public String dbConnectionString ;
        public String updateStatement = "UPDATE {0} SET {1} where sno = {2}";
        public DbQueries(List<String> tablesList, String connString)
        {
            InitializeComponent();
            listOfTables = tablesList;
            dbConnectionString = connString;
            this.Load += new EventHandler(DbQueries_Load);
        }

        void DbQueries_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(listOfTables.ToArray());
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            int SNo = GetSno();
            SNo--;
            String CommandText = String.Format("select * from {0} where sno = {1} ", comboBox1.SelectedItem.ToString(), SNo.ToString());
            ExecuteQuery(CommandText, true);
        }

        private int GetSno()
        {
            int SNo = 0;
            int.TryParse(tb0.Text.ToString(), out SNo);
            return SNo;
        }

        private void btnFetch_Click(object sender, EventArgs e)
        {
            String CommandText = String.Format("select * from {0} where sno = {1} ", comboBox1.SelectedItem.ToString(), GetSno().ToString());
            ExecuteQuery(CommandText, true);
        }

        private void UpdateUI(MySqlDataReader Reader)
        {
            for (int i = 0; i < Reader.FieldCount; i++)
            {
               
                switch (i)
                {
                    case 0:
                        tb0.Text = Reader.GetValue(i).ToString();
                        break;
                    case 1:
                        tb1.Text = Reader.GetValue(i).ToString();
                        break;
                    case 2:
                        tb2.Text = Reader.GetValue(i).ToString();
                        break;
                    case 3:
                        tb3.Text = Reader.GetValue(i).ToString();
                        break;
                    case 4:
                        rtb4.Text = Reader.GetValue(i).ToString();
                        break;
                    case 5:
                        tb5.Text = Reader.GetValue(i).ToString();
                        break;
                    case 6:
                        tb6.Text = Reader.GetValue(i).ToString();
                        break;
                    case 7:
                        tb7.Text = Reader.GetValue(i).ToString();
                        break;
                    case 8:
                        rtb8.Text = Reader.GetValue(i).ToString();
                        break;
                    case 9:
                        tb9.Text = Reader.GetValue(i).ToString();
                        break;
                    default:
                        break;
                }
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int SNo = GetSno();
            SNo++;
            String CommandText = String.Format("select * from {0} where sno = {1} ", comboBox1.SelectedItem.ToString(), SNo.ToString());
            ExecuteQuery(CommandText, true);
        }

        void btnCount_Click(object sender, System.EventArgs e)
        {
            int Count = 0;
            String status = "";
            String CommandText = "";
            for (int i = 0; i < listOfTables.Count; i++)
            {
                CommandText = String.Format("select * from {0} where status = \"\" ", listOfTables[i]);
                MySqlDataReader Reader = ExecuteQuery(CommandText);
                Count += Reader.FieldCount;
                status += String.Format("\t{0}:{1}", listOfTables[i], Reader.FieldCount);
            }
            lblStatus.Text = String.Format("Total Open Count = {0}({1})", Count.ToString(), status);
        }

        private MySqlDataReader ExecuteQuery(String CommandText,bool updateUI = false)
        {
            MySqlDataReader Reader;
            MySqlConnection connection = new MySqlConnection(dbConnectionString);
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = CommandText;
            connection.Open();
            Reader = command.ExecuteReader();
            Reader.Read();
            if (!Reader.HasRows)
            {
                lblStatus.Text = "No Elements with this query:" + CommandText.Replace("select * from","");
            }
            else if(updateUI)
            {
                UpdateUI(Reader);
                lblStatus.Text = "";
            }
            connection.Close();
            return Reader;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //TODO Update Query
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            //TODO Insert Query
        }

        private void button1_Click(object sender, EventArgs e)//Insert
        {
            btnFetch.Hide();
            btnNext.Hide();
            btnPrevious.Hide();
            btnUpdate.Hide();
            btnCount.Hide();
        }

        private void button2_Click(object sender, EventArgs e)//Update
        {
            btnFetch.Show();
            btnNext.Show();
            btnPrevious.Show();
            btnUpdate.Show();
            btnCount.Show();
        }
    }
}
