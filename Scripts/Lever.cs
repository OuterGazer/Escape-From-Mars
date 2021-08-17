using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] InteractiveDevice targetObject;
    [SerializeField] Light lightbulb;

    [SerializeField] AudioClip switchSFX;

    private bool isLeverActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if(this.isLeverActivated) { return; }

        if (other.CompareTag("Player"))
        {
            this.gameObject.transform.Rotate(Vector3.forward, -90f);
            this.targetObject.SendMessage("Activate");
            this.lightbulb.color = Color.green;
            this.gameObject.GetComponent<AudioSource>().PlayOneShot(this.switchSFX);
            this.isLeverActivated = true;
        }
    }
}
