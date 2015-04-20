using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DTCDev.Client.Cars.Engine.Handlers.Cars;

namespace DTCDev.Client.Cars.Controls.Controls.Driver
{
    /// <summary>
    /// Interaction logic for DriverToCar.xaml
    /// </summary>
    public partial class DriverToCar : UserControl
    {
        public DriverToCar()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DriverHandler.Instance.DriverRefreshed += Instance_DriverRefreshed;
            if (DriverHandler.Instance.ListDriver.Count() < 1)
                DriverHandler.Instance.LoadDriver();
            else
                FillDrivers();

            FillCars();
        }

        void Instance_DriverRefreshed(object sender, EventArgs e)
        {
            FillDrivers();
        }

        private void FillDrivers()
        {
            stkDrivers.Children.Clear();
            foreach (var item in DriverHandler.Instance.ListDriver)
            {
                TextBlock txt = new TextBlock();
                txt.Background = new SolidColorBrush(Colors.White);
                txt.Tag = item.Id;
                txt.Text = item.FName + " "+ item.Name[0].ToString() + ". " + item.SName[0].ToString() + ".";
                txt.Margin = new Thickness(4);
                txt.MouseLeftButtonDown += txt_MouseLeftButtonDown;
                stkDrivers.Children.Add(txt);
            }
        }

        Border b;
        int movedID = 0;

        void txt_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            cnvMove.Visibility = Visibility.Visible;
            Point p = e.GetPosition(cnvMove);
            b = new Border();
            b.CornerRadius = new CornerRadius(2);
            b.Width = 120;
            b.Height = 25;
            TextBlock txt = new TextBlock();
            txt.Text = ((TextBlock)sender).Text;
            b.Child = txt;
            b.Background = new SolidColorBrush(Colors.LightBlue);
            cnvMove.Children.Add(b);
            cnvMove.MouseMove += cnvMove_MouseMove;
            cnvMove.MouseLeftButtonUp += cnvMove_MouseLeftButtonUp;
            Canvas.SetLeft(b, p.X);
            Canvas.SetTop(b, p.Y);
            cnvMove.CaptureMouse();
            movedID = (int)((TextBlock)sender).Tag;
        }

        void cnvMove_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(cnvMove);
            Canvas.SetLeft(b, p.X);
            Canvas.SetTop(b, p.Y);
        }

        void cnvMove_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            cnvMove.Children.Clear();
            cnvMove.Visibility = Visibility.Collapsed;
            cnvMove.ReleaseMouseCapture();
            Point p = e.GetPosition(stkCars);
            if (p.X < 1 || p.X > stkCars.ActualWidth || p.Y < 1 || p.Y > stkCars.ActualHeight)
                return;
            else
            {
                int pos = (int)p.Y / 25;
                if (CarsHandler.Instance.Cars.Count() > pos)
                {
                    CarsHandler.Instance.SetDriverToCar(CarsHandler.Instance.Cars[pos].ID, movedID);
                    FillCars();
                }
            }
        }

        private void FillCars()
        {
            stkCars.Children.Clear();
            foreach (var item in CarsHandler.Instance.Cars)
            {
                StackPanel panel = new StackPanel();
                panel.Height = 25;
                panel.Orientation = Orientation.Horizontal;
                TextBlock txtCar = new TextBlock();
                txtCar.Text = item.Car.CarNumber;
                txtCar.Margin = new Thickness(4);
                txtCar.Width=120;
                panel.Children.Add(txtCar);
                Border b = new Border();
                b.Width = 120;
                b.BorderBrush = new SolidColorBrush(Colors.LightGray);
                b.CornerRadius = new CornerRadius(2);
                b.Margin = new Thickness(2);
                TextBlock txtDriver = new TextBlock();
                txtDriver.Margin = new Thickness(2);
                if (item.Driver == null)
                {
                    txtDriver.Text = "Не указан";
                }
                else
                {
                    txtDriver.Text = item.Driver.FName + " " + item.Driver.Name[0].ToString() + ". " + item.Driver.SName[0].ToString() + ".";
                }
                b.Child = txtDriver;
                panel.Children.Add(b);
                stkCars.Children.Add(panel);
            }
        }




        public class CarModel
        {
            public int ID { get; set; }

            public string Number { get; set; }

            public string DriverName { get; set; }

            public int DrID { get; set; }

        }
    }
}
