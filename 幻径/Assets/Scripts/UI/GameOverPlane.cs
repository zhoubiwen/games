using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPlane : MonoBehaviour
{
    private Text currentText;
    private Text highText;
    private Text addDiamondCount;

    private Button btn_Rank;
    private Button btn_Home;
    private Button btn_Restart;

    private Image img_New;
    void Awake()
    {
        gameObject.SetActive(false);
        currentText = transform.Find("txt_Score").GetComponent<Text>();
        highText = transform.Find("HighScore/txt_HighScore").GetComponent<Text>();
        addDiamondCount = transform.Find("Diamond/txt_AddDiamond").GetComponent<Text>();

        btn_Home = transform.Find("btn_Home").GetComponent<Button>();
        btn_Rank = transform.Find("btn_Rank").GetComponent<Button>();
        btn_Restart = transform.Find("Restart").GetComponent<Button>();

        img_New = transform.Find("img_New").GetComponent<Image>();

        btn_Home.onClick.AddListener(OnHomeButtonClick);
        btn_Rank.onClick.AddListener(OnRankButtonClick);
        btn_Restart.onClick.AddListener(OnRestartButtonClick);
        EventCenter.AddListener(EventDefine.ShowGameOver, Show);
    }

    void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowGameOver, Show);
    }

    private void Show()
    {
        if (GameManager.Instance.GetScore() > GameManager.Instance.GetBestScore())
        {
            img_New.gameObject.SetActive(true);
            highText.text = GameManager.Instance.GetScore().ToString();
        }
        else
        {
            img_New.gameObject.SetActive(false);
            highText.text = GameManager.Instance.GetBestScore().ToString();
        }
        GameManager.Instance.SaveScore(GameManager.Instance.GetScore());
        currentText.text = GameManager.Instance.GetScore().ToString();
        addDiamondCount.text = "+" + GameManager.Instance.GetDiamond().ToString();
        //更新游戏总钻石
        GameManager.Instance.UpdataDiamond(GameManager.Instance.GetDiamond());
        gameObject.SetActive(true);

    }

    private void OnHomeButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayButtonAudio);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameData.isGameAgain = false;
    }

    private void OnRankButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayButtonAudio);
        EventCenter.Broadcast(EventDefine.ShowRankPlane);
    }

    private void OnRestartButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayButtonAudio);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameData.isGameAgain = true;
    }
}
