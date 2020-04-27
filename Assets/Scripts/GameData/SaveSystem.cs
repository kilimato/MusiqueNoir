using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

    // we use static (can't be instantiated) since we don't want multiple versions of our save system
public static class SaveSystem
{
    //static string path = Application.persistentDataPath + "/player.dat";
    public static void SavePlayer(GameManager manager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        // can be any file type, since it's a binary file
        string path = Application.persistentDataPath + "/player.dat";

        FileStream stream = new FileStream(path, FileMode.Create);

       // PlayerData data = new PlayerData(manager);

        //formatter.Serialize(stream, data);

        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
