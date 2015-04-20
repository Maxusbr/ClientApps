using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DTCDev.Models.DeviceSender;
using DTCDev.Models.DeviceSender.DISP;

namespace DTCDev.Client.Sensors
{
    public class SensorLocator
    {
        public UserControl GetSensor(SensorsTypeEnum.SensorsMode mode, DevicePresenter.Sensor model)
        {
            if (mode == SensorsTypeEnum.SensorsMode.MIN)
            {
                switch (model.Model.PrType)
                {
                    case 1:
                        Discret.Min.DiscretMinDoor mDoor = new Discret.Min.DiscretMinDoor();
                        mDoor.Model = model;
                        return mDoor;
                    case 2:
                        Discret.Min.DiscretMinWindow mWindow = new Discret.Min.DiscretMinWindow();
                        mWindow.Model = model;
                        return mWindow;
                    case 3:
                        Discret.Min.DiscretMinLeakage mLeakage = new Discret.Min.DiscretMinLeakage();
                        mLeakage.Model = model;
                        return mLeakage;
                    case 4:
                        Analog.Min.CurrentControl mCurrent = new Analog.Min.CurrentControl();
                        mCurrent.Model = model;
                        return mCurrent;
                    case 5:
                        Analog.Min.TemperatureControl mTemper = new Analog.Min.TemperatureControl();
                        mTemper.Model = model;
                        return mTemper;
                    case 6:
                        Analog.Min.WetnessControl mWet = new Analog.Min.WetnessControl();
                        mWet.Model = model;
                        return mWet;
                    case 7:
                        Analog.Min.WetnessControl mWet2 = new Analog.Min.WetnessControl();
                        mWet2.Model = model;
                        return mWet2;
                    case 8:
                        Analog.Min.WetnessControl mWet3 = new Analog.Min.WetnessControl();
                        mWet3.Model = model;
                        return mWet3;
                    case 9:
                        Analog.Min.LightnessControl mLight = new Analog.Min.LightnessControl();
                        mLight.Model = model;
                        return mLight;
                    case 10:
                        Analog.Min.BatteryControl bControl = new Analog.Min.BatteryControl();
                        bControl.Model = model;
                        return bControl;
                    case 11:
                        Analog.Min.BatteryControl b24Control = new Analog.Min.BatteryControl(Analog.Min.BatteryControl.BatType.V24);
                        b24Control.Model = model;
                        return b24Control;
                    case 12:
                        Analog.Min.EngineOnControl engControl = new Analog.Min.EngineOnControl();
                        engControl.Model = model;
                        return engControl;
                    case 13:
                        Discret.Min.DiscretMinLock l = new Discret.Min.DiscretMinLock();
                        l.Model = model;
                        return l;
                    case 14:
                        Discret.Min.DiscretMinSmoke smk = new Discret.Min.DiscretMinSmoke();
                        smk.Model = model;
                        return smk;
                    case 15:
                        Discret.Min.DiscretMinMove move = new Discret.Min.DiscretMinMove();
                        move.Model = model;
                        return move;
                    case 16:
                        Discret.Min.DiscretMinTrunk trunk = new Discret.Min.DiscretMinTrunk();
                        trunk.Model = model;
                        return trunk;
                    case 17:
                        Analog.Min.WaterSwitch ws = new Analog.Min.WaterSwitch();
                        ws.Model = model;
                        return ws;
                    case 18:
                        Analog.Min.FuelLevel fuel = new Analog.Min.FuelLevel();
                        fuel.Model = model;
                        return fuel;
                    case 19:
                        Analog.Min.WaterLevel water = new Analog.Min.WaterLevel();
                        water.Model = model;
                        return water;
                    case 20:
                        Analog.Min.OilPressure oil = new Analog.Min.OilPressure();
                        oil.Model = model;
                        return oil;
                    case 21:
                        Analog.Min.RPMCounter rpm = new Analog.Min.RPMCounter();
                        rpm.Model = model;
                        return rpm;
                    case 22:
                        Analog.Min.SmallShock ssh = new Analog.Min.SmallShock();
                        ssh.Model = model;
                        return ssh;
                    case 23:
                        Analog.Min.BigShock bsh = new Analog.Min.BigShock();
                        bsh.Model = model;
                        return bsh;
                }
            }
            else if(mode== SensorsTypeEnum.SensorsMode.MAX)
            {
                switch (model.Model.PrType)
                {
                    case 1:
                        Discret.Max.MxDoorControl MDoor = new Discret.Max.MxDoorControl();
                        MDoor.Sensor = model;
                        MDoor.SensorName = model.Model.Name;
                        return MDoor;

                    case 2:
                        Discret.Max.MxWindowControl MWindow = new Discret.Max.MxWindowControl();
                        MWindow.Sensor = model;
                        MWindow.SensorName = model.Model.Name;
                        return MWindow;

                    case 3:
                        Discret.Max.MxLeakageControl MLeak = new Discret.Max.MxLeakageControl();
                        MLeak.Sensor = model;
                        MLeak.SensorName = model.Model.Name;
                        return MLeak;

                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                        Analog.Max.MxAnalogDefaultNew MLight = new Analog.Max.MxAnalogDefaultNew(model);
                        return MLight;
                    case 16:
                        Discret.Max.MxTrunkControl MTrunk = new Discret.Max.MxTrunkControl();
                        MTrunk.Sensor = model;
                        MTrunk.SensorName = model.Model.Name;
                        return MTrunk;
                }
            }

            else if (mode == SensorsTypeEnum.SensorsMode.HIST)
            {
                switch (model.Model.PrType)
                {
                    case 1:
                        Discret.Min.DiscretMinDoor mDoor = new Discret.Min.DiscretMinDoor();
                        mDoor.Model = model;
                        return mDoor;
                    case 2:
                        Discret.Min.DiscretMinWindow mWindow = new Discret.Min.DiscretMinWindow();
                        mWindow.Model = model;
                        return mWindow;
                    case 3:
                        Discret.Min.DiscretMinLeakage mLeakage = new Discret.Min.DiscretMinLeakage();
                        mLeakage.Model = model;
                        return mLeakage;
                    case 4:
                        Analog.Min.CurrentControl mCurrent = new Analog.Min.CurrentControl(SensorsTypeEnum.SensorsMode.HIST);
                        mCurrent.Model = model;
                        return mCurrent;
                    case 5:
                        Analog.Min.TemperatureControl mTemper = new Analog.Min.TemperatureControl(SensorsTypeEnum.SensorsMode.HIST);
                        mTemper.Model = model;
                        return mTemper;
                    case 6:
                        Analog.Min.WetnessControl mWet = new Analog.Min.WetnessControl();
                        mWet.Model = model;
                        return mWet;
                    case 7:
                        Analog.Min.WetnessControl mWet2 = new Analog.Min.WetnessControl();
                        mWet2.Model = model;
                        return mWet2;
                    case 8:
                        Analog.Min.WetnessControl mWet3 = new Analog.Min.WetnessControl();
                        mWet3.Model = model;
                        return mWet3;
                    case 9:
                        Analog.Min.LightnessControl mLight = new Analog.Min.LightnessControl();
                        mLight.Model = model;
                        return mLight;
                    case 10:
                        Analog.Min.BatteryControl bControl = new Analog.Min.BatteryControl();
                        bControl.Model = model;
                        return bControl;
                    case 11:
                        Analog.Min.BatteryControl b24Control = new Analog.Min.BatteryControl(Analog.Min.BatteryControl.BatType.V24);
                        b24Control.Model = model;
                        return b24Control;
                    case 12:
                        Analog.Min.EngineOnControl engControl = new Analog.Min.EngineOnControl();
                        engControl.Model = model;
                        return engControl;
                    case 13:
                        Discret.Min.DiscretMinLock l = new Discret.Min.DiscretMinLock();
                        l.Model = model;
                        return l;
                    case 14:
                        Discret.Min.DiscretMinSmoke smk = new Discret.Min.DiscretMinSmoke();
                        smk.Model = model;
                        return smk;
                    case 15:
                        Discret.Min.DiscretMinMove move = new Discret.Min.DiscretMinMove();
                        move.Model = model;
                        return move;
                    case 16:
                        Discret.Min.DiscretMinTrunk trunk = new Discret.Min.DiscretMinTrunk();
                        trunk.Model = model;
                        return trunk;
                    case 17:
                        Analog.Min.WaterSwitch ws = new Analog.Min.WaterSwitch();
                        ws.Model = model;
                        return ws;
                    case 18:
                        Analog.Min.FuelLevel fuel = new Analog.Min.FuelLevel();
                        fuel.Model = model;
                        return fuel;
                    case 19:
                        Analog.Min.WaterLevel water = new Analog.Min.WaterLevel();
                        water.Model = model;
                        return water;
                    case 20:
                        Analog.Min.OilPressure oil = new Analog.Min.OilPressure();
                        oil.Model = model;
                        return oil;
                    case 21:
                        Analog.Min.RPMCounter rpm = new Analog.Min.RPMCounter();
                        rpm.Model = model;
                        return rpm;
                    case 22:
                        Analog.Min.SmallShock ssh = new Analog.Min.SmallShock();
                        ssh.Model = model;
                        return ssh;
                    case 23:
                        Analog.Min.BigShock bsh = new Analog.Min.BigShock();
                        bsh.Model = model;
                        return bsh;
                }
            }

            return null;
        }
    }
}
