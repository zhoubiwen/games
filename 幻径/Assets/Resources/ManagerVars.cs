using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName ="Create ManagerVars")]
public class ManagerVars : ScriptableObject
{
    public static ManagerVars GetManagerVars ()
    {
        return Resources.Load<ManagerVars>("Manager Vars");
    }
    public List<Sprite> bgSprites = new List<Sprite>();
    public List<Sprite> platformSprites = new List<Sprite>();
    public List<Sprite> skinSprites = new List<Sprite>();
    public List<Sprite> characterSkinSprites = new List<Sprite>();

    public List<string> skinName = new List<string>();
    public List<int> skinPrice = new List<int>();

    public float nextXpos = 0.554f;
    public float nextYpos = 0.645f;

    public GameObject Platform;
    public GameObject Character;
    public GameObject SkinChooseItem;
    public List<GameObject> commonPlatformGroup = new List<GameObject>();
    public List<GameObject> grassPlatformGroup = new List<GameObject>();
    public List<GameObject> winterPlatformGroup = new List<GameObject>();


    public GameObject SpikePlatformLeft;
    public GameObject SpikePlatformRight;

    public GameObject deathEffect;
    public GameObject diamond;

    public AudioClip buttonClip;
    public AudioClip diamondClip;
    public AudioClip fallClip;
    public AudioClip hitClip;
    public AudioClip jumpClip;

    public Sprite muteOn;
    public Sprite muteClose;
}
