using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace k180303_Q4
{
    public partial class Alumini_Accociation : System.Web.UI.Page
    {
        public Dictionary<string, int> President = new Dictionary<string, int>();
        public Dictionary<string, int> VicePresident = new Dictionary<string, int>();
        public Dictionary<string, int> GeneralSecratery = new Dictionary<string, int>();
        public Dictionary<string, string> CandidateList = new Dictionary<string, string>();

        protected void Page_Load(object sender, EventArgs e)
        {
         
            string candidateList = ConfigurationManager.AppSettings["CandidateList"];

            try
            {
                if (File.Exists(candidateList))
                {
                    char[] delims = new[] { '\r', '\n'};
                    char[] delims2 = new[] { ',' };

                    string candidatesData = File.ReadAllText(candidateList);
                   string[] candidatesList = candidatesData.Split(delims, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < candidatesList.Length; i++)
                    {
                        if (candidatesList[i].Length > 0)
                        {
                            string[] parseCandidate = candidatesList[i].Split(delims2, StringSplitOptions.RemoveEmptyEntries);

                            if(!CandidateList.ContainsKey(parseCandidate[0]))
                            {
                                CandidateList.Add(parseCandidate[0], parseCandidate[1]);
                                parseCandidate = null;
                            }
                            
                        }
                    }
                }
            }

            catch (Exception)
            {
                Console.WriteLine("Something went wrong!!");
                System.Environment.Exit(0);
            }

            string sourcedirectoy = ConfigurationManager.AppSettings["Path"];
            var files = Directory.GetFiles(sourcedirectoy, "*.xml", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                try
                {
                      XmlDocument xDoc = new XmlDocument();
                     xDoc.Load(file);
                    
                    XmlNodeList amounts = xDoc.GetElementsByTagName("Vote");
                    foreach (XmlElement amount in amounts)
                    {
                        string position = amount.ChildNodes[1].InnerText;
                        string id = amount.ChildNodes[2].InnerText;

                        if (position == "President")
                        {
                            if (!President.ContainsKey(CandidateList[id]))
                            {
                                President.Add(CandidateList[id], 1);
                            }
                            else
                            {
                                ++President[CandidateList[id]];
                            }
                        }

                        if (position == "Vice President")
                        {
                            if (!VicePresident.ContainsKey(CandidateList[id]))
                            {
                                VicePresident.Add(CandidateList[id], 1);
                            }
                            else
                            {
                                ++VicePresident[CandidateList[id]];
                            }
                        }

                        if (position == "General Secretary")
                        {
                            if (!GeneralSecratery.ContainsKey(CandidateList[id]))
                            {
                                GeneralSecratery.Add(CandidateList[id], 1);
                            }
                            else
                            {
                                ++GeneralSecratery[CandidateList[id]];
                            }
                        }
                    }

                }
                catch (Exception )
                {
                    Console.WriteLine("Something went wrong!!");
                    System.Environment.Exit(0);
                }
            }
            Page.DataBind();


        }
    }
}