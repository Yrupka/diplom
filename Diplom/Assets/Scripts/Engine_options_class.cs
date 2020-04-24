﻿using System;
using System.Collections.Generic;

[Serializable]
public class Engine_options_class
{
    [Serializable]
    public struct struct_rpms
    {
        public int rpm;
        public float moment;
        public float consumption;

        public struct_rpms(int rpm, float moment, float consumption)
        {
            this.rpm = rpm;
            this.moment = moment;
            this.consumption = consumption;
        }
    }

    public int fuel_amount;
    public int heat_time;
    public int interpolation;
    public float lever_length;
    public float max_moment;
    public string[] hints;
    public List<struct_rpms> rpms;

    public Engine_options_class(int fuel_amount, float lever_length, int heat_time, int interpolation)
    {
        this.fuel_amount = fuel_amount;
        this.lever_length = lever_length;
        this.heat_time = heat_time;
        this.interpolation = interpolation;
        hints = new string[4];
        rpms = new List<struct_rpms>();
    }

    public void Set_rpms(string[,] rpm_val)
    {
        rpms.Clear();
        for (int i = 0; i < rpm_val.GetLength(0); i++)
        {
            rpms.Add(new struct_rpms(int.Parse(rpm_val[i, 0]),
                float.Parse(rpm_val[i, 1]),
                float.Parse(rpm_val[i, 2])));
        }
    }

    public string[,] Get_rpms()
    {
        string[,] converted_rpms = new string[rpms.Count, 3];
        int i = 0;
        foreach (struct_rpms rpm in rpms)
        {
            converted_rpms[i, 0] = rpm.rpm.ToString();
            converted_rpms[i, 1] = rpm.moment.ToString();
            converted_rpms[i, 2] = rpm.consumption.ToString();
            i++;
        }
        return converted_rpms;
    }

    public List<int> Get_list_rpm()
    {
        List<int> list = new List<int>();
        foreach(struct_rpms rpm in rpms)
            list.Add(rpm.rpm);
        return list;
    }

    public List<float> Get_list_moment()
    {
        List<float> list = new List<float>();
        foreach (struct_rpms rpm in rpms)
            list.Add(rpm.moment);
        return list;
    }

    public List<float> Get_list_consumption()
    {
        List<float> list = new List<float>();
        foreach (struct_rpms rpm in rpms)
            list.Add(rpm.consumption);
        return list;
    }
}
