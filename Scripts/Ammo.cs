using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = -50.0f;

    private Rigidbody projectileRB;

    // Start is called before the first frame update
    void Start()
    {
        this.projectileRB = this.gameObject.GetComponent<Rigidbody>();
    }
    
    void FixedUpdate()
    {
        this.projectileRB.MovePosition(this.projectileRB.position + new Vector3(0, 0, this.projectileSpeed * Time.fixedDeltaTime));
    }

    private void Update()
    {
        if (this.gameObject.transform.position.z < -25.0f)
            GameObject.Destroy(this.gameObject);
    }
}
