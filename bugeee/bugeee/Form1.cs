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
        public Form1()
        {
            InitializeComponent();
            tbAddress.Text = "localhost";
            tbPassword.Text = "password123";
            tbPort.Text = "3306";
            tbUserName.Text = "root";
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            tablesList = new List<string>();
            string myConnectionString = String.Format(dbConnectionString, tbAddress.Text, comboBox1.SelectedItem.ToString().Trim().Replace(",", ""), tbUserName.Text, tbPassword.Text);
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
            DbQueries queries = new DbQueries(tablesList, myConnectionString);
            queries.ShowDialog();
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
    }
}
