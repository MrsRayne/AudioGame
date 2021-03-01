using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private bool windowOpen = true;        // Birds will sing on awake!! Change if there is another story plan

    [SerializeField] private GameObject window;
    [SerializeField] private GameObject cat;
    [SerializeField] private GameObject bathroomDoor;

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
        cat.GetComponents<AudioSource>()[1].Play();
    }
}
