using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using System.Linq;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isGameStart { get; set; }
    public bool isGameOver { get; set; }
    public bool isPause { get; set; }

    public bool PlayerIsMove { get; set; }

    private int gameScore;
    private int gameDiamond;

    private ManagerVars vars;
    private GameData data;

    private bool isFirstGame;
    private bool isMuteOn;
    private int[] bestScoreArr;
    private int selectSkin;
    private bool[] skinUnlocked;
    private int diamondCount;

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        Instance = this;
        EventCenter.AddListener(EventDefine.AddScore, AddScore);
        EventCenter.AddListener(EventDefine.PlayerMover, PlayMove);
        EventCenter.AddListener(EventDefine.AddDiamond, AddDiamond);

        if (GameData.isGameAgain)
        {
            isGameStart = true;
        }
        InitGameData();
    }

    void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.AddScore, AddScore);
        EventCenter.RemoveListener(EventDefine.PlayerMover, PlayMove);
        EventCenter.RemoveListener(EventDefine.AddDiamond, AddDiamond);
    }
    private void InitGameData()
    {
        Read();
        if (data != null)
        {
            isFirstGame = data.GetIsFirstGame();
        }
        else
        {
            isFirstGame = true;
        }
        if (isFirstGame)
        {
            isFirstGame = false;
            isMuteOn = true;
            bestScoreArr = new int[3];
            selectSkin = 0;
            skinUnlocked = new bool[vars.skinSprites.Count];
            skinUnlocked[0] = true;
            diamondCount = 6;

            data = new GameData();
            Save();
        }
        else
        {
            isMuteOn = data.GetIsMuteOn();
            bestScoreArr = data.GetBestScoreArr();
            selectSkin = data.GetSelectSkin();
            skinUnlocked = data.GetSkinUnlocked();
            diamondCount = data.GetDiamondCount();
        }
    }

    /// <summary>
    /// 玩家移动会调用此方法
    /// </summary>
    private void PlayMove()
    {
        PlayerIsMove = true;
    }
    /// <summary>
    /// 增加分数
    /// </summary>
    private void AddScore()
    {
        if (isGameStart == false || isGameOver || isPause)
            return;

        gameScore++;
        EventCenter.Broadcast(EventDefine.UpdataScoreText,gameScore);
    }

    private void AddDiamond()
    {
        if (isGameStart == false || isGameOver || isPause)
            return;

        gameDiamond++;
        EventCenter.Broadcast(EventDefine.UpdataDiamond, gameDiamond);
    }

    public int GetScore()
    {
        return gameScore;
    }

    public int GetDiamond()
    {
        return gameDiamond;
    }
    /// <summary>
    /// 获取当前皮肤是否解锁
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool GetSkinUnlock(int index)
    {
        return skinUnlocked[index];
    }
    /// <summary>
    /// 设置皮肤显示
    /// </summary>
    /// <param name="index"></param>
    public void SetSkinUnlock(int index)
    {
        skinUnlocked[index] = true;
        Save();
    }
    public int GetAllDiamond()
    {
        return diamondCount;
    }
    /// <summary>
    /// 更新钻石总数
    /// </summary>
    public void UpdataDiamond(int value)
    {
        diamondCount += value;
        Save();
    }
    /// <summary>
    /// 设置当前选择的皮肤下标
    /// </summary>
    public void SetSkinSelectedIndex(int index)
    {
        selectSkin = index;
        Save();
    }

    public int GetCurrenSelectSkin()
    {
        return selectSkin;
    }
    /// <summary>
    /// 保存成绩
    /// </summary>
    /// <param name="score">当前游戏成绩</param>
    public void SaveScore(int score)
    {
        List<int> list = bestScoreArr.ToList();
        //从小到排列 例：10 20 40
        list.Sort((x, y) => (x.CompareTo(y)));
        bestScoreArr = list.ToArray();

        int index = -1;
        for (int i = 0; i < bestScoreArr.Length; i++)
        {
            if (score > bestScoreArr[i])
            {
                index = i;
            }
        }
        if (index == -1) return;

        for (int i = index; i > 1 ; i--)
        {
            bestScoreArr[i - 1] = bestScoreArr[i];
        }
        bestScoreArr[index] = score;

        Save();
    }

    public int GetBestScore()
    {
        return bestScoreArr.Max();
    }
    public int[] GetBestArr()
    {
        List<int> list = bestScoreArr.ToList();
       
        list.Sort((x, y) => (-x.CompareTo(y)));
        bestScoreArr = list.ToArray();
        return bestScoreArr;
    }
    public void SetMute(bool value)
    {
        isMuteOn = value;
        Save();
    }
    public bool GetIsMute()
    {
        return isMuteOn;
    }

    /// <summary>
    /// 存储数据
    /// </summary>
    private void Save()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Create(Application.persistentDataPath + "/GameData.data")) 
            {
                data.SetBestScoreArr(bestScoreArr);
                data.SetDiamondCount(diamondCount);
                data.SetIsFirstGame(isFirstGame);
                data.SetIsMuteOn(isMuteOn);
                data.SetSelectSkin(selectSkin);
                data.SetSkinUnlocked(skinUnlocked);
                bf.Serialize(fs, data);
            }
        }
        catch (System.Exception e)
        {

            Debug.Log(e.Message);
        }
    }
    

    /// <summary>
    /// 读取数据
    /// </summary>
    private void Read()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Open(Application.persistentDataPath + "/GameData.data", FileMode.Open)) 
            {
                data = (GameData)bf.Deserialize(fs); 
            }
        }
        catch (System.Exception e)
        {

            Debug.Log(e.Message);
        }
    }

    public void ResetData()
    {
        isFirstGame = false;
        isMuteOn = true;
        bestScoreArr = new int[3];
        selectSkin = 0;
        skinUnlocked = new bool[vars.skinSprites.Count];
        skinUnlocked[0] = true;
        diamondCount = 6;

        Save();
    }
}
