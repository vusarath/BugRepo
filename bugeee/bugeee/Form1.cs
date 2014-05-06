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
    public partial class Form1 : Form
    {
        public String connectionString = "SERVER=localhost;UID='{0}';" + "PASSWORD='{1}'";
        public String dbConnectionString = "SERVER={0};DATABASE={1};UID={2};PASSWORD={3};";
        string myConnectionString = "";
        public Form1()
        {
            InitializeComponent();
            tbAddress.Text = "192.168.1.168";
            tbPassword.Text = "password123";
            tbPort.Text = "3306";
            tbUserName.Text = "root";
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            GetListOfTables();
            DbQueries queries = new DbQueries(tablesList, myConnectionString);
            queries.ShowDialog();
        }

        private void GetListOfTables()
        {
            try
            {
                tablesList = new List<string>();
                myConnectionString = String.Format(dbConnectionString, tbAddress.Text, comboBox1.SelectedItem.ToString().Trim().Replace(",", ""), tbUserName.Text, tbPassword.Text);
                MySqlConnection connection = new MySqlConnection(myConnectionString);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SHOW FULL TABLES;";
                MySqlDataReader Reader;
                connection.Open();
                Reader = command.ExecuteReader();
                while (Reader.Read())
                {
                    string row = "";
                    for (int i = 0; i < Reader.FieldCount; i++)
                    {
                        row += Reader.GetValue(i).ToString();
                        if (!row.Contains("BASE TABLE"))
                        {
                            tablesList.Add(row);
                        }
                    }
                }
                connection.Close();
            }
            catch (Exception)
            {

            }
        }

        List<String> tablesList = new List<string>();
        private void btnFetch_Click(object sender, EventArgs e)
        {
            string myConnectionString = String.Format(connectionString,tbUserName.Text,tbPassword.Text);
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SHOW DATABASES;";
            MySqlDataReader Reader;
            connection.Open();
            Reader = command.ExecuteReader();
            while (Reader.Read())
            {
                string row = "";
                for (int i = 0; i < Reader.FieldCount; i++)
                    row += Reader.GetValue(i).ToString();
                comboBox1.Items.Add(row);
            }
            connection.Close();
        }

        private void btnCount_Click(object sender, EventArgs e)
        {
            GetListOfTables();
            
            int Count = 0;
            String status = "";
            String CommandText = "";
            for (int i = 0; i < tablesList.Count; i++)
            {
                CommandText = String.Format("select COUNT(*) from {0} where status = '' or status = 'reopen'", tablesList[i]);
                MySqlConnection connection = new MySqlConnection(myConnectionString);
                using (MySqlCommand cmd = new MySqlCommand(CommandText, connection))
                {
                    connection.Open();
                    var retVal = Convert.ToInt32(cmd.ExecuteScalar());
                    Count += retVal;
                    status += String.Format("\r\n{0}:{1}", tablesList[i], retVal);
                }

            }
            lblStatus.Show();
            lblStatus.Text = String.Format("Total:{0}{1}", Count.ToString(), status);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetListOfTables();

            int Count = 0;
            String status = "";
            String CommandText = "";
            for (int i = 0; i < tablesList.Count; i++)
            {
                CommandText = String.Format("select COUNT(*) from {0} where status = 'dfixed'", tablesList[i]);
                MySqlConnection connection = new MySqlConnection(myConnectionString);
                using (MySqlCommand cmd = new MySqlCommand(CommandText, connection))
                {
                    connection.Open();
                    var retVal = Convert.ToInt32(cmd.ExecuteScalar());
                    Count += retVal;
                    status += String.Format("\r\n{0}:{1}", tablesList[i], retVal);
                }

            }
            lblStatus.Show();
            lblStatus.Text = String.Format("Total:{0}{1}", Count.ToString(), status);
        }
    }
}
