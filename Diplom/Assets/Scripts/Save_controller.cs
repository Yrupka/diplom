using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class Save_controller
{
    public static void Save_engine_options(Engine_options_class saveClass)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.dataPath + "/config.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, saveClass);
        stream.Close();
    }

    public static Engine_options_class Load_engine_options()
    {
        string path = Application.dataPath + "/config.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Engine_options_class data = formatter.Deserialize(stream) as Engine_options_class;
            stream.Close();
            return data;
        }
        else
            return null;
    }
}
