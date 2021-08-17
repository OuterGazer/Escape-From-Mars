using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float movementSpeed = 15.0f;
    [SerializeField] private float maxHomingTime = 1.0f;
    private float homingTime;

    [Header("Effects")]
    [SerializeField] private GameObject explosionVFX;
    [SerializeField] private AudioClip explosionSFX;
    [SerializeField] private Light homingLight;

    private MissileShooter shotOrigin;
    private Rigidbody missileRB;

    // Start is called before the first frame update
    void Start()
    {
        this.missileRB = this.gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        this.homingTime += Time.deltaTime;

        // Follow player for maxHomingTime, then stop following and go straight.
        if (this.homingTime > this.maxHomingTime)
            return;

        Transform playerPos = GameObject.FindObjectOfType<Movement>().gameObject.transform;

        this.gameObject.transform.LookAt(playerPos, Vector3.up);

        if (this.homingTime > 5.0f)
            DestroyMissile();
    }

    private void FixedUpdate()
    {
        if (this.homingTime > this.maxHomingTime)
        {
            this.missileRB.MovePosition(this.missileRB.position + this.gameObject.transform.forward * this.movementSpeed * Time.fixedDeltaTime);
            this.homingLight.color = Color.red;
            return;
        }
            

        Vector3 playerPos = GameObject.FindObjectOfType<Movement>().gameObject.transform.position;

        Vector3 dirToPlayer = (playerPos - this.missileRB.position).normalized; 
        //float angleToPlayer = Vector3.SignedAngle(playerPos, this.gameObject.transform.position, -Vector3.forward);

        this.missileRB.MovePosition(this.missileRB.position + dirToPlayer * this.movementSpeed * Time.fixedDeltaTime);
        //this.missileRB.rotation *= Quaternion.AngleAxis(angleToPlayer, Vector3.forward);

        //this.missileRB.rotation.SetFromToRotation(this.missileRB.position, dirToPlayer);
        //this.missileRB.rotation.SetLookRotation(dirToPlayer);
        //Quaternion rotToPlayer = Quaternion.LookRotation(playerPos);
        //this.missileRB.rotation = rotToPlayer;
        //
        //this.missileRB.MoveRotation(this.missileRB.rotation * Quaternion.Euler(0, 0, angleToPlayer));
    }

    public void SetShotOrigin(MissileShooter shotOrigin)
    {
        this.shotOrigin = shotOrigin;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Environment"))
        {
            DestroyMissile();
        }
        else if (other.CompareTag("Player") && other.GetComponent<CollisionHandler>() != null)
        {
            if(other.GetComponent<CollisionHandler>().IsAlive)
                other.GetComponent<CollisionHandler>().StartCrashSequence();
            
            DestroyMissile();
        }
        
    }

    public void DestroyMissile()
    {
        Instantiate<GameObject>(this.explosionVFX, this.gameObject.transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(this.explosionSFX, Camera.main.transform.position);
        GameObject.Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        this.shotOrigin.AllowShootingAgain();
    }
}
