using k180303_Q2.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace k180303_Q2
{

    public partial class RecordAVote : Form
    {

        protected Dictionary<string, string> authenticVoter = new Dictionary<string, string>();
        protected List<Vote> votes = new List<Vote>();
        protected Vote voteobject { get; set; } = new Vote();
        protected string StationID;
        protected string NIC;
        public RecordAVote(string stationid)
        {
            InitializeComponent();
            StationID = stationid;
        }

        private void RecordAVote_Load(object sender, EventArgs e)
        {
           
            string root = ConfigurationManager.AppSettings["OutputXml"];
            string Station210001Files = ConfigurationManager.AppSettings["Station210001"];
            string Station210002Files = ConfigurationManager.AppSettings["Station210002"];

            // If directory does not exist, create it. 
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
                Directory.CreateDirectory(Station210001Files);
                Directory.CreateDirectory(Station210002Files);

            }


            string voterFile = ConfigurationManager.AppSettings["VoterList"];
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



             string candidateFile = ConfigurationManager.AppSettings["CandidateList"];
             Dictionary<string, string> presidentCombo = new Dictionary<string, string>();
             Dictionary<string, string> vicepresidentCombo = new Dictionary<string, string>();
             Dictionary<string, string> generalsecretaryCombo = new Dictionary<string, string>();

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

                            if (parseCandidate[2] == "President")
                            {
                                if(!presidentCombo.ContainsKey(parseCandidate[0]))
                                {
                                    presidentCombo.Add(parseCandidate[0], parseCandidate[1]);
                                }
                           
                              
                            }
                            else if (parseCandidate[2] == "Vice President")
                            {
                                if (!vicepresidentCombo.ContainsKey(parseCandidate[0]))
                                {
                                    vicepresidentCombo.Add(parseCandidate[0], parseCandidate[1]);
                                }
                            
                            }

                            else if(parseCandidate[2] == "General Secretary")
                            {
                                if (!generalsecretaryCombo.ContainsKey(parseCandidate[0]))
                                {
                                    generalsecretaryCombo.Add(parseCandidate[0], parseCandidate[1]);
                                }
                            
                            }
                        }
                    }


                }

                PresidentComboBox.DataSource = new BindingSource(presidentCombo, null);
                PresidentComboBox.DisplayMember = "Value";
                PresidentComboBox.ValueMember = "Key";
                PresidentComboBox.Text = "Choose President";

                VisePresidentComboBox.DataSource = new BindingSource(vicepresidentCombo, null);
                VisePresidentComboBox.DisplayMember = "Value";
                VisePresidentComboBox.ValueMember = "Key";
                VisePresidentComboBox.Text = "Choose Vice President";

                GeneralSecretaryComboBox.DataSource = new BindingSource(generalsecretaryCombo, null);
                GeneralSecretaryComboBox.DisplayMember = "Value";
                GeneralSecretaryComboBox.ValueMember = "Key";
                GeneralSecretaryComboBox.Text = "Choose General Secretary";
            }

            catch (Exception)
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
            if (NIC == "Format: 99999-9999999-9" || NIC == "" || NIC == null)
            {
                MessageBox.Show("Kindly Enter NIC!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            bool NICconfirmation = authenticVoter.ContainsKey(NIC);
            if (!NICconfirmation)
            {
                MessageBox.Show("Invalid NIC!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CNICBox.Clear();
                return;

            }
           

            if (PresidentComboBox.Text.ToString() == "Choose President" && VisePresidentComboBox.Text.ToString() == "Choose Vice President" && GeneralSecretaryComboBox.Text.ToString() == "Choose General Secretary")
            {
                MessageBox.Show("Kindly Choose Atleast One Person !!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string voteCastedList = ConfigurationManager.AppSettings["VoteCastedList"];
            try
            {
                if (!File.Exists(voteCastedList))
                {
                    System.IO.File.WriteAllText(voteCastedList, NIC);
                }
                else
                {
                    string[] votedPeople = File.ReadAllText(voteCastedList).Split(Environment.NewLine);
                    for (int people = 0; people < votedPeople.Length; people++)
                    {
                        if (votedPeople[people] == NIC)
                        {
                            MessageBox.Show("You already have casted your vote!!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            CNICBox.Text = "(Format: 99999-9999999-9)";
                            PresidentComboBox.Text = "Choose President";
                            VisePresidentComboBox.Text = "Choose Vice President";
                            GeneralSecretaryComboBox.Text = "Choose General Secretary";
                            return;
                        }
                    }
                    File.AppendAllText(voteCastedList, Environment.NewLine + NIC);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to create file!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            if (PresidentComboBox.Text.ToString() != "Choose President")
            {
                voteobject.NIC = NIC;
                voteobject.Position = "President";
                voteobject.ID = Convert.ToString(PresidentComboBox.SelectedValue); 

                Vote newvote = new Vote(voteobject);
                votes.Add(newvote);

            }
            if (VisePresidentComboBox.Text.ToString() != "Choose Vice President")
            {
                voteobject.NIC = NIC;
                voteobject.Position = "Vice President";
                voteobject.ID = Convert.ToString(VisePresidentComboBox.SelectedValue); 

                Vote newvote = new Vote(voteobject);
                votes.Add(newvote);
            }
            if (GeneralSecretaryComboBox.Text.ToString() != "Choose General Secretary")
            {
                voteobject.NIC = NIC;
                voteobject.Position = "General Secretary";
                voteobject.ID = Convert.ToString(GeneralSecretaryComboBox.SelectedValue);

                Vote newvote = new Vote(voteobject);
                votes.Add(newvote);
            }


            DateTime dt = DateTime.Now;
            string month = dt.ToString("MMM");

            string Station210001Files = ConfigurationManager.AppSettings["Station210001"];
            string Station210002Files = ConfigurationManager.AppSettings["Station210002"];
            string filepath;
            if (StationID == "210001")
            {
                 filepath = Station210001Files + "//AA_Elec_" + StationID + "_" + dt.Day.ToString() + month + "21_" + dt.Hour.ToString() + "00.xml";
            }
            else
            {
                 filepath = Station210002Files + "//AA_Elec_" + StationID + "_" + dt.Day.ToString() + month + "21_" + dt.Hour.ToString() + "00.xml";
            }
            if (!File.Exists(filepath))
            {
                var doc = new XDocument();
                var rootElement = new XElement("AllVotes");
                doc.Add(rootElement);
                var newElement = new XElement("Votes");

                foreach (var eachvote in votes)
                {

                    var singlevote =
                    new XElement("Vote",
                    new XElement("NIC", eachvote.NIC),
                    new XElement("Position", eachvote.Position),
                    new XElement("CandidateID", eachvote.ID));
                    newElement.Add(singlevote);

                }
                doc.Element("AllVotes").Add(newElement);
                doc.Save(filepath);

              votes.Clear();
               
            }

            else
            {
                var doc = XDocument.Load(filepath);
                var newElement = new XElement("Votes");
                foreach (var eachvote in votes)
                {
                    
                  var singlevote = 
                  new XElement("Vote",
                  new XElement("NIC", eachvote.NIC),
                  new XElement("Position", eachvote.Position),
                  new XElement("CandidateID", eachvote.ID));
                    newElement.Add(singlevote);
                   
                }
                doc.Element("AllVotes").Add(newElement);
                doc.Save(filepath);
                votes.Clear();
            }


            MessageBox.Show("Your Response Has Been Recorded", "Alumini Accociation Election 2021",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            CNICBox.Text = "(Format: 99999-9999999-9)";
            PresidentComboBox.Text = "Choose President";
            VisePresidentComboBox.Text = "Choose Vice President";
            GeneralSecretaryComboBox.Text = "Choose General Secretary";
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
