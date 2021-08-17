using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceWind : MonoBehaviour
{
    [SerializeField] AudioClip windLoop;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource windSound = GameObject.Find("Wind").AddComponent<AudioSource>();
        windSound.clip = this.windLoop;
        windSound.loop = true;
        windSound.Play();
    }
}
