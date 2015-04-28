using DTCDev.Client.Help.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace DTCDev.Client.Help.Engine
{
    public class ContentWorker
    {
        private string Way = AppDomain.CurrentDomain.BaseDirectory;


        public FileDataModel LoadContent(string file)
        {
            try
            {
                FileDataModel model = new FileDataModel();
                CheckDir();
                XmlSerializer serializer = new XmlSerializer(typeof(FileDataModel));
                StreamReader sr = new StreamReader(Way + "Help\\Content\\"+file+".xml");
                model = (FileDataModel)serializer.Deserialize(sr);
                sr.Close();
                return model;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Невозможно открыть файл страницы " + ex.Message);
            }
            return new FileDataModel();
        }

        public void SaveContent(FileDataModel model)
        {
            try
            {
                CheckDir();
                XmlSerializer serializer = new XmlSerializer(typeof(FileDataModel));
                if (File.Exists(Way + "Help\\Content\\"+model.FileName+".xml"))
                    File.Delete(Way + "Help\\Content\\" + model.FileName + ".xml");
                StreamWriter sw = File.CreateText(Way + "Help\\Content\\" + model.FileName + ".xml");
                serializer.Serialize(sw, model);
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невозможно сохранить файл страницы " + ex.Message);
            }
            CheckDir();
        }

        public BitmapImage GetImage(string name)
        {

            try
            {
                BitmapImage img = new BitmapImage();
                img.BeginInit();
                img.UriSource = new Uri(Way + "Help\\Content\\Images\\" + name);
                img.EndInit();
                return img;
            }
            catch
            {
                return new BitmapImage();
            }
        }

        public string CopyImage(string image)
        {
            try
            {
                FileInfo inf = new FileInfo(image);
                File.Copy(image, Way + "Help\\Content\\Images\\" + inf.Name);
                return inf.Name;
            }
            catch { }
            return "";
        }


        private void CheckDir()
        {
            if (Directory.Exists(Way + "Help") == false)
                Directory.CreateDirectory(Way + "Help");
            if(Directory.Exists(Way+"Help\\Content")==false)
                Directory.CreateDirectory(Way+"Help\\Content");
            if(Directory.Exists(Way+"Help\\Content\\Images")==false)
                Directory.CreateDirectory(Way+"Help\\Content\\Images");
        }
    }
}
