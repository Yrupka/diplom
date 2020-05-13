using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class File_controller
{
    public static void Save_engine_options(List<Engine_options> saveClass)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.dataPath + "/config.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, saveClass);
        stream.Close();
    }

    public static Engine_options Load_one_profile()
    {
        List<Engine_options> data = Load_engine_options();
        if (data == null)
            return null;
        return data[0];
    }

    public static List<Engine_options> Load_engine_options()
    {
        string path = Application.dataPath + "/config.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            List<Engine_options> data = formatter.Deserialize(stream) as List<Engine_options>;
            stream.Close();
            return data;
        }
        else
            return null;
    }
}
