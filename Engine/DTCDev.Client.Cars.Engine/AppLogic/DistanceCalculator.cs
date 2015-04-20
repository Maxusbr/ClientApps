using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Client.Controls.Map;
using DTCDev.Models.CarsSending.Car;

namespace DTCDev.Client.Cars.Engine.AppLogic
{
    public class DistanceCalculator
    {
        public double Calculate(double lat1, double lat2, double lon1, double lon2)
        {
            double R = 6371;
            double dLat = ToRad(lat2 - lat1);
            double dLon = ToRad(lon2 - lon1);

            double latR1 = ToRad(lat1);
            double latR2 = ToRad(lat2);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(latR1) * Math.Cos(latR2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;

        }

        //public double Calculate(CarStateModel first, CarStateModel item)
        //{
        //    double lat1 = first.Lt/10000.0, lat2 = item.Lt/10000.0, lon1 = first.Ln/10000.0, lon2 = item.Ln/10000.0;
        //    double R = 6371;
        //    double dLat = ToRad(lat2 - lat1);
        //    double dLon = ToRad(lon2 - lon1);

        //    double latR1 = ToRad(lat1);
        //    double latR2 = ToRad(lat2);

        //    double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(latR1) * Math.Cos(latR2);

        //    double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        //    return R * c;

        //}

        public double Calculate(CarStateModel first, CarStateModel item)
        {
            double deltaLatitude = first.Lt / 10000.0 - item.Lt / 10000.0;
            double deltaLongitude = first.Ln / 10000.0 - item.Ln / 10000.0;
            double kmLat = Math.Round(deltaLatitude * 111.1111, 1);
            double kmLon = Math.Round(deltaLongitude * 111.1111 * GetCos((int)item.Ln), 1);
            //km x and lm y

            //Find distance:
            double dist = Math.Sqrt(kmLat * kmLat + kmLon * kmLon);

            return dist;

        }

        public double Calculate(Location first, Location second)
        {
            double lat1 = first.Latitude, lat2 = second.Latitude, lon1 = first.Longitude, lon2 = second.Longitude;
            double R = 6371;
            double dLat = ToRad(lat2 - lat1);
            double dLon = ToRad(lon2 - lon1);

            double latR1 = ToRad(lat1);
            double latR2 = ToRad(lat2);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(latR1) * Math.Cos(latR2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;

        }

        private double ToRad(double deg)
        {
            return deg * Math.PI / 180;
        }


        private double GetCos(int angle)
        {
            switch (angle)
            {
                case 1:
                    return 0.9998;
                case 2: return 0.9994;
                case 3: return 0.9986;
                case 4: return 0.9976;
                case 5: return 0.9962;
                case 6: return 0.9945;
                case 7: return 0.9925;
                case 8: return 0.9903;
                case 9: return 0.9877;
                case 10: return 0.9848;
                case 11: return 0.9816;
                case 12: return 0.9781;
                case 13: return 0.9744;
                case 14: return 0.9703;
                case 15: return 0.9659;
                case 16: return 0.9613;
                case 17: return 0.9563;
                case 18: return 0.9511;
                case 19: return 0.9455;
                case 20: return 0.9397;
                case 21: return 0.9336;
                case 22: return 0.9272;
                case 23: return 0.9205;
                case 24: return 0.9135;
                case 25: return 0.9063;
                case 26: return 0.8988;
                case 27: return 0.891;
                case 28: return 0.8829;
                case 29: return 0.8746;
                case 30: return 0.866;
                case 31: return 0.8572;
                case 32: return 0.848;
                case 33: return 0.8387;
                case 34: return 0.829;
                case 35: return 0.8192;
                case 36: return 0.809;
                case 37: return 0.7986;
                case 38: return 0.788;
                case 39: return 0.7771;
                case 40: return 0.766;
                case 41: return 0.7547;
                case 42: return 0.7431;
                case 43: return 0.7314;
                case 44: return 0.7193;
                case 45: return 0.7071;
                case 46: return 0.6947;
                case 47: return 0.682;
                case 48: return 0.6691;
                case 49: return 0.6561;
                case 50: return 0.6428;
                case 51: return 0.6293;
                case 52: return 0.6157;
                case 53: return 0.6018;
                case 54: return 0.5878;
                case 55: return 0.5736;
                case 56: return 0.5592;
                case 57: return 0.5446;
                case 58: return 0.5299;
                case 59: return 0.515;
                case 60: return 0.5;
                case 61: return 0.4848;
                case 62: return 0.4695;
                case 63: return 0.454;
                case 64: return 0.4384;
                case 65: return 0.4226;
                case 66: return 0.4067;
                case 67: return 0.3907;
                case 68: return 0.3746;
                case 69: return 0.3584;
                case 70: return 0.342;
                case 71: return 0.3256;
                case 72: return 0.309;
                case 73: return 0.2924;
                case 74: return 0.2756;
                case 75: return 0.2588;
                case 76: return 0.2419;
                case 77: return 0.225;
                case 78: return 0.2079;
                case 79: return 0.1908;
                case 80: return 0.1736;
                case 81: return 0.1564;
                case 82: return 0.1392;
                case 83: return 0.1219;
                case 84: return 0.1045;
                case 85: return 0.0872;
                case 86: return 0.0698;
                case 87: return 0.0523;
                case 88: return 0.0349;
                case 89: return 0.0175;
                case 90: return 0;
                default: return 1;
            }
        }
    }
}
