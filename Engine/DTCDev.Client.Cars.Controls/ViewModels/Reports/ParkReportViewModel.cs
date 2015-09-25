using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using DTCDev.Client.Cars.Engine.Handlers.Cars;
using DTCDev.Client.ViewModel;
using Microsoft.Office.Interop.Excel;

namespace DTCDev.Client.Cars.Controls.ViewModels.Reports
{
    public class ParkReportViewModel : ViewModelBase
    {
        private readonly ObservableCollection<ParkReportLine> _reportLines = new ObservableCollection<ParkReportLine>();
        private readonly ReportsHandler _handler;

        public ParkReportViewModel()
        {
            _handler = ReportsHandler.Instance;

            CarsHandler.Instance.CarsRefreshed += Instance_CarsRefreshed;
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;
            CreateReport();
        }

        private void Instance_CarsRefreshed(IEnumerable<Engine.DisplayModels.DISP_Car> data)
        {
            CreateReport();
        }

        private void CreateReport()
        {
            Date = string.Format("Парковый отчет за период: с {0} по {1}", DateTime.Now.AddDays(-7).ToShortDateString(),
                DateTime.Now.ToShortDateString());
            foreach (var car in CarsHandler.Instance.Cars)
            {
                var value = new Random();
                var dt = value.NextDouble();
                var dd = value.NextDouble();

                ReportLines.Add(new ParkReportLine
                {
                    DID = car.Name, Start = DateTime.Now.AddDays(-7), End = DateTime.Now, Distance =(int) (795 * dt), Odometr = (int)(546846 * dd),
                    DriverSupport = new ChangedValue { OldValue =60* value.NextDouble(), Value = 60*value.NextDouble(), UsePercent = true},
                    NakatValue = new ChangedValue { OldValue = 18 * value.NextDouble(), Value = 18 * value.NextDouble(), UsePercent = true },
                    Idling  = new ChangedValue { OldValue = 7 * value.NextDouble(), Value = 7 * value.NextDouble(), UsePercent = true }, 
                    Speeding = new ChangedValue { OldValue = 29 * value.NextDouble(), Value = 29 * value.NextDouble(), UsePercent = true },
                    HardBrakes = new ChangedValue { OldValue = dt, Value = dt, UsePercent = false },
                    FuelRate = new ChangedValue { OldValue = 25 * dd, Value = 25 * dd, UsePercent = false }
                });
                Thread.Sleep(10);
            }
            var midNakatValue = ReportLines.Average(o => o.NakatValue.Value);
            var midIdling = ReportLines.Average(o => o.Idling.Value);
            var midSpeeding = ReportLines.Average(o => o.Speeding.Value);
            var midHardBrakes = ReportLines.Average(o => o.HardBrakes.Value);
            var midFuelRate = ReportLines.Average(o => o.FuelRate.Value);
            MidNakatValue = new ChangedValue { OldValue =midNakatValue, Value = midNakatValue, UsePercent = true};
            MidIdling = new ChangedValue { OldValue = midIdling, Value = midIdling, UsePercent = true };
            MidSpeeding = new ChangedValue { OldValue = midSpeeding, Value = midSpeeding, UsePercent = true };
            MidHardBrakes = new ChangedValue { OldValue = midHardBrakes, Value = midHardBrakes, UsePercent = false };
            MidFuelRate = new ChangedValue { OldValue = midFuelRate, Value = midFuelRate, UsePercent = false };
        }
        public string Date { get; set; }

        public ObservableCollection<ParkReportLine> ReportLines
        {
            get { return _reportLines; }
        }
        /// <summary>
        /// Среднее значение движения накатом
        /// </summary>
        public ChangedValue MidNakatValue { get; set; }
        /// <summary>
        /// Среднее значение холостого хода
        /// </summary>
        public ChangedValue MidIdling { get; set; }
        /// <summary>
        /// Среднее значение превышения скорости
        /// </summary>
        public ChangedValue MidSpeeding { get; set; }
        /// <summary>
        /// Среднее значение жесткого отрможения
        /// </summary>
        public ChangedValue MidHardBrakes { get; set; }
        /// <summary>
        /// Среднее значение расхода топлива
        /// </summary>
        public ChangedValue MidFuelRate { get; set; }
    }

    public class ParkReportLine
    {
        /// <summary>
        /// Номер автомобиля
        /// </summary>
        public string DID { get; set; }

        public DateTime Start { get; set; }
        /// <summary>
        /// Начало
        /// </summary>
        public string StartDate { get { return Start.ToLongDateString(); } }

        public DateTime End { get; set; }
        /// <summary>
        /// Окончание
        /// </summary>
        public string EndDate { get { return End.ToLongDateString(); } }
        /// <summary>
        /// Одометр
        /// </summary>
        public int Odometr { get; set; }
        /// <summary>
        /// Расстояние
        /// </summary>
        public int Distance { get; set; }
        /// <summary>
        /// Система поддержки водителей
        /// </summary>
        public ChangedValue DriverSupport { get; set; }
        /// <summary>
        /// Движение накатом
        /// </summary>
        public ChangedValue NakatValue { get; set; }
        /// <summary>
        /// Режим холостого хода
        /// </summary>
        public ChangedValue Idling { get; set; }
        /// <summary>
        /// Превышение скорости
        /// </summary>
        public ChangedValue Speeding { get; set; }
        /// <summary>
        /// Жесткое торможение
        /// </summary>
        public ChangedValue HardBrakes { get; set; }
        /// <summary>
        /// Расход топлива
        /// </summary>
        public ChangedValue FuelRate { get; set; }
    }

    public class ChangedValue
    {
        private double _oldValue;
        private double _value;
        public ChangedValue() {}

        public double Value
        {
            get { return _value; }
            set { _value = Math.Round(value, 1); }
        }

        public double OldValue
        {
            get { return _oldValue; }
            set { _oldValue = Math.Round(value, 1); }
        }

        public bool ChangedUp { get { return Value > OldValue; } }

        public bool ChangedDn { get { return Value < OldValue; } }

        public string Percent { get { return string.Format("{0}%", Value); } }

        public bool UsePercent { get; set; }

        public string StringValue { get { return UsePercent ? Percent : Value.ToString(); } }

        public override string ToString()
        {
            return UsePercent ? Percent : Value.ToString();
        }
    }
}
