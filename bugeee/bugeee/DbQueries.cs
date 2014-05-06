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
        //0 table name 1 = column values need to be set 2 Unique No
        public String updateStatement = "UPDATE {0} SET {1} where sno = {2}";
        public String insertStatement = "Insert {0} SET {1}";
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
        List<String> fieldsList = new List<string>();
        private void UpdateUI(MySqlDataReader Reader)
        {
            for (int i = 0; i < Reader.FieldCount; i++)
            {
                if (!fieldsList.Contains(Reader.GetName(i)))
                {
                    fieldsList.Add(Reader.GetName(i));
                }
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
                CommandText = String.Format("select COUNT(*) from {0} where status = '' or status = 'reopen'", listOfTables[i]);
                MySqlConnection connection = new MySqlConnection(dbConnectionString);
                using (MySqlCommand cmd = new MySqlCommand(CommandText, connection))
                {
                   connection.Open();
                   var retVal = Convert.ToInt32(cmd.ExecuteScalar());
                   Count += retVal;
                   status += String.Format("\t{0}:{1}", listOfTables[i], retVal);
                }
                
            }
            lblStatus.Text = String.Format("Total Open Count = {0}({1})", Count.ToString(), status);
        }

        private bool ExecuteCommand(String CommandText,out String exception)
        {
            exception = "";
            try
            {
                MySqlConnection connection = new MySqlConnection(dbConnectionString);
                MySqlCommand myCommand = new MySqlCommand(CommandText, connection);
                myCommand.Connection.Open();
                myCommand.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                exception = ex.Message;
                return false;
            }
            return true;
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
            String values = ReadFromUI();
            lblStatus.Text = "";
            var commandText = String.Format(updateStatement,comboBox1.SelectedItem.ToString(),values,tb0.Text);
            var exception = "";
            if (!ExecuteCommand(commandText, out exception))
            {
                lblStatus.Text = "failed to update : " + exception;
            }
            else
            {
                lblStatus.Text = "Update Sussces";
            }
        }

        private string ReadFromUI()
        {
            var retValue = "";
            for (int i = 0; i < fieldsList.Count; i++)
            {
                
                   switch (i)
                {
                    case 0:
                         retValue = String.Format("{0}={1}",fieldsList[i],tb0.Text);
                        break;
                    case 1:
                        retValue += String.Format(" , {0}='{1}'", fieldsList[i], tb1.Text);
                        break;
                    case 2:
                        retValue += String.Format(" , {0}='{1}'", fieldsList[i], tb2.Text);
                        break;
                    case 3:
                        retValue += String.Format(" , {0}='{1}'", fieldsList[i], tb3.Text);
                        break;
                    case 4:
                        retValue += String.Format(" , {0}='{1}'", fieldsList[i], rtb4.Text);
                        break;
                    case 5:
                        retValue += String.Format(" , {0}='{1}'", fieldsList[i], tb5.Text);
                        break;
                    case 6:
                        retValue += String.Format(" , {0}='{1}'", fieldsList[i], tb6.Text);
                        break;
                    case 7:
                        retValue += String.Format(" , {0}='{1}'", fieldsList[i], tb7.Text);
                        break;
                    case 8:
                        retValue += String.Format(" , {0}='{1}'", fieldsList[i], rtb8.Text);
                        break;
                    case 9:
                        retValue += String.Format(" , {0}='{1}'", fieldsList[i], tb9.Text);
                        break;
                    default:
                        break;
                }
                    
            }
            return retValue;
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            String values = ReadFromUI();
            lblStatus.Text = "";
            var commandText = String.Format(insertStatement, comboBox1.SelectedItem.ToString(), values);
            var exception = "";
            if (!ExecuteCommand(commandText, out exception))
            {
                lblStatus.Text = "failed to Insert : " + exception;
            }
            else
            {
                lblStatus.Text = "Insert Sussces";
            }
        }
        private void ResetUI()
        {
            this.tb9.Text = "";
            this.tb7.Text = "";
            this.tb6.Text = "";
            this.tb5.Text = "";
            this.tb3.Text = "";
            this.tb2.Text = "";
            this.tb1.Text = "";
            this.tb0.Text = "";
            this.rtb4.Text = "";
            this.rtb8.Text = "";
        }
        private void button1_Click(object sender, EventArgs e)//Insert
        {
            ResetUI();
            btnFetch.Hide();
            btnNext.Hide();
            btnPrevious.Hide();
            btnUpdate.Hide();
            btnCount.Hide();
        }

        private void button2_Click(object sender, EventArgs e)//Update
        {
            ResetUI();
            btnFetch.Show();
            btnNext.Show();
            btnPrevious.Show();
            btnUpdate.Show();
            btnCount.Show();
        }
    }
}
