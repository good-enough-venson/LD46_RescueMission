using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] clips;
    public float delay;
    private float timer;
    private int songInd = 0;

    private void Start() {
        if(audioSource && clips.Length > 0) {
            audioSource.clip = clips[songInd];
            audioSource.Play();
            timer = Time.time + clips[songInd].length + delay;
        }
    }

    private void Update() {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            songInd++;
            if (songInd >= clips.Length)
                songInd = 0;

            audioSource.clip = clips[songInd];
            audioSource.Play();
            timer = Time.time + audioSource.clip.length + delay;
        }
    }
}
