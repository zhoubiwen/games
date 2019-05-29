using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePool : MonoBehaviour
{
    public static GamePool Instance;

    private int initSpawnCount = 5;

    private List<GameObject> normalPlatformList = new List<GameObject>();
    private List<GameObject> commonPlatformList = new List<GameObject>();
    private List<GameObject> grassPlatformList = new List<GameObject>();
    private List<GameObject> winterPlatformList = new List<GameObject>();
    private List<GameObject> spikePlatformLeftList = new List<GameObject>();
    private List<GameObject> spikePlatformRightList = new List<GameObject>();
    private List<GameObject> deathEffectList = new List<GameObject>();
    private List<GameObject> diamondList = new List<GameObject>();

    private ManagerVars vars;

    private void Awake()
    {
        Instance = this;
        vars = ManagerVars.GetManagerVars();
        Init();
    }

    private void Init()
    {
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObj(vars.Platform,ref normalPlatformList);
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            for (int j = 0; j < vars.commonPlatformGroup.Count; j++)
            {
                InstantiateObj(vars.commonPlatformGroup[j],ref commonPlatformList);
            }
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            for (int j = 0; j < vars.grassPlatformGroup.Count; j++)
            {
                InstantiateObj(vars.grassPlatformGroup[j],ref grassPlatformList);
            }
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            for (int j = 0; j < vars.winterPlatformGroup.Count; j++)
            {
                InstantiateObj(vars.winterPlatformGroup[j],ref winterPlatformList);
            }
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObj(vars.SpikePlatformLeft,ref spikePlatformLeftList);
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObj(vars.SpikePlatformRight,ref spikePlatformRightList);
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObj(vars.deathEffect, ref deathEffectList);
        }
        for (int i = 0; i < initSpawnCount; i++)
        {
            InstantiateObj(vars.diamond, ref diamondList);
        }
    }

    private GameObject InstantiateObj(GameObject prefab,ref List<GameObject> addList)
    {
        GameObject go = Instantiate(prefab, transform);
        go.SetActive(false);
        addList.Add(go);
        return go;
    }
    /// <summary>
    /// 获取普通平台（单个）
    /// </summary>
    /// <returns></returns>
    public GameObject GetNormalPlatform()
    {
        for (int i = 0; i < normalPlatformList.Count; i++)
        {
            if (normalPlatformList[i].activeInHierarchy == false)
            {
                return normalPlatformList[i];
            }
        }
        return InstantiateObj(vars.Platform, ref normalPlatformList);
    }
    /// <summary>
    /// 获取普通组合平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetCommonPlatform()
    {
        for (int i = 0; i < commonPlatformList.Count; i++)
        {
            if (commonPlatformList[i].activeInHierarchy == false)
            {
                return commonPlatformList[i];
            }
        }

        int value = Random.Range(0, vars.commonPlatformGroup.Count);
        return InstantiateObj(vars.commonPlatformGroup[value],ref commonPlatformList);
    }
    /// <summary>
    /// 获取草地组合平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetGrassPlatform()
    {
        for (int i = 0; i < grassPlatformList.Count; i++)
        {
            if (grassPlatformList[i].activeInHierarchy == false)
            {
                return grassPlatformList[i];
            }
        }

        int value = Random.Range(0, vars.grassPlatformGroup.Count);
        return InstantiateObj(vars.grassPlatformGroup[value], ref grassPlatformList);
    }
    /// <summary>
    /// 获取雪地组合平台
    /// </summary>
    /// <returns></returns>
    public GameObject GetWinterPlatform()
    {
        for (int i = 0; i < winterPlatformList.Count; i++)
        {
            if (winterPlatformList[i].activeInHierarchy == false)
            {
                return winterPlatformList[i];
            }
        }

        int value = Random.Range(0, vars.winterPlatformGroup.Count);
        return InstantiateObj(vars.winterPlatformGroup[value], ref winterPlatformList);
    }
    /// <summary>
    /// 获取左钉子组合
    /// </summary>
    /// <returns></returns>
    public GameObject GetLeftSpikePlatform()
    {
        for (int i = 0; i < spikePlatformLeftList.Count; i++)
        {
            if (spikePlatformLeftList[i].activeInHierarchy == false)
            {
                return spikePlatformLeftList[i];
            }
        }
        return InstantiateObj(vars.SpikePlatformLeft, ref spikePlatformLeftList);
    }
    /// <summary>
    /// 获取右钉子组合
    /// </summary>
    /// <returns></returns>
    public GameObject GetRightSpikePlatform()
    {
        for (int i = 0; i < spikePlatformRightList.Count; i++)
        {
            if (spikePlatformRightList[i].activeInHierarchy == false)
            {
                return spikePlatformRightList[i];
            }
        }
        return InstantiateObj(vars.SpikePlatformRight, ref spikePlatformRightList);
    }
    /// <summary>
    /// 获取死亡特效
    /// </summary>
    /// <returns></returns>
    public GameObject GetDeathEffect()
    {
        for (int i = 0; i < deathEffectList.Count; i++)
        {
            if (deathEffectList[i].activeInHierarchy == false)
            {
                return deathEffectList[i];
            }
        }
        return InstantiateObj(vars.deathEffect, ref deathEffectList);
    }
    /// <summary>
    /// 获取钻石
    /// </summary>
    /// <returns></returns>
    public GameObject GetDiamond()
    {
        for (int i = 0; i < diamondList.Count; i++)
        {
            if (diamondList[i].activeInHierarchy == false)
            {
                return diamondList[i];
            }
        }
        return InstantiateObj(vars.diamond, ref diamondList);
    }
}
