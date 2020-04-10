using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] clips;
    public bool canPlay;
    // Start is called before the first frame update
    void Start()
    {
        canPlay = true;
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying && canPlay)
        {
            audioSource.clip = GetRandom(clips);
            audioSource.Play();
        }
    }

    private AudioClip GetRandom(AudioClip[] clips)
    {
        return clips[Random.Range(0, clips.Length)];
    }
}
