using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTrailDestroyer : MonoBehaviour
{
    private void Start()
    {
        if (!this.gameObject.name.Contains("Clone"))
        {
            this.gameObject.GetComponent<Animator>().speed = 0f;
        }
    }

    public void DestroyTrail()
    {
        if(this.gameObject.name.Contains("Clone"))
            GameObject.Destroy(this.gameObject);
    }
}
