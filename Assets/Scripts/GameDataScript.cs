using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[CreateAssetMenu(fileName = "GameData", menuName = "Game Data", order = 51)]
public class GameDataScript : ScriptableObject
{
    public bool resetOnStart;
    public bool music = true;
    public bool sound = true;
    public int level = 1;
    public int balls = 6;
    public int points = 0;
    public int pointsToBall = 0;
    public string playerName = "no_name";

    public bool firstOpen = true;
    public bool isNewBestResult = false;
    public string gratsText = "";

    public List<KeyValuePair<string, int>> topPlayers = new List<KeyValuePair<string, int>>(6);
   
    public void Reset()
    {
        level = 1;
        balls = 6;
        points = 0;
        pointsToBall = 0;
    }

    // тот же самый сброс параметром только с именем
    public void ResetWithName()
    {
        level = 1;
        balls = 6;
        points = 0;
        pointsToBall = 0;
        playerName = "no_name";
    }

    public void Save()
    {
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetInt("balls", balls);
        PlayerPrefs.SetInt("points", points);
        PlayerPrefs.SetInt("pointsToBall", pointsToBall);
        PlayerPrefs.SetInt("music", music ? 1 : 0);
        PlayerPrefs.SetInt("sound", sound ? 1 : 0); 
    }

    public void Load() 
    {
        level = PlayerPrefs.GetInt("level", 1);
        balls = PlayerPrefs.GetInt("balls", 6);
        points = PlayerPrefs.GetInt("points", 0);
        pointsToBall = PlayerPrefs.GetInt("pointsToBall", 0);
        music = PlayerPrefs.GetInt("music", 1) == 1;
        sound = PlayerPrefs.GetInt("sound", 1) == 1;
    }

    // сохранение результатов лучших игроков в файл
    public void SavePlayers(List<KeyValuePair<string, int>> newTopPlayers)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = new FileStream(Application.persistentDataPath + "/topPlayers.gd", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        bf.Serialize(file, newTopPlayers);
        file.Close();
    }

    // загрузка лучших результатов из файла
    public void LoadPlayers()
    {
        if (File.Exists(Application.persistentDataPath + "/topPlayers.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = new FileStream(Application.persistentDataPath + "/topPlayers.gd", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            topPlayers = (List<KeyValuePair<string, int>>)bf.Deserialize(file);
            file.Close();
        }
    }

    // список всех доступных бонусов
    public Dictionary<string, int> BonusDistribution = new Dictionary<string, int>()
    {
        {"Bonus", 10},
        {"Slow", 20}, 
        {"Fast", 20}, 
        {"Plus1", 30}, 
        {"Plus2", 15}, 
        {"Plus10", 5} 
    };
}
