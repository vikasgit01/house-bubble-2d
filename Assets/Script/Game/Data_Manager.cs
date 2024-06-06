using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Manager : MonoBehaviour
{

    public static Data_Manager instance;

    private bool _playingFirstTime;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (_playingFirstTime)
        {
            SaveData();
        }
    }

    public void SaveData()
    {
        SaveSystem.SaveGame(this);
    }

    public void LoadData()
    {
        GameData gameData = SaveSystem.LoadGame();
        if (gameData != null)
        {
            Game_Manager.instance.IncrementGold(gameData.gold);
            BowShoot.instance.AddArrows(gameData.arrow);
            Trap_Manager.instance.AddBomb(gameData.bomb);
            Trap_Manager.instance.AddTnt(gameData.tnt1);
            Trap_Manager.instance.AddTnt2(gameData.tnt2);
            Game_Manager.instance.LevelCompletedToValue(SaveSystem.LoadGame().levelsCompleted);
        }
    }

    public void SetGameDataToZero()
    {
        GameData gameData = SaveSystem.LoadGame();

        if (gameData != null)
        {
            Game_Manager.instance.IncrementGold(gameData.gold * 0);
            Game_Manager.instance.LevelCompletedToValue(0);
            BowShoot.instance.AddArrows(Level_01.instance.startArrows);
            Trap_Manager.instance.AddBomb(gameData.bomb * 0);
            Trap_Manager.instance.AddTnt(gameData.tnt1 * 0);
            Trap_Manager.instance.AddTnt2(gameData.tnt2 * 0);
            SetPlayingFirstTime(true);
        }
        SaveData();
    }

    public int TotalGold()
    {
        if (FindObjectOfType<Game_Manager>() != null)
        {
            return Game_Manager.instance.TotalGold();
        }
        else
        {
            return 0;
        }
    }

    public int TotalArrows()
    {
        if (FindObjectOfType<Game_Manager>() != null)
        {
            return Game_Manager.instance.TotalArrows();
        }
        else
        {
            return 0;
        }
    }

    public int TotalBomb()
    {
        if (FindObjectOfType<Game_Manager>() != null)
        {
            return Game_Manager.instance.TotalBomb();
        }
        else
        {
            return 0;
        }
    }

    public int TotalTNT1()
    {
        if (FindObjectOfType<Game_Manager>() != null)
        {
            return Game_Manager.instance.TotalTNT1();
        }
        else
        {
            return 0;
        }
    }

    public int TotalTNT2()
    {
        if (FindObjectOfType<Game_Manager>() != null)
        {
            return Game_Manager.instance.TotalTNT2();
        }
        else
        {
            return 0;
        }
    }

    public int TotalLevelsCompleted()
    {
        if (FindObjectOfType<Game_Manager>() != null)
        {
            return Game_Manager.instance.TotalLevelsCompleted();
        }
        else
        {
            return 0;
        }
    }

    public bool PlayingFirstTime()
    {
        return _playingFirstTime;
    }

    public void SetPlayingFirstTime(bool ft)
    {
        _playingFirstTime = ft;
        SaveData();
    }

}
