using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private bool windowOpen = true;        // Birds will sing on awake!! Change if there is another story plan
    private bool radioOn = true;

    [SerializeField] private GameObject window;
    [SerializeField] private GameObject cat;
    [SerializeField] private GameObject bathroomDoor;
    [SerializeField] private GameObject radio;

    //public AudioClipRepetition catMeow_Repetition;
    public MeowAudioClips[] meowAudioClips = default;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (windowOpen && Input.GetKeyDown(KeyCode.B))
        {
            CloseWindow();
        }
        else if(!windowOpen && Input.GetKeyDown(KeyCode.B))
        {
            OpenWindow();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            PetCat();
        }

        if (radioOn && Input.GetKeyDown(KeyCode.M))
        {
            RadioOff();
        }
        else if(!radioOn && Input.GetKeyDown(KeyCode.M))
        {
            RadioOn();
        }
    }

    public void CloseWindow()
    {
        window.GetComponents<AudioSource>()[0].enabled = false;
        window.GetComponents<AudioSource>()[1].Play();
        windowOpen = false;
    }
    public void OpenWindow()
    {
        window.GetComponents<AudioSource>()[1].Play();
        window.GetComponents<AudioSource>()[0].enabled = true;
        windowOpen = true;
    }

    public void PetCat()
    {
        cat.GetComponents<AudioSource>()[1].PlayOneShot(CatMeowClip());
    }

    public void RadioOn()
    {
        radio.GetComponent<AudioSource>().enabled = true;
        radioOn = true;
    }
    public void RadioOff()
    {
        radio.GetComponent<AudioSource>().enabled = false;
        radioOn = false;
    }

    public AudioClip CatMeowClip()
    {
        AudioClip catMeow = default;
        foreach (var meow in meowAudioClips)
        {
            catMeow = meow.MeowClipRepetition.GetAudioClip();
        }
        
        return catMeow;
    }

    [Serializable]
    public class MeowAudioClips
    {
        public AudioClipRepetition MeowClipRepetition;
    }
}
