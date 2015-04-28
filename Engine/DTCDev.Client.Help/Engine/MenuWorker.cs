using DTCDev.Client.Help.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace DTCDev.Client.Help.Engine
{
    public class MenuWorker
    {
        List<HelpMenuModel> doc = new List<HelpMenuModel>();

        private string Way = AppDomain.CurrentDomain.BaseDirectory;

        public List<HelpMenuModel> DOC
        {
            get { return doc; }
            set { doc = value; }
        }

        public void LoadMenu()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<HelpMenuModel>));
                StreamReader sr = new StreamReader(Way+"Help\\HelpMenu.xml");
                doc = (List<HelpMenuModel>)serializer.Deserialize(sr);
                sr.Close();
                doc = doc.OrderBy(p => p.Header).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невозможно открыть файл дерева " + ex.Message);
            }
        }

        public void SaveMenu()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<HelpMenuModel>));
                if (File.Exists(Way + "Help\\HelpMenu.xml"))
                    File.Delete(Way + "Help\\HelpMenu.xml");
                StreamWriter sw = File.CreateText(Way + "Help\\HelpMenu.xml");
                serializer.Serialize(sw, doc);
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невозможно сохранить файл дерева " + ex.Message);
            }

        }
    }
}
