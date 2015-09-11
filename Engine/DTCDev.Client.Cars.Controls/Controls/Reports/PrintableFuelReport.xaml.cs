using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using DTCDev.Client.Cars.Controls.ViewModels.Reports;
using Microsoft.Office.Interop.Excel;
using Border = System.Windows.Controls.Border;
using Point = System.Windows.Point;

namespace DTCDev.Client.Cars.Controls.Controls.Reports
{
    /// <summary>
    /// Логика взаимодействия для PrintableFuelReport.xaml
    /// </summary>
    public partial class PrintableFuelReport : UserControl
    {
        readonly PrintableFuelReportViewModel _vm;
        private readonly DateTime _dt;
        public PrintableFuelReport()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;
            DataContext = _vm = new PrintableFuelReportViewModel();
        }

        public PrintableFuelReport(FuelReportViewModel model, DateTime dt, bool addGraph = true, bool addtable = true )
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;
            _dt = dt;
            _vm = new PrintableFuelReportViewModel(model, dt);
            DisplayData(addGraph, addtable);
        }

        private void DisplayData(bool addGraph, bool addtable)
        {
            if (_vm.Result == null || _vm.Result.Report == null || !_vm.Result.Report.Any())
                return;
            if(addtable)
            {
                grdData.Visibility= Visibility.Visible;
                ItemsData.ItemsSource = _vm.Report;
            }
            tbCarNumber.Text = _vm.CarNumber;
            tbSelectedDate.Text = _vm.SelectedDate;
            CreateData();
            DisplayCharts();
            CalcParams(addGraph);
        }

        List<DTM> _dtm = new List<DTM>();

        private void CreateData()
        {
            _dtm.Clear();
            foreach (var item in _vm.Result.Report)
            {
                _dtm.Add(new DTM
                {
                    DT = item.DT.ToDateTime(),
                    Vol = item.Vol
                });
            }
            _dtm = _dtm.OrderBy(p => p.DT).ToList();
            if (_dtm.Count() <= 10) return;
            double max = _dtm.Max(p => p.Vol);
            var iMax = (int)max;
            var abc = _dtm.Count();
            //remove max values for eraise engine off
            for (var i = 0; i < abc; i++)
            {
                if (_dtm[i].Vol != iMax) continue;
                _dtm.RemoveAt(i);
                i--;
                abc--;
            }
            var col = _dtm.Count() - 4;

            col -= 5;
            iMax = iMax / 15;

            for (var i = 0; i < col; i++)
            {
                var a = (_dtm[i + 1].Vol + _dtm[i + 2].Vol + _dtm[i + 3].Vol + _dtm[i + 4].Vol + _dtm[i + 5].Vol) / 5;
                if (Math.Abs(_dtm[i].Vol - a) >= iMax) continue;
                double delta = (a - _dtm[i].Vol);
                _dtm[i + 3].Vol = (int)(_dtm[i].Vol + delta);
                _dtm.RemoveAt(i + 1);
                _dtm.RemoveAt(i + 2);
                col -= 4;
            }
            col -= 5;

            for (var i = 0; i < col; i++)
            {
                if (Math.Abs(_dtm[i].Vol - _dtm[i + 1].Vol) <= iMax) continue;
                var a = 0;
                for (var j = i + 2; j < i + 10; j++)
                {
                    a += _dtm[j].Vol;
                }
                a = a / 8;
                if (Math.Abs(a - _dtm[i].Vol) >= iMax) continue;
                _dtm.RemoveAt(i + 1);
                col--;
                i--;
            }

            col = _dtm.Count() - 20;
            for (var i = 0; i < col; i++)
            {
                var lIndexes = new List<int>();
                var hIndexes = new List<int>();
                for (var j = i + 1; j < i + 18; j++)
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
                if (lIndexes.Count <= 2) continue;
                foreach (var item in hIndexes.Where(item => _dtm.Count > item))
                {
                    _dtm.RemoveAt(item);
                    col--;
                }
            }
        }

        private void DisplayCharts()
        {
            cnvDisplay.Children.Clear();

            var pathGeometry = new PathGeometry {FillRule = FillRule.Nonzero};
            var figure = new PathFigure
            {
                StartPoint = new Point(0, 200),
                IsClosed = false
            };
            var width = 0;
            var lastFuel = 0;
            var dt = _dt;
            var model = _dtm.FirstOrDefault();
            if (model != null)
            {
                var dayWidth = 0;
                //Если есть какие то данные за эти сутки
                for (var h = 0; h < 24; h++)
                {
                    model = _dtm.FirstOrDefault(p => p.DT >= dt && p.DT < dt + TimeSpan.FromHours(1));
                    if (model != null)
                    {
                        var hourWidth = 0;
                        for (var m = 0; m < 60; m++)
                        {
                            model = _dtm.FirstOrDefault(p => p.DT >= dt && p.DT < dt + TimeSpan.FromMinutes(1));
                            if (model != null)
                            {
                                lastFuel = 200 - model.Vol * 2;
                                model.WidthPoint = new Point(width + dayWidth + hourWidth, lastFuel);
                            }
                            else
                            {

                            }
                            hourWidth += 1;
                            var ls = new LineSegment {Point = new Point(width + dayWidth + hourWidth, lastFuel)};
                            figure.Segments.Add(ls);
                            dt += TimeSpan.FromMinutes(1);
                        }

                        //create timeline
                        var b = new Border
                        {
                            Background = new SolidColorBrush(Colors.Gray),
                            Width = 1,
                            Height = 200
                        };
                        cnvDisplay.Children.Add(b);
                        Canvas.SetLeft(b, width + dayWidth);

                        var text = new TextBlock
                        {
                            TextAlignment = TextAlignment.Center,
                            Width = hourWidth
                        };
                        if (h < 10)
                            text.Text = "0" + h.ToString() + ":00";
                        else
                            text.Text = h.ToString() + ":00";
                        
                        cnvDisplay.Children.Add(text);
                        Canvas.SetLeft(text, width + dayWidth);
                        Canvas.SetTop(text, 210);

                        dayWidth += hourWidth;

                        var b1 = new Border
                        {
                            Background = new SolidColorBrush(Colors.Gray),
                            Width = 1,
                            Height = 200
                        };
                        cnvDisplay.Children.Add(b1);
                        Canvas.SetLeft(b1, width + dayWidth);
                    }
                    else
                    {
                        var text = new TextBlock {Width = 25};
                        if (h < 10)
                            text.Text = "0" + h.ToString();
                        else
                            text.Text = h.ToString();
                        
                        cnvDisplay.Children.Add(text);
                        Canvas.SetLeft(text, width + dayWidth);
                        Canvas.SetTop(text, 210);
                        dayWidth += 25;
                        dt += TimeSpan.FromHours(1);

                        var ls = new LineSegment {Point = new Point(width + dayWidth, lastFuel)};
                        figure.Segments.Add(ls);
                    }
                }
                width = DecorateDay(width, dt - TimeSpan.FromDays(1), dayWidth);
            }
            else
            {
                //нет данных по дню, рисуем пустой прочерк
                width = DecorateDay(width, dt, 50);
                var ls = new LineSegment {Point = new Point(width, lastFuel)};
                figure.Segments.Add(ls);
            }


            pathGeometry.Figures.Add(figure);

            var pth = new Path
            {
                Stroke = new SolidColorBrush(Colors.Blue),
                StrokeThickness = 2,
                Data = pathGeometry
            };
            cnvDisplay.Children.Add(pth);
            cnvDisplay.Width = width;

            var brdrH1 = new Border
            {
                Height = 1,
                Width = width,
                Background = new SolidColorBrush(Colors.Gray)
            };
            var brdrH2 = new Border
            {
                Height = 1,
                Width = width,
                Background = new SolidColorBrush(Colors.Gray)
            };
            var brdrH3 = new Border
            {
                Height = 1,
                Width = width,
                Background = new SolidColorBrush(Colors.Gray)
            };
            var brdrH4 = new Border
            {
                Height = 1,
                Width = width,
                Background = new SolidColorBrush(Colors.Gray)
            };
            var brdrH5 = new Border
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
        }

        private void CalcParams(bool addGraph)
        {
            if (_dtm.Count < 5)
                return;
            var imax = _dtm.Max(p => p.Vol);
            imax = imax / 10;
            var added = 0;
            var deleted = 0;
            if(addGraph)
            for (var i = 0; i < _dtm.Count - 1; i++)
            {
                if (_dtm[i + 1].Vol - _dtm[i].Vol > imax)
                {
                    added += _dtm[i + 1].Vol - _dtm[i].Vol;
                    var brdr = new Border
                    {
                        Background = new SolidColorBrush(Colors.Green),
                        Opacity = 0.7,
                        Width = 30,
                        Height = 200
                    };
                    var txt = new TextBlock
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
                if (_dtm[i].Vol - _dtm[i + 1].Vol > imax)
                {
                    deleted += _dtm[i].Vol - _dtm[i + 1].Vol;
                    var brdr = new Border
                    {
                        Background = new SolidColorBrush(Colors.Red),
                        Opacity = 0.7,
                        Width = 30,
                        Height = 200
                    };
                    var txt = new TextBlock
                    {
                        Text = (_dtm[i].Vol - _dtm[i + 1].Vol).ToString(),
                        Foreground = new SolidColorBrush(Colors.White),
                        FontWeight = FontWeights.Bold,
                        FontSize = 14,
                        TextAlignment = TextAlignment.Center
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
            var text = new TextBlock
            {
                Text = dt.ToString("dd.MM.yyyy"),
                TextAlignment = TextAlignment.Center,
                Width = dayWidth,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Background = new SolidColorBrush(Colors.BlanchedAlmond),
                Tag = dt.ToString("dd.MM.yyyy")
            };
            cnvDisplay.Children.Add(text);
            Canvas.SetLeft(text, width);
            Canvas.SetTop(text, 240);
            var brdr = new Border
            {
                BorderBrush = new SolidColorBrush(Colors.Blue),
                BorderThickness = new Thickness(2, 0, 2, 2),
                Width = dayWidth - 2,
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

        
    }
}
