using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformGroupType
{
    Winter,
    Grass
}

public class PlatformSpawn : MonoBehaviour
{
    public Vector2 StartPosSpawn;

    private ManagerVars vars;
    //生成平台的数量
    private int spawnPlatformCount;
    //生成平台的位置
    private Vector2 platformSpawnPos;
    private bool isLeftSpawn;
    //选择的平台主题
    private Sprite SelectPlatformSprite;
    private PlatformGroupType groupType;
    //钉子生成方向
    private bool spikeSpawnLeft;
    //钉子方向的初始平台位置
    private Vector3 spikeSpawnPlatformPos;
    //生成钉子平台之后需要在钉子方向生成的平台数量
    private int afterSpikeSpawnCount;
    private bool isSpawnSpike;
    //里程碑数
    private int mileStoneCount = 10;
    private float fallTime = 2f;
    private float minFallTime = 0.5f;
    //时间系数
    private float multiple = 0.9f;

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        EventCenter.AddListener(EventDefine.DecidePath, DecidePath);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.DecidePath, DecidePath);
    }
    private void Start()
    {      
        RandomPlatformTheme();
        platformSpawnPos = StartPosSpawn;
        for (int i = 0; i < 5; i++)
        {
            spawnPlatformCount = 5;
            DecidePath();
        }

        //生成人物
        GameObject go = Instantiate(vars.Character);
        go.transform.position = new Vector3(0, -1.8f, 0);
    }
    void Update()
    {
        if (GameManager.Instance.isGameStart && GameManager.Instance.isGameOver == false)
        {
            UpdataFallTime();
        }
    }
    /// <summary>
    /// 确定路径
    /// </summary>
    private void DecidePath()
    {
        if (isSpawnSpike)
        {
            AfterSpawnSpike();
            return;
        }

        if (spawnPlatformCount > 0)
        {
            spawnPlatformCount--;
            SpawnPlatform();
        }
        else
        {
            isLeftSpawn = !isLeftSpawn;
            spawnPlatformCount = Random.Range(1, 4);
            SpawnPlatform();
        }
    }
    /// <summary>
    /// 生成平台
    /// </summary>
    private void SpawnPlatform()
    {
        int RanDir = Random.Range(0, 2);
        //生成单个平台
        if (spawnPlatformCount >= 1)
        {
            SpawnNormalPlatfor(RanDir);
        }
        //生成组合平台
        else if (spawnPlatformCount == 0)
        {
            int value = Random.Range(0, 3);
            //生成普通组合平台
            if (value == 0)
            {
                SpwanCommonPlatformGroup(RanDir);
            }
            //生成主题组合平台
            else if (value == 1)
            {
                switch (groupType)
                {
                    case PlatformGroupType.Winter:
                        SpwanWinterPlatformGroup(RanDir);
                        break;
                    case PlatformGroupType.Grass:
                        SpwanGrassPlatformGroup(RanDir);
                        break;
                    default:
                        break;
                }
            }
            //生成钉子组合平台
            else
            {
                int dir = -1;
                if (isLeftSpawn)
                {
                    dir = 0;//钉子在右边
                }
                else
                {
                    dir = 1;//钉子在左边
                }
                SpwanSpikePlatform(dir);

                isSpawnSpike = true;
                afterSpikeSpawnCount = 4;

                if (spikeSpawnLeft)
                {
                    spikeSpawnPlatformPos = new Vector3(platformSpawnPos.x - 1.65f, platformSpawnPos.y + vars.nextYpos, 0);
                }
                else
                {
                    spikeSpawnPlatformPos = new Vector3(platformSpawnPos.x + 1.65f, platformSpawnPos.y + vars.nextYpos, 0);
                }
            }
        }

        int diamondValue = Random.Range(0, 10);
        if (diamondValue == 6 && GameManager.Instance.PlayerIsMove)
        {
            GameObject go = GamePool.Instance.GetDiamond();
            go.transform.position = new Vector3(platformSpawnPos.x, platformSpawnPos.y + 0.5f, 0);
            go.SetActive(true);
        }

        if (isLeftSpawn)//往左生成
        {
            platformSpawnPos = new Vector3(platformSpawnPos.x - vars.nextXpos, platformSpawnPos.y + vars.nextYpos, 0);
        }
        else//往右生成
        {
            platformSpawnPos = new Vector3(platformSpawnPos.x + vars.nextXpos, platformSpawnPos.y + vars.nextYpos, 0);
        }
    }

    private void UpdataFallTime()
    {
        if (GameManager.Instance.GetScore() > mileStoneCount)
        {
            
            mileStoneCount *= 2;
            fallTime *= multiple;
            if (fallTime < minFallTime)
            {
                fallTime = minFallTime;
            }
        }
    }
    /// <summary>
    /// 生成普通平台（单个）
    /// </summary>
    private void SpawnNormalPlatfor(int dir)
    {
        GameObject go = GamePool.Instance.GetNormalPlatform();
        go.transform.position = platformSpawnPos;
        go.GetComponent<PlatformScript>().Init(SelectPlatformSprite, fallTime,dir);
        go.SetActive(true);
    }
    /// <summary>
    /// 生成普通组合平台
    /// </summary>
    private void SpwanCommonPlatformGroup(int dir)
    {

        GameObject go = GamePool.Instance.GetCommonPlatform();
        go.transform.position = platformSpawnPos;
        go.GetComponent<PlatformScript>().Init(SelectPlatformSprite, fallTime, dir);
        go.SetActive(true);
    }
    /// <summary>
    /// 生成草地组合平台
    /// </summary>
    private void SpwanGrassPlatformGroup(int dir)
    {

        GameObject go = GamePool.Instance.GetGrassPlatform();
        go.transform.position = platformSpawnPos;
        go.GetComponent<PlatformScript>().Init(SelectPlatformSprite, fallTime, dir);
        go.SetActive(true);
    }
    /// <summary>
    /// 生成雪地组合平台
    /// </summary>
    private void SpwanWinterPlatformGroup(int dir)
    {

        GameObject go = GamePool.Instance.GetWinterPlatform();
        go.transform.position = platformSpawnPos;
        go.GetComponent<PlatformScript>().Init(SelectPlatformSprite, fallTime, dir);
        go.SetActive(true);
    }
    /// <summary>
    /// 生成钉子组合平台
    /// </summary>
    /// <param name="dir"></param>
    private void SpwanSpikePlatform(int dir)
    {
        GameObject go = null;
        if (dir == 0)
        {
            spikeSpawnLeft = false;
            go = GamePool.Instance.GetRightSpikePlatform();
        }
        else if(dir == 1)
        {
            spikeSpawnLeft = true;
            go = GamePool.Instance.GetLeftSpikePlatform();
        }
        go.transform.position = platformSpawnPos;
        go.GetComponent<PlatformScript>().Init(SelectPlatformSprite, fallTime, dir);
        go.SetActive(true);
    }
    /// <summary>
    /// 生成钉子平台之后需要生成的平台
    /// 包括钉子方向，也包括原来的方向的平台
    /// </summary>
    private void AfterSpawnSpike()
    {
        if (afterSpikeSpawnCount > 0)
        {
            afterSpikeSpawnCount--;
            for (int i = 0; i < 2; i++)
            {
                GameObject go = GamePool.Instance.GetNormalPlatform();
                if (i == 0)//生成原来方向的平台
                {
                    go.transform.position = platformSpawnPos;
                    if (spikeSpawnLeft)//向右边生成
                    {
                        platformSpawnPos = new Vector3(platformSpawnPos.x + vars.nextXpos, platformSpawnPos.y + vars.nextYpos, 0);

                    }
                    else
                    {
                        platformSpawnPos = new Vector3(platformSpawnPos.x - vars.nextXpos, platformSpawnPos.y + vars.nextYpos, 0);
                    }
                }
                else//生成钉子方向的平台
                {
                    go.transform.position = spikeSpawnPlatformPos;
                    if (spikeSpawnLeft)
                    {
                        spikeSpawnPlatformPos = new Vector3(spikeSpawnPlatformPos.x - vars.nextXpos, spikeSpawnPlatformPos.y + vars.nextYpos, 0);
                    }
                    else
                    {
                        spikeSpawnPlatformPos = new Vector3(spikeSpawnPlatformPos.x + vars.nextXpos, spikeSpawnPlatformPos.y + vars.nextYpos, 0);
                    }
                }
                go.GetComponent<PlatformScript>().Init(SelectPlatformSprite, fallTime, 1);
                go.SetActive(true);
            }
            
        }
        else
        {
            isSpawnSpike = false;
            DecidePath();
        }
    }
    /// <summary>
    /// 随机平台主题
    /// </summary>
    private void RandomPlatformTheme()
    {
        int value = Random.Range(0, vars.platformSprites.Count);
        SelectPlatformSprite = vars.platformSprites[value];

        if (value == 2)
        {
            groupType = PlatformGroupType.Winter;
        }
        else
        {
            groupType = PlatformGroupType.Grass;
        }
    }
}
