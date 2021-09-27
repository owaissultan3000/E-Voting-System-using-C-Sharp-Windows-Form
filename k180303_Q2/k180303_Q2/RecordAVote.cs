using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace k180303_Q2
{
    
    public partial class RecordAVote : Form
    {
       protected Dictionary<string, string> authenticVoter =  new Dictionary<string, string>();
        protected string StationID;
        protected string NIC;
        public RecordAVote(string stationid)
        {
            InitializeComponent();
            StationID = stationid;
        }

        private void RecordAVote_Load(object sender, EventArgs e)
        {
            string root = @"C:\k180303OutputFiles\Q2";

            // If directory does not exist, create it. 
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            String voterFile = ConfigurationManager.AppSettings["VoterList"];
            try
            {
                if (File.Exists(voterFile))
                {
                    string votersData = File.ReadAllText(voterFile);
                    string[] voterssList = votersData.Split(Environment.NewLine);

                    for (int voter = 0; voter < voterssList.Length; voter++)
                    {
                        if (voterssList[voter].Length > 0)
                        {
                            string[] parseCandidate = voterssList[voter].Split(", ");
                            authenticVoter.Add(parseCandidate[2], parseCandidate[1]);
                            parseCandidate = null;
                        }
                    }
                }
            }

            catch (Exception)
            {
                MessageBox.Show("File Or Directory Not Found!!", "Error 404", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }



            String candidateFile = ConfigurationManager.AppSettings["CandidateList"];
            try
            {
                if (File.Exists(candidateFile))
                {
                    string candidatesData = File.ReadAllText(candidateFile);
                    string[] candidatesList = candidatesData.Split(Environment.NewLine);

                    for (int candidate = 0; candidate < candidatesList.Length; candidate++)
                    {
                        if (candidatesList[candidate].Length > 0)
                        {
                            string[] parseCandidate = candidatesList[candidate].Split(", ");
                            string name = parseCandidate[0] + " ( " + parseCandidate[1] + " )";

                            if (parseCandidate[2] == "President")
                            {
                                
                                PresidentComboBox.Items.Add(name);
                                name = null;
                            }
                            else if(parseCandidate[2] == "Vice President")
                            {
                                VisePresidentComboBox.Items.Add(name);
                                name = null;
                            }

                            else
                            {
                                GeneralSecretaryComboBox.Items.Add(name);
                                name = null;
                            }
                        }
                    }

                }

            }
            catch(Exception)
            {
                MessageBox.Show("File Or Directory Not Found!!", "Error 404", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }

        }
        private void CNICBox_TextChanged(object sender, EventArgs e)
        {
            NIC = CNICBox.Text;
        }

        private void VoteCastingButton_Click(object sender, EventArgs e)
        {
            bool NICconfirmation = authenticVoter.ContainsKey(NIC);
            if (!NICconfirmation)
            {
                MessageBox.Show("Invalid NIC!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CNICBox.Clear();
                return;

            }
            

            string [] President = PresidentComboBox.SelectedItem.ToString().Split("(");
            string PresidentID = President[0];
            string [] VicePresident = VisePresidentComboBox.Text.ToString().Split("(");
            string VicePresidentID = VicePresident[0];
            string [] GeneralSecretary = GeneralSecretaryComboBox.Text.ToString().Split("(");
            string GeneralSecretaryID = GeneralSecretary[0];

            String storeOutputXml = ConfigurationManager.AppSettings["OutputXml"];
            DateTime dt = DateTime.Now;
            string month = dt.ToString("MMM");
           string filepath = storeOutputXml + "//AA_Elec_" + StationID + "_" + dt.Day.ToString() + month + "21_" + dt.Hour.ToString() + "00.xml";
            if(!File.Exists(filepath))
            {
                XDocument voteinfo = new XDocument(new XElement("AllVotes",
                                               new XElement("Votes",
                                               new XElement("Vote",
                                               new XElement("NIC", NIC),
                                               new XElement("Position", "President"),
                                               new XElement("CandidateID", PresidentID.Trim())),

                                               new XElement("Vote",
                                               new XElement("NIC", NIC),
                                               new XElement("Position", "Vice President"),
                                               new XElement("CandidateID", VicePresidentID.Trim())),

                                               new XElement("Vote",
                                               new XElement("NIC", NIC),
                                               new XElement("Position", "General Secretary"),
                                               new XElement("CandidateID", GeneralSecretaryID.Trim()))

                                           )));
                voteinfo.Save(filepath);
            }

            else
            {

                XDocument doc = XDocument.Load(filepath);
                XElement school = doc.Element("AllVotes");
                school.Add(new XElement("Votes",
                                           new XElement("Vote",
                                            new XElement("NIC", NIC),
                                            new XElement("Position", "President"),
                                            new XElement("CandidateID", PresidentID.Trim())),

                                            new XElement("Vote",
                                            new XElement("NIC", NIC),
                                            new XElement("Position", "Vice President"),
                                            new XElement("CandidateID", VicePresidentID.Trim())),

                                            new XElement("Vote",
                                            new XElement("NIC", NIC),
                                            new XElement("Position", "General Secretary"),
                                            new XElement("CandidateID", GeneralSecretaryID.Trim()))



                    ));
                doc.Save(filepath);
              
            }
            
           MessageBox.Show("Your Response Has Been Recorded", "Alumini Accociation Election 2021",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            CNICBox.Clear();
            PresidentComboBox.Text = "";
            VisePresidentComboBox.Text = "";
            GeneralSecretaryComboBox.Text = "";
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            Login_form loginForm = new Login_form();
            Hide();
            loginForm.ShowDialog();
            Close();
        }
    }
}
