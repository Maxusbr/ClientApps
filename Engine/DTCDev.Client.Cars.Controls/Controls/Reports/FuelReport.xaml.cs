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
            //stkDates.Children.Clear();
            //stkFuel.Children.Clear();
            //stkFuelTakes.Children.Clear();
            if (vm.Result == null)
                return;
            if (vm.Result.Report == null)
                return;
            if (vm.Result.Report.Count() < 1)
                return;
            days = (vm.DateStop - vm.DateStart).Days;
            CreateData();
            DisplayCharts();
            CalcParams();
            //CalcTaken();
        }

        List<DTM> _dtm = new List<DTM>();
        int days = 0;

        private void CreateData()
        {
            _dtm.Clear();
            foreach (var item in vm.Result.Report)
            {
                _dtm.Add(new DTM
                {
                    DT = item.DT.ToDateTime(),
                    Vol = item.Vol
                });
            }
            _dtm = _dtm.OrderBy(p => p.DT).ToList();
            if(_dtm.Count()>10)
            {
                double max = _dtm.Max(p => p.Vol);
                int iMax = (int)max;
                int abc = _dtm.Count();
                //remove max values for eraise engine off
                for (int i = 0; i < abc; i++)
                {
                    if(_dtm[i].Vol==iMax)
                    {
                        _dtm.RemoveAt(i);
                        i--;
                        abc--;
                    }
                }
                int col = _dtm.Count()-4;

                

                //commented because there are no more max data
                //for (int i = 3; i < col; i++)
                //{
                //    if(_dtm[i].Vol==(int)max)
                //    {
                //        int prev = _dtm[i-1].Vol;
                //        if (prev != iMax)
                //        {
                //            int c = i;
                //            bool finded = true;
                //            while (c < col && finded)
                //            {
                //                c++;
                //                if (_dtm[c].Vol != iMax)
                //                {
                //                    finded = true;
                //                    if(Math.Abs(_dtm[c].Vol-prev)<10)
                //                    {
                //                        for (int j = i; j < c; j++)
                //                        {
                //                            _dtm.RemoveAt(i);
                //                            col--;
                //                        }
                //                    }
                //                    break;
                //                }

                //            }
                //        }
                //    }
                //    int previos = (_dtm[i - 3].Vol + _dtm[i - 2].Vol + _dtm[i - 1].Vol) / 3;
                //    int next = (_dtm[i + 1].Vol + _dtm[i + 2].Vol + _dtm[i + 3].Vol) / 3;
                //    double pr = previos / max;
                //    double nx = next / max;
                //    if(Math.Abs(pr-nx)<0.1d)
                //    {
                //        if(Math.Abs(_dtm[i].Vol-previos)>3 || Math.Abs(_dtm[i].Vol-next)>3)
                //        {
                //            _dtm.RemoveAt(i);
                //            i--;
                //            col--;
                //        }
                //    }
                //}
                //col = _dtm.Count() - 1;
                //for (int i = 1; i < col; i++)
                //{
                //    _dtm[i].Vol = (_dtm[i - 1].Vol + _dtm[i].Vol + _dtm[i + 1].Vol) / 3;
                //}

                col -= 5;
                iMax = iMax / 15;

                for (int i = 0; i < col; i++)
                {
                    int a = (_dtm[i + 1].Vol + _dtm[i + 2].Vol + _dtm[i + 3].Vol + _dtm[i + 4].Vol + _dtm[i + 5].Vol) / 5;
                    if (Math.Abs(_dtm[i].Vol - a) < iMax)
                    {
                        double delta = (a - _dtm[i].Vol);
                        _dtm[i + 3].Vol = (int)(_dtm[i].Vol + delta);
                        _dtm.RemoveAt(i + 1);
                        _dtm.RemoveAt(i + 2);
                        col -= 4;
                    }
                }
                col -= 5;

                for (int i = 0; i < col; i++)
                {
                    if(Math.Abs(_dtm[i].Vol-_dtm[i+1].Vol)>iMax)
                    {
                        int a = 0;
                        for (int j = i+2; j < i+10; j++)
                        {
                            a += _dtm[j].Vol;
                        }
                        a = a / 8;
                        if(Math.Abs(a-_dtm[i].Vol)<iMax)
                        {
                            _dtm.RemoveAt(i + 1);
                            col--;
                            i--;
                        }
                    }
                }

                col = _dtm.Count() - 20;
                for (int i = 0; i < col; i++)
                {
                    List<int> lIndexes = new List<int>();
                    List<int> hIndexes = new List<int>();
                    for (int j = i + 1; j < i + 18; j++)
                    {
                        if (_dtm[j].Vol > _dtm[i].Vol)
                        {
                            hIndexes.Add(j);
                        }
                        else if (_dtm[j].Vol <= _dtm[i].Vol)
                        {
                            lIndexes.Add(j);
                        }
                    }
                    if(lIndexes.Count>2)
                    {
                        foreach (var item in hIndexes)
                        {
                            if (_dtm.Count > item)
                            {
                                _dtm.RemoveAt(item);
                                col--;
                            }
                        }
                    }
                }

                //var kalman = new KalmanFilterSimple1D(f: 1, h: 1, q: 1, r: 1);
                //kalman.SetState(_dtm[0].Vol, 0.1);
                //for (int i = 1; i < _dtm.Count(); i++)
                //{
                //    kalman.Correct(_dtm[i].Vol);
                //    _dtm[i].Vol = (int)kalman.State;
                //}
            }
        }

        private void DisplayCharts()
        {
            cnvDisplay.Children.Clear();
            int width = 0;
            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.FillRule = FillRule.Nonzero;
            PathFigure figure = new PathFigure();
            figure.StartPoint = new Point(0, 200);
            figure.IsClosed = false;


            DateTime dt = vm.DateStart;
            int lastFuel = 0;

            for (int d = 0; d < days; d++)
            {
                DTM model = _dtm.Where(p => p.DT >= dt && p.DT < dt + TimeSpan.FromDays(1)).FirstOrDefault();
                if (model != null)
                {
                    int DayWidth=0;
                    //Если есть какие то данные за эти сутки
                    for (int h = 0; h < 24; h++)
                    {
                        model = _dtm.Where(p => p.DT >= dt && p.DT < dt + TimeSpan.FromHours(1)).FirstOrDefault();
                        if (model != null)
                        {
                            int hourWidth = 0;
                            for (int m = 0; m < 60; m++)
                            {
                                model = _dtm.Where(p => p.DT >= dt && p.DT < dt + TimeSpan.FromMinutes(1)).FirstOrDefault();
                                if(model!=null)
                                {
                                    lastFuel = 200 - model.Vol * 2;
                                    model.WidthPoint = new Point(width + DayWidth + hourWidth, lastFuel);
                                }
                                else
                                {

                                }
                                hourWidth += 1;
                                LineSegment ls = new LineSegment();
                                ls.Point = new Point(width + DayWidth + hourWidth, lastFuel);
                                figure.Segments.Add(ls);
                                dt += TimeSpan.FromMinutes(1);
                            }

                            //create timeline
                            Border b = new Border();
                            b.Background = new SolidColorBrush(Colors.Gray);
                            b.Width = 1;
                            b.Height = 200;
                            cnvDisplay.Children.Add(b);
                            Canvas.SetLeft(b, width + DayWidth);

                            TextBlock text = new TextBlock();
                            if (h < 10)
                                text.Text = "0" + h.ToString()+":00";
                            else
                                text.Text = h.ToString()+":00";
                            text.TextAlignment = TextAlignment.Center;
                            text.Width = hourWidth;
                            cnvDisplay.Children.Add(text);
                            Canvas.SetLeft(text, width + DayWidth);
                            Canvas.SetTop(text, 210);
                            
                            DayWidth += hourWidth;

                            Border b1 = new Border();
                            b1.Background = new SolidColorBrush(Colors.Gray);
                            b1.Width = 1;
                            b1.Height = 200;
                            cnvDisplay.Children.Add(b1);
                            Canvas.SetLeft(b1, width + DayWidth);
                        }
                        else
                        {
                            TextBlock text = new TextBlock();
                            if (h < 10)
                                text.Text = "0" + h.ToString();
                            else
                                text.Text = h.ToString();
                            text.Width = 25;
                            cnvDisplay.Children.Add(text);
                            Canvas.SetLeft(text, width + DayWidth);
                            Canvas.SetTop(text, 210);
                            DayWidth += 25;
                            dt += TimeSpan.FromHours(1);

                            LineSegment ls = new LineSegment();
                            ls.Point = new Point(width + DayWidth, lastFuel);
                            figure.Segments.Add(ls);
                        }
                    }
                    width = DecorateDay(width, dt, DayWidth);
                    //width+=DayWidth;
                }
                else
                {
                    //нет данных по дню, рисуем пустой прочерк
                    width = DecorateDay(width, dt, 50);
                    dt += TimeSpan.FromDays(1);

                    LineSegment ls = new LineSegment();
                    ls.Point = new Point(width, lastFuel);
                    figure.Segments.Add(ls);
                }
            }

            pathGeometry.Figures.Add(figure);

            Path pth = new Path();
            pth.Stroke = new SolidColorBrush(Colors.Blue);
            pth.StrokeThickness = 2;
            pth.Data = pathGeometry;
            cnvDisplay.Children.Add(pth);
            cnvDisplay.Width = width;

            Border brdrH1 = new Border
            {
                Height = 1,
                Width = width,
                Background = new SolidColorBrush(Colors.Gray)
            };
            Border brdrH2 = new Border
            {
                Height = 1,
                Width = width,
                Background = new SolidColorBrush(Colors.Gray)
            };
            Border brdrH3 = new Border
            {
                Height = 1,
                Width = width,
                Background = new SolidColorBrush(Colors.Gray)
            };
            Border brdrH4 = new Border
            {
                Height = 1,
                Width = width,
                Background = new SolidColorBrush(Colors.Gray)
            };
            Border brdrH5 = new Border
            {
                Height = 1,
                Width = width,
                Background = new SolidColorBrush(Colors.Gray)
            };
            cnvDisplay.Children.Add(brdrH1);
            cnvDisplay.Children.Add(brdrH2);
            cnvDisplay.Children.Add(brdrH3);
            cnvDisplay.Children.Add(brdrH4);
            cnvDisplay.Children.Add(brdrH5);

            Canvas.SetTop(brdrH2, 50);
            Canvas.SetTop(brdrH3, 100);
            Canvas.SetTop(brdrH4, 150);
            Canvas.SetTop(brdrH5, 200);

            //double step = stkFuel.ActualHeight / 100.0f;
            //int lastVol = 0;
            //for (int d = 0; d < days; d++)
            //{
            //    for (int h = 0; h < 24; h++)
            //    {
            //        for (int m = 0; m < 60; m++)
            //        {
            //            Border b = new Border
            //            {
            //                Width = 1,
            //                VerticalAlignment = System.Windows.VerticalAlignment.Bottom
            //            };
            //            DTM model = _dtm.Where(p => p.DT >= dt && p.DT < dt + TimeSpan.FromMinutes(1)).FirstOrDefault();
            //            if (model != null)
            //            {
            //                lastVol = (int)(step * (double)model.Vol);
            //                b.Height = lastVol;
            //                if (b.Height < 1)
            //                    b.Height = lastVol = 1;
            //                b.Background = new SolidColorBrush(Colors.Blue);
            //            }
            //            else
            //            {
            //                b.Height = lastVol;
            //                b.Background = new SolidColorBrush(Colors.LightGray);
            //            }
            //            stkFuel.Children.Add(b);
            //            dt += TimeSpan.FromMinutes(1);
                        
            //        }
            //    }
            //}
        }

        private void CalcParams()
        {
            if (_dtm.Count < 5)
                return;
            int imax = _dtm.Max(p => p.Vol);
            imax = imax / 10;
            int added=0;
            int deleted = 0;
            for (int i = 0; i < _dtm.Count-1; i++)
            {
                if(_dtm[i+1].Vol-_dtm[i].Vol>imax)
                {
                    added += _dtm[i + 1].Vol - _dtm[i].Vol;
                    Border brdr = new Border
                    {
                        Background = new SolidColorBrush(Colors.Green),
                        Opacity = 0.7,
                        Width = 30,
                        Height = 200
                    };
                    TextBlock txt = new TextBlock
                    {
                        Text = (_dtm[i + 1].Vol - _dtm[i].Vol).ToString(),
                        Foreground = new SolidColorBrush(Colors.White),
                        FontWeight = FontWeights.Bold,
                        FontSize = 14,
                        TextAlignment = TextAlignment.Center
                    };
                    brdr.Child = txt;
                    cnvDisplay.Children.Add(brdr);
                    Canvas.SetLeft(brdr, _dtm[i + 1].WidthPoint.X - 15);
                }
                if(_dtm[i].Vol-_dtm[i+1].Vol>imax)
                {
                    deleted += _dtm[i].Vol - _dtm[i + 1].Vol;
                    Border brdr = new Border
                    {
                        Background = new SolidColorBrush(Colors.Red),
                        Opacity = 0.7,
                        Width = 30,
                        Height = 200
                    };
                    TextBlock txt = new TextBlock
                    {
                        Text = (_dtm[i].Vol - _dtm[i + 1].Vol).ToString(),
                        Foreground = new SolidColorBrush(Colors.White),
                        FontWeight = FontWeights.Bold,
                        FontSize = 14,
                        TextAlignment= TextAlignment.Center
                    };
                    brdr.Child = txt;
                    cnvDisplay.Children.Add(brdr);
                    Canvas.SetLeft(brdr, _dtm[i + 1].WidthPoint.X - 15);
                }
            }
            txtAdd.Text = added.ToString();
            txtDown.Text = deleted.ToString();
        }

        private int DecorateDay(int width, DateTime dt, int dayWidth)
        {
            TextBlock text = new TextBlock
            {
                Text = dt.ToString("dd.MM.yyyy"),
                TextAlignment = TextAlignment.Center,
                Width = dayWidth,
                FontSize = 16,
                FontWeight = FontWeights.Bold
            };
            cnvDisplay.Children.Add(text);
            Canvas.SetLeft(text, width);
            Canvas.SetTop(text, 240);
            Border brdr = new Border
            {
                BorderBrush = new SolidColorBrush(Colors.Blue),
                BorderThickness = new Thickness(2, 0, 2, 2),
                Width = dayWidth-2,
                Height = 25
            };
            cnvDisplay.Children.Add(brdr);
            Canvas.SetLeft(brdr, width + 1);
            Canvas.SetTop(brdr, 210);
            width += dayWidth;
            return width;
        }

       


        private class DTM
        {
            public DateTime DT { get; set; }

            public int Vol { get; set; }

            public Point WidthPoint { get; set; }
        }

        class KalmanFilterSimple1D
        {
            public double X0 { get; private set; } // predicted state
            public double P0 { get; private set; } // predicted covariance

            public double F { get; private set; } // factor of real value to previous real value
            public double Q { get; private set; } // measurement noise
            public double H { get; private set; } // factor of measured value to real value
            public double R { get; private set; } // environment noise

            public double State { get; private set; }
            public double Covariance { get; private set; }

            public KalmanFilterSimple1D(double q, double r, double f = 1, double h = 1)
            {
                Q = q;
                R = r;
                F = f;
                H = h;
            }

            public void SetState(double state, double covariance)
            {
                State = state;
                Covariance = covariance;
            }

            public void Correct(double data)
            {
                //time update - prediction
                X0 = F * State;
                P0 = F * Covariance * F + Q;

                //measurement update - correction
                var K = H * P0 / (H * P0 * H + R);
                State = X0 + K * (data - H * X0);
                Covariance = (1 - K * H) * P0;
            }
        }
    }
}
