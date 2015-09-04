using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Device
{
    public class PosPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public PosPoint() {}
        public PosPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    public class CarLocationModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public PosPoint Location { get; set; }
        public List<PosPoint> Routes { get; set; }
    }
}
