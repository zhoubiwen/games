using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AduioManager : MonoBehaviour
{
    private AudioSource source;
    private ManagerVars vars;

    void Awake()
    {
        EventCenter.AddListener(EventDefine.PlayButtonAudio, PlayButtonAudio);
        EventCenter.AddListener<bool>(EventDefine.isMuteOn, IsMuteOn);
        source = GetComponent<AudioSource>();
        vars = ManagerVars.GetManagerVars();
    }
    void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.PlayButtonAudio, PlayButtonAudio);
        EventCenter.RemoveListener<bool>(EventDefine.isMuteOn, IsMuteOn);
    }
    private void PlayButtonAudio()
    {
        source.PlayOneShot(vars.buttonClip);
    }

    private void IsMuteOn(bool value)
    {
        source.mute = !value;
    }
}
