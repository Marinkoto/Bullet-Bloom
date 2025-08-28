using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public void PlaySound(AudioSource source, AudioClip clip, bool randomisePitch)
    {
        if (randomisePitch)
        {
            StartCoroutine(ChangePitch(source));
        }
        source.PlayOneShot(clip);
    }
    public IEnumerator ChangePitch(AudioSource source)
    {
        source.pitch = Random.Range(0.5f, 2f);
        yield return new WaitUntil(() => source.isPlaying == false);
        source.pitch = 1;
    }
}
