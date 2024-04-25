using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamdomSoundOnAwake : MonoBehaviour
{
    public List<AudioClip> audioclips ;
    private AudioSource myAudioSource;
    void Awake()
     {
     myAudioSource = GetComponent<AudioSource>();
     }

    void Start()
    {
     AudioClip clip = audioclips[Random.Range(0 , audioclips.Count)];
      myAudioSource.PlayOneShot(clip);
    }
}
