using DTCDev.Client.Cars.Controls.ViewModels.Reports;
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

namespace DTCDev.Client.Cars.Controls.Controls.Reports
{
    /// <summary>
    /// Interaction logic for FuelReport.xaml
    /// </summary>
    public partial class FuelReport : UserControl
    {
        FuelReportViewModel vm = new FuelReportViewModel();

        public FuelReport()
        {
            InitializeComponent();
            this.DataContext = vm;
            vm.FuelLoaded += vm_FuelLoaded;
        }

        void vm_FuelLoaded(object sender, EventArgs e)
        {
            DisplayData();
        }

        private void DisplayData()
        {
            stkDates.Children.Clear();
            stkFuel.Children.Clear();
            stkFuelTakes.Children.Clear();
            if (vm.Result == null)
                return;
            if (vm.Result.Report == null)
                return;
            if (vm.Result.Report.Count() < 1)
                return;
            CreateTimeLine();
            CreateData();
            DisplayCharts();
            CalcTaken();
        }

        private void CreateTimeLine()
        {
            days = (vm.DateStop - vm.DateStart).Days;
            stkDates.Width 
                = stkFuel.Width
                = stkFuelTakes.Width
                = brd1.Width
                = brd2.Width
                = brd3.Width
                = brd4.Width
                = brd5.Width
                = brd6.Width
                = days * 1440;
            stkDates.Children.Clear();
            stkFuelTakesTime.Children.Clear();
            stkFuelTime.Children.Clear();
            DateTime dt = vm.DateStart;
            while (dt <= vm.DateStop)
            {
                TextBlock txt = new TextBlock();
                txt.FontWeight = FontWeights.Bold;
                txt.Text = dt.ToString("dd.MM");
                txt.Width = 60;
                stkDates.Children.Add(txt);
                for (int i = 1; i < 24; i++)
                {
                    TextBlock txt1 = new TextBlock();
                    txt1.Text = i.ToString() + ":00";
                    txt1.Width = 60;
                    txt1.FontSize = 11;
                    stkDates.Children.Add(txt1);
                    Border brdr1 = new Border();
                    Border brdr2 = new Border();
                    brdr1.Width = brdr2.Width = 1;
                    brdr1.Height = brdr2.Height = 128;
                    brdr1.Background = brdr2.Background = new SolidColorBrush(Colors.Orange);
                    brdr1.Margin = brdr2.Margin = new Thickness(0, 0, 59, 0);
                    stkFuelTakesTime.Children.Add(brdr1);
                    stkFuelTime.Children.Add(brdr2);
                }
                dt += TimeSpan.FromDays(1);
            }
        }

        List<DTM> _dtm = new List<DTM>();
        int days = 0;

        private void CreateData()
        {
            foreach (var item in vm.Result.Report)
            {
                _dtm.Add(new DTM
                {
                    DT = item.DT.ToDateTime(),
                    Vol = item.Vol
                });
            }
        }

        private void DisplayCharts()
        {
            DateTime dt = vm.DateStart;
            double step = stkFuel.ActualHeight / 100.0f;
            int lastVol = 0;
            for (int d = 0; d < days; d++)
            {
                for (int h = 0; h < 24; h++)
                {
                    for (int m = 0; m < 60; m++)
                    {
                        Border b = new Border
                        {
                            Width = 1,
                            VerticalAlignment = System.Windows.VerticalAlignment.Bottom
                        };
                        DTM model = _dtm.Where(p => p.DT >= dt && p.DT < dt + TimeSpan.FromMinutes(1)).FirstOrDefault();
                        if (model != null)
                        {
                            lastVol = (int)(step * (double)model.Vol);
                            b.Height = lastVol;
                            if (b.Height < 1)
                                b.Height = lastVol = 1;
                            b.Background = new SolidColorBrush(Colors.Blue);
                        }
                        else
                        {
                            b.Height = lastVol;
                            b.Background = new SolidColorBrush(Colors.LightGray);
                        }
                        stkFuel.Children.Add(b);
                        dt += TimeSpan.FromMinutes(1);
                        
                    }
                }
            }
        }

        private void CalcTaken()
        {
            DateTime dt = vm.DateStart;
            double step = stkFuelTakes.ActualHeight / 20.0f;
            int lastVol = 0;
            for (int d = 0; d < days; d++)
            {
                for (int h = 0; h < 24; h++)
                {
                    for (int m = 0; m < 60; m += 10)
                    {
                        Border b = new Border
                                            {
                                                Width = 10,
                                                VerticalAlignment = System.Windows.VerticalAlignment.Bottom
                                            };
                        List<DTM> models = _dtm.Where(p => p.DT >= dt && p.DT < dt + TimeSpan.FromMinutes(10)).ToList();
                        if (models != null)
                        {
                            if (models.Count() > 0)
                            {
                                int total = models[0].Vol - models[models.Count() - 1].Vol;
                                total = total * 6;
                                if (total < 0)
                                    total = 0;
                                if (total > 20)
                                {
                                    total = 20;
                                    b.Background = new SolidColorBrush(Colors.Red);
                                }
                                if (total > 10)
                                {
                                    b.Background = new SolidColorBrush(Colors.Yellow);
                                }
                                else
                                    b.Background = new SolidColorBrush(Colors.Green);

                                lastVol = (int)(step * (double)total);
                                b.Height = lastVol;
                                if (b.Height < 1)
                                    b.Height = lastVol = 1;
                            }
                        }
                        else
                        {
                            b.Height = lastVol;
                            b.Background = new SolidColorBrush(Colors.LightGray);
                        }
                        stkFuelTakes.Children.Add(b);
                        dt += TimeSpan.FromMinutes(10);
                    }
                }
            }
        }



        private class DTM
        {
            public DateTime DT { get; set; }

            public int Vol { get; set; }
        }
    }
}
