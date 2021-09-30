using k180303_Q2.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace k180303_Q2
{
    public partial class Login_form : Form
    {
        protected String Email;
        protected String Password;
        protected Dictionary<string, StaffCredentials> authenticatedUser = new Dictionary<string, StaffCredentials>();
        protected StaffCredentials staffCredentials { get; set; } = new StaffCredentials();

        public Login_form()
        {
            InitializeComponent();
            Email = null;
            Password = null;
        }

        private void Email_textBox_TextChanged(object sender, EventArgs e)
        {
            Email = Email_textBox.Text.ToLower();
        }

        private void Password_textBox_TextChanged(object sender, EventArgs e)
        {
            Password = Password_textBox.Text;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (Email == null || Password == null)
            {
                MessageBox.Show("Email or Password Is Missing", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Password_textBox.Clear();
                return;
            }
            else
            {
                bool userExists = authenticatedUser.ContainsKey(Email);

                if(!userExists)
                {
                    MessageBox.Show("Invalid Email Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Email_textBox.Clear();
                    Password_textBox.Clear();
                    return;
                }

                else
                {
                   
                    StaffCredentials staffobject = new StaffCredentials();
                    staffobject = authenticatedUser[Email];

                    bool passwordMatch = staffobject.Password == Password;

                    if (!passwordMatch)
                    {
                        MessageBox.Show("Invalid Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Password_textBox.Clear();
                        return;
                    }

                    else
                    {
                        RecordAVote recordAVote = new RecordAVote(staffobject.StationId);
                        Hide();
                        recordAVote.ShowDialog();
                        
                    }
                }

            }

        }

        private void Login_form_Load(object sender, EventArgs e)
        {

            String path = ConfigurationManager.AppSettings["FilePath"];

            try
            {
                string readText = File.ReadAllText(path);
                string[] userList = readText.Split(Environment.NewLine);

                for(int user=0;user<userList.Length;user++)
                {
                    if(userList[user].Length > 0)
                    {
                        string[] parseUser = userList[user].Split(",");
                        var valueBytes = System.Convert.FromBase64String(parseUser[1]);
                        parseUser[1] = Encoding.UTF8.GetString(valueBytes);

                        //duplicacy check in dictonary
                        bool keyExists = authenticatedUser.ContainsKey(parseUser[0]);
                        if (!keyExists)
                        {
                            staffCredentials.Email = parseUser[0];
                            staffCredentials.Password = parseUser[1];
                            staffCredentials.StationId = parseUser[2];

                            StaffCredentials staff = new StaffCredentials(staffCredentials);
                            authenticatedUser.Add(parseUser[0], staff);
                            parseUser = null;
                        }
                        else
                        {
                            parseUser = null;
                        }
                        
                    }
                    
                }

            }
            catch(NullReferenceException)
            {
                MessageBox.Show("Null Exception!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }

            catch (Exception )
            {
                MessageBox.Show("File Or Directory Not Found!!", "Error 404", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }

        }
    }
}
