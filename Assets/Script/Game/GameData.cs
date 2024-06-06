using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is not attached to anything
[System.Serializable]
public class GameData
{
    public int gold;
    public int arrow;
    public int bomb;
    public int tnt1;
    public int tnt2;
    public int levelsCompleted;
    public bool playingFirstTime = true;

    public GameData(Data_Manager dataManager)
    {
        gold = dataManager.TotalGold();
        arrow = dataManager.TotalArrows();
        bomb = dataManager.TotalBomb();
        tnt1 = dataManager.TotalTNT1();
        tnt2 = dataManager.TotalTNT2();
        levelsCompleted = dataManager.TotalLevelsCompleted();
        playingFirstTime = dataManager.PlayingFirstTime();
    }
}
