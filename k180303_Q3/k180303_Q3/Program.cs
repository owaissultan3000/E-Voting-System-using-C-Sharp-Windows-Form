using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace k180303_Q3
{
    class DataAggregation
    {

        public void MergeXML(string sourcedirectory)
        {
            try
            {
                
                string[] files = Directory.GetFiles(sourcedirectory, "*.xml", SearchOption.AllDirectories);
                Dictionary<string, List<string>> creationDate = new Dictionary<string, List<string>>();
                string temp1 = files[0].Split("_")[3];
                for (int i=0;i<files.Length; i++)
                {
                    string temp = files[i].Split("_")[3];
                    if (creationDate.ContainsKey(temp))
                    {
                        var tempList = creationDate[temp];
                        tempList.Add(files[i]);
                        creationDate[temp] = tempList;
                    }
                    else
                    {
                        List<string> data =new List<string>();
                        data.Add(files[i]);
                        creationDate[temp] = data;
                    }
                }

                foreach (string key in creationDate.Keys)
                {
                    
                    List<string> fileList = creationDate[key];
                    string[] filenameparse = fileList[0].Split("\\");
                    string[] temp = filenameparse[4].Split("_");
                    string filename = temp[0] + "_" + temp[1] + "_" + temp[2] + "_" + temp[3];
                    var xml1 = XDocument.Load(fileList[0]);
                    if(fileList.Count > 1)
                    {
                        for (int i = 1; i < fileList.Count; i++)
                        {
                            var newxml = XDocument.Load(fileList[i]);
                             xml1.Descendants("Votes").LastOrDefault().AddAfterSelf(newxml.Descendants("Votes"));
                        }
            
                    }
                    string output = ConfigurationManager.AppSettings["Directory"];
                    output += "\\" + filename + ".xml";
                    xml1.Save(output);

                }

            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong!!");
                System.Environment.Exit(0);
            }
            
        }

        static void Main(string[] args)
        {

            string directory = ConfigurationManager.AppSettings["Directory"];

            if (Directory.Exists(directory))
            {
                Directory.GetFiles(directory).ToList().ForEach(File.Delete);
            }
            else
            {
                Directory.CreateDirectory(directory);
            }
            string sourceDirectory1 = ConfigurationManager.AppSettings["PathStation210001"];
            string sourceDirectory2 = ConfigurationManager.AppSettings["PathStation210002"];

            DataAggregation dataAggregation = new DataAggregation();
            dataAggregation.MergeXML(sourceDirectory1);
            dataAggregation.MergeXML(sourceDirectory2);
            Console.WriteLine("File Merging Process Has Been Completed");
            return;
    
        }
    }
}
