using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Client.Cars.Engine.AppLogic
{
    public class PIDConverter
    {
        public string GetPidInfo(string rw)
        {
            if (rw == "XX")
                return "Зажигание";
            byte[] data1 = Enumerable.Range(0, rw.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(rw.Substring(x, 2), 16))
                     .ToArray();
            if (data1.Count() < 1)
                return "ERR";
            switch (data1[0])
            {
                case 0x00: return "Список PID'ов (0-20)";
                case 0x01: return "Monitor status since DTCs cleared.";
                case 0x02: return "Freeze DTC";
                case 0x03: return "Fuel system status";
                case 0x04: return "Нагрузка на двигатель";
                case 0x05: return "Т охлаждающей жидкости";
                case 0x06: return "Short t fuel % trim—Bank 1";
                case 0x07: return "Long t fuel % trim—Bank 1";
                case 0x08: return "Short t fuel % trim—Bank 2";
                case 0x09: return "Long t fuel % trim—Bank 2";
                case 0x0A: return "Давление топлива";
                case 0x0B: return "Давление во впуск. коллекторе";
                case 0x0C: return "Обороты двигателя";
                case 0x0D: return "Скорость автомобиля";
                case 0x0E: return "Угол опережения зажигания";
                case 0x0F: return "Температура всасываемого воздуха";
                case 0x10: return "Массовый расход воздуха";
                case 0x11: return "Положение дроссельной заслонки";
                case 0x12: return "Commanded secondary air status";
                case 0x13: return "Наличие датчиков кислорода";
                case 0x14: return "Bank 1, Sensor 1: Oxygen sensor voltage";
                case 0x15: return "Bank 1, Sensor 2: Oxygen sensor voltage";
                case 0x16: return "Bank 1, Sensor 3: Oxygen sensor voltage";
                case 0x17: return "Bank 1, Sensor 4: Oxygen sensor voltage";
                case 0x18: return "Bank 2, Sensor 1: Oxygen sensor voltage";
                case 0x19: return "Bank 2, Sensor 2: Oxygen sensor voltage";
                case 0x1A: return "Bank 2, Sensor 3: Oxygen sensor voltage";
                case 0x1B: return "Bank 2, Sensor 4:Oxygen sensor voltage";
                case 0x1C: return "OBD standards this vehicle conforms to";
                case 0x1D: return "Oxygen sensors present";
                case 0x1E: return "Auxiliary input status";
                case 0x1F: return "Время, прошедшее с запуска двигателя";
                case 0x20: return "Список поддерживаемых PID'ов (21-40)";
                case 0x21: return "Дистанция, пройденная с зажженной лампой «проверь двигатель»";
                case 0x22: return "Fuel Rail Pressure ";
                case 0x23: return "Fuel Rail Pressure ";
                case 0x24: return "O2S2_WR_lambda(1):Equivalence Ratio";
                case 0x25: return "O2S3_WR_lambda(1): Equivalence Ratio";
                case 0x26: return "O2S4_WR_lambda(1): Equivalence Ratio";
                case 0x27: return "O2S5_WR_lambda(1): Equivalence Ratio";
                case 0x28: return "O2S6_WR_lambda(1): Equivalence Ratio";
                case 0x2A: return "O2S7_WR_lambda(1): Equivalence Ratio";
                case 0x2B: return "O2S8_WR_lambda(1): Equivalence Ratio";
                case 0x2C: return "Commanded EGR";
                case 0x2D: return "EGR Error";
                case 0x2E: return "Commanded evaporative purge	0";
                case 0x2F: return "Уровень топлива ";
                case 0x30: return "Количество прогревов со времени очистки кодов нейсправности)";
                case 0x31: return "Дистанция, пройденная со времени очистки кодов нейсправностей";
                case 0x32: return "Evap. System Vapor Pressure";
                case 0x33: return "Атмосферное давление (абсолютное)";
                case 0x34: return "O2S1_WR_lambda(1): Equivalence Ratio";
                case 0x35: return "O2S2_WR_lambda(1): Equivalence Ratio";
                case 0x36: return "O2S3_WR_lambda(1):Equivalence Ratio";
                case 0x37: return "O2S4_WR_lambda(1):Equivalence Ratio";
                case 0x38: return "O2S5_WR_lambda(1):Equivalence Ratio";
                case 0x39: return "O2S6_WR_lambda(1):Equivalence Ratio";
                case 0x3A: return "O2S7_WR_lambda(1):Equivalence Ratio";
                case 0x3B: return "O2S8_WR_lambda(1):Equivalence Ratio";
                case 0x3C: return "Catalyst Temperature Bank 1, Sensor 1";
                case 0x3D: return "Catalyst Temperature Bank 2, Sensor 1";
                case 0x3E: return "Catalyst Temperature Bank 1, Sensor 2";
                case 0x3F: return "Catalyst Temperature Bank 2, Sensor 2";
                case 0x40: return "Список поддерживаемых PID'ов (41-60)";
                case 0x41: return "Monitor status this drive cycle";
                case 0x42: return "Напряжение контрольного модуля ";
                case 0x43: return "Абсолютное значение нагрузки ";
                case 0x44: return "Command equivalence ratio";
                case 0x45: return "Относительное положение дроссельной заслонки";
                case 0x46: return "Температура окружающего воздуха ";
                case 0x47: return "Абсолютное положение дроссельной заслонки B ";
                case 0x48: return "Абсолютное положение дроссельной заслонки C ";
                case 0x49: return "Положение педали акселератора D ";
                case 0x4A: return "Положение педали акселератора E ";
                case 0x4B: return "Положение педали акселератора F ";
                case 0x4C: return "Commanded throttle actuator";
                case 0x4D: return "Время со включенной лампой «проверь двигатель» ";
                case 0x4E: return "Время, прошедшее с момента очистки кодов неисправностей";
                case 0x4F: return "Max val for equivalence ratio";
                case 0x50: return "Max val for air flow rate from MAF sensor";
                case 0x51: return "Тип топлива";
                case 0x52: return "Ethanol fuel %";
                case 0x53: return "Absolute Evap system Vapor Pressure	0";
                case 0x54: return "Evap system vapor pressure";
                case 0x55: return "Short term secondary oxygen sensor trim bank 1 and bank 3";
                case 0x56: return "Long term secondary oxygen sensor trim bank 1 and bank 3";
                case 0x57: return "Short term secondary oxygen sensor trim bank 2 and bank 4";
                case 0x58: return "Long term secondary oxygen sensor trim bank 2 and bank 4";
                case 0x59: return "Абсолютное давление на топливной рампе";
                case 0x5A: return "Относительное положение педали акселератора";
                case 0x5B: return "Заряд силовой батареи гибрида";
                case 0x5C: return "Температура масла двигателя";
                case 0x5D: return "Регулирование момента впрыска";
                case 0x5E: return "Engine fuel rate	0";
                case 0x5F: return "Emission requirements to which vehicle is designed";
                case 0x60: return "Список поддерживаемых PID'ов (61-80)";
                case 0x61: return "Запрашиваемый момент двигателя ";
                case 0x62: return "Реальный момент двигателя ";
                case 0x63: return "Исходный момент двигателя";
                case 0x64: return "Engine percent torque data";
                case 0x65: return "Auxiliary input / output supported";
                case 0x66: return "Mass air flow sensor	";
                case 0x67: return "Engine coolant temperature";
                case 0x68: return "Intake air temperature sensor";
                case 0x69: return "Commanded EGR and EGR Error";
                case 0x6A: return "Commanded Diesel intake air flow control and relative intake air flow position";
                case 0x6B: return "Exhaust gas recirculation temperature";
                case 0x6C: return "Commanded throttle actuator control and relative throttle position";
                case 0x6D: return "Fuel pressure control system";
                case 0x6E: return "Injection pressure control system";
                case 0x6F: return "Turbocharger compressor inlet pressure";
                case 0x70: return "Boost pressure control";
                case 0x71: return "Variable Geometry turbo (VGT) control";
                case 0x72: return "Wastegate control";
                case 0x73: return "Exhaust pressure";
                case 0x74: return "Turbocharger RPM";
                case 0x75: return "Turbocharger temperature";
                case 0x76: return "Turbocharger temperature";
                case 0x77: return "Charge air cooler temperature (CACT)";
                case 0x78: return "Exhaust Gas temperature (EGT) Bank 1";
                case 0x79: return "Exhaust Gas temperature (EGT) Bank 2";
                case 0x7A: return "Diesel particulate filter (DPF)";
                case 0x7B: return "Diesel particulate filter (DPF)";
                case 0x7C: return "Diesel Particulate filter (DPF) temperature";
                case 0x7D: return "NOx NTE control area status";
                case 0x7E: return "PM NTE control area status";
                case 0x7F: return "Engine run time";
                case 0x80: return "PIDs supported [81 - A0]";
                case 0x81: return "Engine run time for Auxiliary Emissions Control Device(AECD)";
                case 0x82: return "Engine run time for Auxiliary Emissions Control Device(AECD)";
                case 0x83: return "NOx sensor";
                case 0x84: return "Manifold surface temperature	";
                case 0x85: return "NOx reagent system	";
                case 0x86: return "Particulate matter (PM) sensor";
                case 0x87: return "Intake manifold absolute pressure";
                case 0xA0: return "PIDs supported [A1 - C0]";
                case 0xC0: return "PIDs supported [C1 - E0]";
            }

            return "PID not found";
        }

        public int GetMaxVol(string rw)
        {
            if (rw == "XX")
                return -1;
            byte[] data1 = Enumerable.Range(0, rw.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(rw.Substring(x, 2), 16))
                     .ToArray();
            if (data1.Count() < 1)
                return -1;
            switch (data1[0])
            {
                case 0x04: return 100;
                case 0x05: return 215;
                case 0x06: return 100;
                case 0x07: return 100;
                case 0x08: return 100;
                case 0x09: return 100;
                case 0x0A: return 765;
                case 0x0B: return 255;
                case 0x0C: return 16383;
                case 0x0D: return 255;
                case 0x0E: return 64;
                case 0x0F: return 215;
                case 0x10: return 656;
                case 0x11: return 100;
                case 0x14: return 2;
                case 0x15: return 2;
                case 0x16: return 2;
                case 0x17: return 2;
                case 0x18: return 2;
                case 0x19: return 2;
                case 0x1A: return 2;
                case 0x1B: return 2;
                case 0x1F: return 65535;
                case 0x21: return 65535;
                case 0x22: return 5178;
                case 0x23: return 656;
                case 0x24: return 8;
                case 0x25: return 8;
                case 0x26: return 8;
                case 0x27: return 8;
                case 0x28: return 8;
                case 0x2A: return 8;
                case 0x2B: return 8;
                case 0x2C: return 100;
                case 0x2D: return 100;
                case 0x2E: return 100;
                case 0x2F: return 100;
                case 0x30: return 255;
                case 0x31: return 65535;
                case 0x32: return 9;
                case 0x33: return 255;
                case 0x34: return 128;
                case 0x35: return 128;
                case 0x36: return 128;
                case 0x37: return 128;
                case 0x38: return 128;
                case 0x39: return 128;
                case 0x3A: return 128;
                case 0x3B: return 128;
                case 0x3C: return 7;
                case 0x3D: return 7;
                case 0x3E: return 7;
                case 0x3F: return 7;
                case 0x42: return 66;
                case 0x43: return 26;
                case 0x44: return 2;
                case 0x45: return 100;
                case 0x46: return 215;
                case 0x47: return 100;
                case 0x48: return 100;
                case 0x49: return 100;
                case 0x4A: return 100;
                case 0x4B: return 100;
                case 0x4C: return 100;
                case 0x4D: return 65535;
                case 0x4E: return 65535;
                case 0x4F: return 255;
                case 0x50: return 2550;
                case 0x52: return 100;
                case 0x53: return 328;
                case 0x54: return 328;
                case 0x55: return 100;
                case 0x56: return 100;
                case 0x57: return 100;
                case 0x58: return 100;
                case 0x59: return 656;
                case 0x5A: return 100;
                case 0x5B: return 200;
                case 0x5C: return 210;
                case 0x5D: return 302;
                case 0x5E: return 3213;
                case 0x61: return 125;
                case 0x62: return 125;
                case 0x63: return 65535;
                case 0x64: return 125;
            }

            return -1;
        }

        public int GetMinVol(string rw)
        {
            if (rw == "XX")
                return 0;
            byte[] data1 = Enumerable.Range(0, rw.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(rw.Substring(x, 2), 16))
                     .ToArray();
            if (data1.Count() < 1)
                return 0;
            switch (data1[0])
            {
                case 0x05: return -40;
                case 0x06: return -100;
                case 0x07: return -100;
                case 0x08: return -100;
                case 0x09: return -100;
                case 0x0E: return -64;
                case 0x0F: return -40;
                case 0x14: return -100;
                case 0x15: return -100;
                case 0x16: return -100;
                case 0x17: return -100;
                case 0x18: return -100;
                case 0x19: return -100;
                case 0x1A: return -100;
                case 0x1B: return -100;
                case 0x2D: return -100;
                case 0x32: return -9;
                case 0x34: return -128;
                case 0x35: return -128;
                case 0x36: return -128;
                case 0x37: return -128;
                case 0x38: return -128;
                case 0x39: return -128;
                case 0x3A: return -128;
                case 0x3B: return -128;
                case 0x3C: return -40;
                case 0x3D: return -40;
                case 0x3E: return -40;
                case 0x3F: return -40;
                case 0x46: return -40;
                case 0x54: return -328;
                case 0x55: return -100;
                case 0x56: return -100;
                case 0x57: return -100;
                case 0x58: return -100;
                case 0x5C: return -40;
                case 0x5D: return -210;
                case 0x61: return -125;
                case 0x62: return -125;
                case 0x64: return -125;
            }

            return 0;
        }
    }
}
