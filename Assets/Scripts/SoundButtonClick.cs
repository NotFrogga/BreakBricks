using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButtonClick : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;

    public void PlaySoundOnClick()
    {
        if (audioClip != null)
        {
            AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
        }
    }
}
