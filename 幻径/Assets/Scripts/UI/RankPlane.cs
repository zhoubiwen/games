using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RankPlane : MonoBehaviour
{
    public Text[] scoreArr;
    private Button btn_close;
    private Transform ScoreList;

    void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowRankPlane, Show);
        btn_close = transform.Find("btn_close").GetComponent<Button>();
        ScoreList = transform.Find("ScoreList");

        ScoreList.GetComponent<Image>().color = new Color(ScoreList.GetComponent<Image>().color.r, ScoreList.GetComponent<Image>().color.g, ScoreList.GetComponent<Image>().color.b, 0);
        ScoreList.localScale = Vector3.zero;
        gameObject.SetActive(false);
        btn_close.onClick.AddListener(OnCloseButtonClick);
    }
    void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowRankPlane, Show);
    }
    private void Show()
    {
        gameObject.SetActive(true);
        ScoreList.GetComponent<Image>().DOColor(new Color(ScoreList.GetComponent<Image>().color.r, ScoreList.GetComponent<Image>().color.g, ScoreList.GetComponent<Image>().color.b, 0.7f), -.3f);
        ScoreList.DOScale(Vector3.one, 0.3f);
        int[] arr = GameManager.Instance.GetBestArr();
        for (int i = 0; i < arr.Length; i++)
        {
            scoreArr[i].text = arr[i].ToString();
        }
    }

    private void OnCloseButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayButtonAudio);
        ScoreList.GetComponent<Image>().DOColor(new Color(ScoreList.GetComponent<Image>().color.r, ScoreList.GetComponent<Image>().color.g, ScoreList.GetComponent<Image>().color.b, 0), -.3f);
        ScoreList.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
