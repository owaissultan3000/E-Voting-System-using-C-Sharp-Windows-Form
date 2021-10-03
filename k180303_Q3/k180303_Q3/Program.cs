using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace k180303_Q3
{
    class DataAggregation
    {

        public void MergeXMLStation210001()
        {
            try
            {
                string sourcedirectoy = ConfigurationManager.AppSettings["PathStation210001"];

                var files = Directory.GetFiles(sourcedirectoy, "*.xml", SearchOption.AllDirectories);

                string[] fileNameParse = files[0].Split("\\");
                string fileName = fileNameParse[4].Substring(0, 22);



                var xml1 = XDocument.Load(files[0]);

                if (files.Length > 1)
                {
                    for (int count = 1; count < files.Length; count++)
                    {
                        var newxml = XDocument.Load(files[count]);
                        xml1.Descendants("Votes").LastOrDefault().AddAfterSelf(newxml.Descendants("Votes"));
                    }

                }
                string output = ConfigurationManager.AppSettings["Directory"];
                xml1.Save(output + "\\" + fileName);
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong!!");
                return;
            }
            
         
        }

        public void MergeXMLStation210002()
        {
            try
            {
                string sourcedirectoy = ConfigurationManager.AppSettings["PathStation210002"];
                var files = Directory.GetFiles(sourcedirectoy, "*.xml", SearchOption.AllDirectories);

                string[] fileNameParse = files[0].Split("\\");
                string fileName = fileNameParse[4].Substring(0, 22);

                var xml02 = XDocument.Load(files[0]);

                if (files.Length > 1)
                {
                    for (int count = 1; count < files.Length; count++)
                    {
                        var newxml = XDocument.Load(files[count]);
                        xml02.Descendants("Votes").LastOrDefault().AddAfterSelf(newxml.Descendants("Votes"));
                    }
                }
                string output = ConfigurationManager.AppSettings["Directory"];
                xml02.Save(output + "\\" + fileName);
            }
            catch(Exception)
            {
                Console.WriteLine("Something went wrong!!");
                return;
            }
            
        }
        static void Main(string[] args)
        {
            string directory = ConfigurationManager.AppSettings["Directory"];

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            if(Directory.Exists(directory))
            {
                Directory.GetFiles(directory).ToList().ForEach(File.Delete);
            }
            DataAggregation dataAggregation = new DataAggregation();
            dataAggregation.MergeXMLStation210001();
            dataAggregation.MergeXMLStation210002();
            Console.WriteLine("File Merging Process Has Been Completed");
            return;
    
        }
    }
}
