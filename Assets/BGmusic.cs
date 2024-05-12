using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGmusic : MonoBehaviour
{
    public static BGmusic Instance;
    AudioSource audioSource;
    [SerializeField] AudioClip musicClip;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSource.clip = musicClip;
        audioSource.Play();
    }
}
