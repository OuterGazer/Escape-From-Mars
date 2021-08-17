using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeapon : MonoBehaviour
{
    [SerializeField] private Ammo projectile;
    [SerializeField] private float timeBetweenShots = 1.50f;
    [SerializeField] private float timeBetweenBursts = 0.50f;
    [SerializeField] private AudioClip shotSFX;

    private AudioSource audioSource;

    public bool shouldShoot = true;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        this.audioSource = this.gameObject.GetComponent<AudioSource>();

        do
        {
            GameObject.Instantiate<Ammo>(this.projectile, this.gameObject.transform.position, Quaternion.identity);
            this.audioSource.PlayOneShot(this.shotSFX);

            if(this.timeBetweenBursts > 0)
            {
                yield return new WaitForSeconds(this.timeBetweenBursts);

                GameObject.Instantiate<Ammo>(this.projectile, this.gameObject.transform.position, Quaternion.identity);
                this.audioSource.PlayOneShot(this.shotSFX);
            }

            yield return new WaitForSeconds(this.timeBetweenShots);

        } while (this.shouldShoot);
    }
}
