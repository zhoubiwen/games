using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlane : MonoBehaviour
{
    private Button btn_Pause;
    private Button btn_Play;
    private Text txt_Score;
    private Text txt_Diamond;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowGamePlane, Show);
        EventCenter.AddListener<int>(EventDefine.UpdataScoreText, UpdataScore);
        EventCenter.AddListener<int>(EventDefine.UpdataDiamond, UpdataDiamond);
        Init();
    }

   
    private void Init()
    {
        btn_Pause = transform.Find("btn_Pause").GetComponent<Button>();
        btn_Pause.onClick.AddListener(OnPauseClick);
        btn_Play = transform.Find("btn_Play").GetComponent<Button>();
        btn_Play.onClick.AddListener(OnPlayClick);
        txt_Score = transform.Find("txt_Score").GetComponent<Text>();
        txt_Diamond = transform.Find("Diamond/txt_Diamond").GetComponent<Text>();
        btn_Play.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowGamePlane, Show);
        EventCenter.RemoveListener<int>(EventDefine.UpdataScoreText, UpdataScore);
        EventCenter.RemoveListener<int>(EventDefine.UpdataDiamond, UpdataDiamond);
    }

   

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void OnPlayClick()
    {
        EventCenter.Broadcast(EventDefine.PlayButtonAudio);
        btn_Pause.gameObject.SetActive(true);
        btn_Play.gameObject.SetActive(false);
        //TODO继续游戏
        Time.timeScale = 1;
        GameManager.Instance.isPause = false;
    }
    
    private void OnPauseClick()
    {
        EventCenter.Broadcast(EventDefine.PlayButtonAudio);
        btn_Pause.gameObject.SetActive(false);
        btn_Play.gameObject.SetActive(true);
        //TODO 游戏暂停
        Time.timeScale = 0;
        GameManager.Instance.isPause = true;
    }

    private void UpdataScore(int score)
    {
        txt_Score.text = score.ToString();
    }

    private void UpdataDiamond(int diamond)
    {
        txt_Diamond.text = diamond.ToString();
    }

}
