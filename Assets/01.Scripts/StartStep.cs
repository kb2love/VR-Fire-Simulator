using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrFireSim;

public class StartStep : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clip;

    AudioManager manager;
    void Start()
    {
        Invoke("OpenStep00", 3.0f);
        manager = gameObject.AddComponent<AudioManager>();
        manager.LoopAudioPlay(audioSource, clip);
    }
    void OpenStep00()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
