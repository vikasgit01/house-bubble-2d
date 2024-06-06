using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    public static void SaveGame(Data_Manager dataManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine( Application.persistentDataPath, "GameData");
        FileStream stream = new FileStream(path, FileMode.Create);
        GameData gameData = new GameData(dataManager);

        formatter.Serialize(stream, gameData);
        stream.Close();
    }

    public static GameData LoadGame()
    {
        string path = Path.Combine(Application.persistentDataPath, "GameData");

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GameData gameData = formatter.Deserialize(stream) as GameData;
            stream.Close();
            return gameData;
        }
        else
        {
            Data_Manager.instance.SaveData();
            Debug.Log("Save file not found");
            return null;
        }
    }

}
