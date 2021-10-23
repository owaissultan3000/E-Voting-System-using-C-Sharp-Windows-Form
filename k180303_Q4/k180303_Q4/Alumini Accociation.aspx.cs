using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
                    char[] delims = new[] { '\r', '\n' };
                    char[] delims2 = new[] { ',' };

                    string candidatesData = File.ReadAllText(candidateList);
                    string[] candidatesList = candidatesData.Split(delims, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < candidatesList.Length; i++)
                    {
                        if (candidatesList[i].Length > 0)
                        {
                            string[] parseCandidate = candidatesList[i].Split(delims2, StringSplitOptions.RemoveEmptyEntries);

                            if (!CandidateList.ContainsKey(parseCandidate[0]))
                            {
                                CandidateList.Add(parseCandidate[0], parseCandidate[1]);
                                
                            }

                            if(parseCandidate[2] == " President" && !President.ContainsKey(parseCandidate[0]))
                            {
                                President.Add(parseCandidate[0], 0);
                            }

                           else if (parseCandidate[2] == " Vice President" && !VicePresident.ContainsKey(parseCandidate[0]))
                            {
                                VicePresident.Add(parseCandidate[0], 0);
                            }

                           else if (parseCandidate[2] == " General Secretary" && !GeneralSecratery.ContainsKey(parseCandidate[0]))
                            {
                                GeneralSecratery.Add(parseCandidate[0], 0);
                            }

                            parseCandidate = null;
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
                            
                                ++President[id];
                            
                        }

                        if (position == "Vice President")
                        {
                           
                                ++VicePresident[id];
                            
                        }

                        if (position == "General Secretary")
                        {
                           
                                ++GeneralSecratery[id];
                            
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Something went wrong!!");
                    System.Environment.Exit(0);
                }
            }
            var items = from pair in President orderby pair.Value descending select pair;
            var sortedDictionary = items.ToDictionary(entry => entry.Key,
                                              entry => entry.Value);
            President.Clear();
            foreach (var item in sortedDictionary)
            {
                string name = item.Key;
                int voteCount = item.Value;
                President.Add(name, voteCount);
            }

            sortedDictionary.Clear();

            items = from pair in VicePresident orderby pair.Value descending select pair;
            sortedDictionary = items.ToDictionary(entry => entry.Key,
                                               entry => entry.Value);
            VicePresident.Clear();
            foreach (var item in sortedDictionary)
            {
                string name = item.Key;
                int voteCount = item.Value;
                VicePresident.Add(name, voteCount);
            }

            sortedDictionary.Clear();

            items = from pair in GeneralSecratery orderby pair.Value descending select pair;
            sortedDictionary = items.ToDictionary(entry => entry.Key,
                                               entry => entry.Value);
            GeneralSecratery.Clear();
            foreach (var item in sortedDictionary)
            {
                string name = item.Key;
                int voteCount = item.Value;
                GeneralSecratery.Add(name, voteCount);
            }

            Page.DataBind();


        }
    }
}