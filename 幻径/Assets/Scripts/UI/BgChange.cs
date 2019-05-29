using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgChange : MonoBehaviour
{
    private SpriteRenderer sprite;
    private ManagerVars vars;

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        sprite = GetComponent<SpriteRenderer>();
        int value = Random.Range(0, vars.bgSprites.Count);
        sprite.sprite = vars.bgSprites[value];
    }
}
