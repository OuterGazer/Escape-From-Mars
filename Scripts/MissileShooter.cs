using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileShooter : MonoBehaviour
{
    [SerializeField] private Missile ammo;
    [SerializeField] private AudioClip shootSFX;

    private bool hasShotMissile = false;
    public void AllowShootingAgain()
    {
        this.hasShotMissile = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<CollisionHandler>() != null)
        {
            if (!this.hasShotMissile && other.GetComponent<CollisionHandler>().IsAlive)
            {
                this.hasShotMissile = true;
                Missile missile = GameObject.Instantiate<Missile>(this.ammo, this.gameObject.transform.position, Quaternion.identity);
                this.gameObject.GetComponent<AudioSource>().PlayOneShot(this.shootSFX);
                missile.SetShotOrigin(this);
            }
                
        }
    }
}
