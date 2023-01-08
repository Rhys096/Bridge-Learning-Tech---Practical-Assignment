using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad
{
    static string path = Application.persistentDataPath + "/score.json";

    public static void SaveAttempt(int score, int collected)
    {
        ScoreData compare = LoadAttempt();
        string data;
        if (compare.score < score)
        {
            data = JsonUtility.ToJson(new ScoreData(score, collected));
        }
        else
        {
            data = JsonUtility.ToJson(compare);
        }
        
        System.IO.File.WriteAllText(path, data);
    }

    public static ScoreData LoadAttempt()
    {
        if (!System.IO.File.Exists(path))
        {
            return new ScoreData(0, 0);
        }
        string text = System.IO.File.ReadAllText(path);
        ScoreData sd = JsonUtility.FromJson<ScoreData>(text);
        if (sd != null)
        {
            return sd;
        }
        else
        {
            return new ScoreData(0, 0);
        }
    }
}

[System.Serializable]
public class ScoreData
{

    public ScoreData(int pScore, int pCollected)
    {
        score = pScore;
        collected = pCollected;
    }

    public int score;
    public int collected;
}