using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveDevice : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private float openingSpeed = 3.0f;
    [SerializeField] private Vector3 targetPos;
    private Vector3 originPos;
    [SerializeField] AudioClip doorOpeningSFX;

    [Header("Fan Settings")]
    [SerializeField] float rotSpeed = 10.0f;

    private Rigidbody fanRB;

    private bool shouldOpenDoor = false;
    private bool shouldRotateFan = false;

    public void Activate()
    {
        if (this.gameObject.CompareTag("Horizontal Door"))
        {
            this.shouldOpenDoor = true;
            this.originPos = this.gameObject.transform.position;
            this.gameObject.GetComponent<AudioSource>().PlayOneShot(this.doorOpeningSFX);
        }
        else if (this.gameObject.CompareTag("Fan"))
        {
            this.shouldRotateFan = true;
            this.fanRB = this.gameObject.GetComponent<Rigidbody>();
            this.gameObject.GetComponent<AudioSource>().Play(); ;

            GameObject[] sideWinds = GameObject.FindGameObjectsWithTag("Wind Fan");
            foreach(GameObject item in sideWinds)
            {
                item.GetComponent<SideWind>().ActivatePermanentWind();
            }
        }
    }

    private void Update()
    {
        if (this.shouldOpenDoor)
        {
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position,
                                                                     this.targetPos,
                                                                     this.openingSpeed * Time.deltaTime);

            this.openingSpeed = 15.0f * ((this.gameObject.transform.position.x - this.targetPos.x) /
                                        (this.originPos.x - this.targetPos.x));

            if (Mathf.Approximately(this.gameObject.transform.position.x, this.targetPos.x))
            {
                this.shouldOpenDoor = false;
            }

        }
    }

    private void FixedUpdate()
    {
        if (this.shouldRotateFan)
        {
            this.fanRB.MoveRotation(this.fanRB.rotation * Quaternion.Euler(0.0f, 0.0f, this.rotSpeed * Time.fixedDeltaTime));
        }
    }
}
