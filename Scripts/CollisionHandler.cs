using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float loadDelay = 2.0f;
    [SerializeField] private float minLandingSuccessAngle = -5.0f;
    [SerializeField] private float maxLandingSuccessAngle = 5.0f;
    [SerializeField] private float minLandingDestroyAngle = -50.0f;
    [SerializeField] private float maxLandingDestroyAngle = 50.0f;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip explosionSFX;
    [SerializeField] private AudioClip successSFX;
    [SerializeField] private AudioClip checkpointSFX;

    [Header("Particle Effects")]
    [SerializeField] private GameObject explosionVFX;
    [SerializeField] private ParticleSystem successVFX;

    private bool hasCollided = false;
    private bool isDismemberingProcessStarted = false;
    private bool isCheckpointActivated = false;
    public bool IsCheckpointActivated
    {
        get { return this.isCheckpointActivated; }
        set { this.isCheckpointActivated = value; }
    }
    private bool isAlive = true;
    public bool IsAlive => this.isAlive;
    public void SetIsALive(bool isAlive)
    {
        this.isAlive = isAlive;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (this.hasCollided)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                CheckVerticality(collision);
                break;

            case "Checkpoint":
                CheckVerticality(collision);
                break;

            case "Finish":
                CheckVerticality(collision);
                break;

            default:
                this.hasCollided = true;
                StartCrashSequence();
                break;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (this.hasCollided)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                CheckVerticality(collision);
                break;

            case "Checkpoint":
                CheckVerticality(collision);
                break;

            case "Finish":
                CheckVerticality(collision);
                break;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if(this.isAlive)
            StartCrashSequence();
    }

    private void CheckVerticality(Collision collision)
    {
        if(this.gameObject.transform.rotation.z < Quaternion.Euler(0, 0, this.maxLandingSuccessAngle).z &&
            this.gameObject.transform.rotation.z > Quaternion.Euler(0, 0, this.minLandingSuccessAngle).z)
        {
            if(collision.gameObject.CompareTag("Friendly")) { return; }

            if (collision.gameObject.CompareTag("Checkpoint"))
            {
                StartCheckpointProcess(collision);
                return;
            }

            this.hasCollided = true;
            StartSuccessSequence(collision);
        }
        else if (this.gameObject.transform.rotation.z > Quaternion.Euler(0, 0, this.maxLandingDestroyAngle).z ||
                 this.gameObject.transform.rotation.z < Quaternion.Euler(0, 0, this.minLandingDestroyAngle).z)
        {
            this.hasCollided = true;
            StartCrashSequence();
        }
    }

    private void StartSuccessSequence(Collision collision)
    {
        LevelLoader levelLoader = GameObject.FindObjectOfType<LevelLoader>();

        GameObject.FindObjectOfType<Movement>().enabled = false;

        levelLoader.Invoke(nameof(levelLoader.LoadNextLevel), this.loadDelay);

        this.gameObject.GetComponent<AudioSource>().PlayOneShot(this.successSFX);
        GameObject.Instantiate<ParticleSystem>(this.successVFX, collision.transform.position, Quaternion.identity);
    }

    public void StartCrashSequence()
    {
        this.isAlive = false;

        LevelLoader levelLoader = GameObject.FindObjectOfType<LevelLoader>();

        GameObject.FindObjectOfType<Movement>().enabled = false;

        levelLoader.Invoke(nameof(levelLoader.RestartLevel), this.loadDelay);

        AudioSource.PlayClipAtPoint(this.explosionSFX, Camera.main.transform.position);
        Instantiate<GameObject>(this.explosionVFX, this.gameObject.transform.position, Quaternion.identity);
        DismemberRocket();
    }

    private void StartCheckpointProcess(Collision collision)
    {
        if(this.isCheckpointActivated) { return; }

        this.isCheckpointActivated = true;
        //this.gameObject.GetComponent<AudioSource>().PlayOneShot(this.checkpointSFX);
        AudioSource.PlayClipAtPoint(this.checkpointSFX, Camera.main.transform.position);
        collision.gameObject.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        GameObject.FindObjectOfType<LevelLoader>().SetRocketSpawningCheckpoint(collision.gameObject.transform.position + new Vector3(0, 2.125f, 0));

    }

    private void DismemberRocket()
    {
        if (!this.isDismemberingProcessStarted)
            this.isDismemberingProcessStarted = true;
        else
            return;

        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;

        foreach(Transform child in transform)
        {
            if (child.gameObject.GetComponent<Collider>() == null)
                child.gameObject.AddComponent<SphereCollider>();

            child.gameObject.AddComponent<Rigidbody>().AddExplosionForce(Random.Range(5.0f, 20.0f),
                                                                         this.gameObject.transform.position,
                                                                         Random.Range(1.0f, 5.0f),
                                                                         Random.Range(4.0f, 10.0f),
                                                                         ForceMode.Impulse);

        }
    }

    private void OnDestroy()
    {
        if (Physics.gravity.x != 0)
            Physics.gravity = new Vector3(0.0f, Physics.gravity.y, Physics.gravity.z);
    }
}
