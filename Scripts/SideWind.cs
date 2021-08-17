using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWind : MonoBehaviour
{
    [SerializeField] float activeWindTime = 2.0f;
    [SerializeField] float inactiveWindTime = 4.0f;
    [SerializeField] float windStrength = 6.0f;
    [SerializeField] ParticleSystem windParticles;
    [SerializeField] AudioClip windSFX;

    private Collider[] sideGravityTrigger;

    private bool isWindBlowing = false;

    // Start is called before the first frame update
    void Start()
    {
        this.sideGravityTrigger = this.gameObject.GetComponents<BoxCollider>();

        this.windParticles.Stop();
        this.sideGravityTrigger[0].enabled = false;
        this.sideGravityTrigger[1].enabled = false;

        if(!this.gameObject.CompareTag("Wind Fan"))
            this.StartCoroutine(BlowWind());
    }

    private void Update()
    {
        if (!this.isWindBlowing && !this.gameObject.CompareTag("Wind Fan"))
            this.StartCoroutine(BlowWind());
    }

    private IEnumerator BlowWind()
    {
        this.isWindBlowing = true;

        this.windParticles.Play();
        this.gameObject.GetComponent<AudioSource>().Play();

        this.sideGravityTrigger[0].enabled = true;
        this.sideGravityTrigger[1].enabled = true;

        yield return new WaitForSeconds(this.activeWindTime);

        this.windParticles.Stop();
        this.gameObject.GetComponent<AudioSource>().Stop();

        this.sideGravityTrigger[0].enabled = false;
        this.sideGravityTrigger[1].enabled = false;

        if(Physics.gravity.x != 0)
            Physics.gravity = new Vector3(0.0f, Physics.gravity.y, Physics.gravity.z);

        yield return new WaitForSeconds(this.inactiveWindTime);

        this.isWindBlowing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.isWindBlowing && other.CompareTag("Player"))
            Physics.gravity = new Vector3(this.windStrength, Physics.gravity.y, Physics.gravity.z);
        else if(this.gameObject.CompareTag("Wind Fan") && other.CompareTag("Player"))
            Physics.gravity = new Vector3(this.windStrength, Physics.gravity.y, Physics.gravity.z);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            Physics.gravity = new Vector3(0.0f, Physics.gravity.y, Physics.gravity.z);
    }

    public void ActivatePermanentWind()
    {
        if (this.gameObject.CompareTag("Wind Fan"))
        {
            this.windParticles.Play();
            AudioSource windSound = this.gameObject.GetComponent<AudioSource>();
            if(windSound != null)
                windSound.Play();

            this.sideGravityTrigger[0].enabled = true;
            this.sideGravityTrigger[1].enabled = true;
        }
    }
}
