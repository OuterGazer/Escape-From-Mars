using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyLaserSize : MonoBehaviour
{
    [SerializeField] PingPongObstacle obstacle;
    [SerializeField] float maxStartLifetime = 0.30f;
    [SerializeField] float minStartLifetime = 0.025f;

    private ParticleSystem laserParticles;
    private ParticleSystem.MainModule mainModule;

    private bool shouldParticlesPlay;
    private bool shouldParticlesShrink;

    // Start is called before the first frame update
    void Start()
    {
        this.laserParticles = this.gameObject.GetComponent<ParticleSystem>();
        this.mainModule = this.laserParticles.main;

    }

    // Update is called once per frame
    void Update()
    {
        float currentStartLifetime;

        if (!this.shouldParticlesShrink && (this.obstacle.CurrentPos < this.obstacle.Threshold))
            currentStartLifetime = Mathf.Lerp(this.minStartLifetime, this.maxStartLifetime, this.obstacle.CurrentPos);
        else
            currentStartLifetime = Mathf.Lerp(this.maxStartLifetime, this.minStartLifetime, this.obstacle.CurrentPos);

        this.mainModule.startLifetime = new ParticleSystem.MinMaxCurve(currentStartLifetime);

        if ((this.obstacle.CurrentPos > this.obstacle.Threshold) &&
            !this.shouldParticlesPlay)
        {
            this.laserParticles.Stop();
            StopHitParticlesOnObstacle();
        }
        else if((this.obstacle.CurrentPos > this.obstacle.Threshold) && 
                this.shouldParticlesShrink)
        {
            this.shouldParticlesShrink = false;
        }
            
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            this.shouldParticlesPlay = true;
            this.shouldParticlesShrink = true;
            this.laserParticles.Play();
            PlayHitParticlesOnObstacle(other);
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        this.shouldParticlesPlay = false;
    }

    private void StopHitParticlesOnObstacle()
    {
        ParticleSystem[] hitParticles = this.obstacle.GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem item in hitParticles)
        {
            if (this.gameObject.CompareTag("Left Particles") && item.CompareTag("Left Particles"))
                item.Stop();
            else if(this.gameObject.CompareTag("Right Particles") && item.CompareTag("Right Particles"))
                item.Stop();
        }
    }

    private void PlayHitParticlesOnObstacle(Collider other)
    {
        ParticleSystem[] hitParticles = other.GetComponentsInChildren<ParticleSystem>();

        foreach(ParticleSystem item in hitParticles)
        {
            if (this.gameObject.CompareTag("Left Particles") && item.CompareTag("Left Particles"))
                item.Play();
            else if (this.gameObject.CompareTag("Right Particles") && item.CompareTag("Right Particles"))
                item.Play();
        }
    }
}
