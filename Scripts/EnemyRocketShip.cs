using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRocketShip : MonoBehaviour
{
    [SerializeField] private float maxRocketSpeed = 25.0f;
    [SerializeField] private float minRocketSpeed = 15.0f;
    private float movementSpeed;

    private Rigidbody rocketRB;

    // Start is called before the first frame update
    void Start()
    {
        this.movementSpeed = Random.Range(this.minRocketSpeed, this.maxRocketSpeed);

        this.rocketRB = this.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.transform.position.y > 30.0f)
            GameObject.Destroy(this.gameObject);
    }

    private void FixedUpdate()
    {
        this.rocketRB.MovePosition(this.rocketRB.position + new Vector3(0, this.movementSpeed * Time.fixedDeltaTime, 0));
    }
}
