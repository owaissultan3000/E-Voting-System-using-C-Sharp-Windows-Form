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
                var gparent = Directory.GetParent(Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName));
                string root = gparent.ToString();
                root += "\\k180303_Q4";
                string sourcedirectoy = ConfigurationManager.AppSettings["PathStation210001"];

                var files = Directory.GetFiles(sourcedirectoy, "*.xml", SearchOption.AllDirectories);

                string[] fileNameParse = files[0].Split("\\");
                string fileName = fileNameParse[4].Substring(0, 21);



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
                output += "\\" + fileName + ".xml";
                root += "\\" + "station1" + ".xml";
                xml1.Save(output);
                xml1.Save(root);


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
                var gparent = Directory.GetParent(Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName));
                string root = gparent.ToString();
                root += "\\k180303_Q4";

                string sourcedirectoy = ConfigurationManager.AppSettings["PathStation210002"];
                var files = Directory.GetFiles(sourcedirectoy, "*.xml", SearchOption.AllDirectories);

                string[] fileNameParse = files[0].Split("\\");
                string fileName = fileNameParse[4].Substring(0, 21);

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
                output += "\\" + fileName + ".xml";
                root += "\\" + "station2" + ".xml";
                xml02.Save(output );
                xml02.Save(root);
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

            if (Directory.Exists(directory))
            {
                Directory.GetFiles(directory).ToList().ForEach(File.Delete);
            }
            else
            {
                Directory.CreateDirectory(directory);
            }
           
            var gparent = Directory.GetParent(Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName));
            string root = gparent.ToString();
            root += "\\k180303_Q4";

            if(Directory.Exists(root))
            {
                DirectoryInfo di = new DirectoryInfo(root);
                FileInfo[] fi = di.GetFiles("*.xml");
                foreach (FileInfo filetemp in fi)
                {
                    filetemp.Delete();
                }
            }
            DataAggregation dataAggregation = new DataAggregation();
            dataAggregation.MergeXMLStation210001();
            dataAggregation.MergeXMLStation210002();
            Console.WriteLine("File Merging Process Has Been Completed");
            return;
    
        }
    }
}
