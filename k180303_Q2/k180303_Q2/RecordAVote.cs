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
                            else if (parseCandidate[2] == "Vice President")
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

            


            string[] President = PresidentComboBox.Text.ToString().Split("(");
            string PresidentID = President[0].Trim();
            string[] VicePresident = VisePresidentComboBox.Text.ToString().Split("(");
            string VicePresidentID = VicePresident[0].Trim();
            string[] GeneralSecretary = GeneralSecretaryComboBox.Text.ToString().Split("(");
            string GeneralSecretaryID = GeneralSecretary[0].Trim();

            if (PresidentID == "Choose President" && VicePresidentID == "Choose Vice President" && GeneralSecretaryID == "Choose General Secretary")
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

            if (PresidentID != "Choose President")
            {
                voteobject.NIC = NIC;
                voteobject.Position = "President";
                voteobject.ID = PresidentID;

                Vote newvote = new Vote(voteobject);
                votes.Add(newvote);

            }
            if (VicePresidentID != "Choose Vice President")
            {
                voteobject.NIC = NIC;
                voteobject.Position = "Vice President";
                voteobject.ID = VicePresidentID;

                Vote newvote = new Vote(voteobject);
                votes.Add(newvote);
            }
            if (GeneralSecretaryID != "Choose General Secretary")
            {
                voteobject.NIC = NIC;
                voteobject.Position = "General Secretary";
                voteobject.ID = GeneralSecretaryID;

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
