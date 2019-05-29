using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public static bool isGameAgain;

    private bool isFirstGame;
    private bool isMuteOn;
    private int[] bestScoreArr;
    private int selectSkin;
    private bool[] skinUnlocked;
    private int diamondCount;

    public void SetIsFirstGame(bool isFirstGame)
    {
        this.isFirstGame = isFirstGame;
    }
    public void SetIsMuteOn(bool isMuteOn)
    {
        this.isMuteOn = isMuteOn;
    }
    public void SetBestScoreArr(int[] bestScoreArr)
    {
        this.bestScoreArr = bestScoreArr;
    }
    public void SetSelectSkin(int selectSkin)
    {
        this.selectSkin = selectSkin;
    }
    public void SetSkinUnlocked(bool[] skinUnlocked)
    {
        this.skinUnlocked = skinUnlocked;
    }
    public void SetDiamondCount(int diamondCount)
    {
        this.diamondCount = diamondCount;
    }

    public bool GetIsFirstGame()
    {
        return isFirstGame;
    }
    public bool GetIsMuteOn()
    {
        return isMuteOn; ;
    }
    public int[] GetBestScoreArr()
    {
        return bestScoreArr;
    }
    public int GetSelectSkin()
    {
        return selectSkin;
    }
    public bool[]GetSkinUnlocked()
    {
        return skinUnlocked;
    }
    public int GetDiamondCount()
    {
        return diamondCount;
    }
}