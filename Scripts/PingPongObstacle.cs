using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongObstacle : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 20.0f;
    [SerializeField] private float movementLength = 21.5f;
    [SerializeField] private float threshold = 0.96f;
    public float Threshold => this.threshold;

    [SerializeField] private float rotationSpeedX = 5.0f;
    [SerializeField] private float rotationSpeedY = 10.0f;
    [SerializeField] private float rotationSpeedZ = 8.0f;

    private Vector3 originPos;
    private float currentPos;
    public float CurrentPos => this.currentPos;
    private Vector3 endPos;
    static float dampFactor = 0f;
    private Rigidbody stoneRB;

    // Start is called before the first frame update
    void Start()
    {
        this.originPos = this.gameObject.transform.position - new Vector3(this.movementLength / 2f, 0, 0);
        this.endPos = this.gameObject.transform.position + new Vector3(this.movementLength / 2f, 0, 0);

        this.stoneRB = this.gameObject.GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        MoveHorizontally();

        RotateRigidbody();
    }

    private void MoveHorizontally()
    {
        this.currentPos = Mathf.Abs((this.stoneRB.position.x - this.originPos.x) / (this.endPos.x - this.originPos.x));
        dampFactor = Mathf.Lerp(0.0f, 180.0f, this.currentPos);

        this.stoneRB.MovePosition(this.stoneRB.position + new Vector3(Mathf.Sin(Mathf.Deg2Rad * dampFactor) * this.movementSpeed * Time.fixedDeltaTime, 0, 0));

        if (this.currentPos > this.threshold)
        {
            Vector3 temp = this.endPos;
            this.endPos = this.originPos;
            this.originPos = temp;
            this.movementSpeed *= -1f;
        }

        // GameDev.tv way

        // if(Mathf.Approximately(this.cyclesFactor, 0)) { return; }    // prevents dividing by zero error when cyclesFactor is zero

        // float cycles = Time.time / this.cyclesFactor // The smaller the cycle factor, the fastest the obstacle will pendulate (e.g. 10.34 seconds / 10 factor = 1 cycle).
        // float rawSinWave = Mathf.Sin(cycles * Mathf.PI * 2)  // returns numbers between 1 and -1 due to Time.time continuousy running

        // this.movementFactor = (rawSinWave + 1f) / 2f     // converts rawSinWave to a number between 0 and 1

        // Vector3 offset = this.movementLength * this.movementFactor   // movementLength is how far away the obstacles moves. movementFactor is the percentage of how far away or close is to the endpoint
        // this.gameObject.transform.position = this.originPos + offset     // Because we place the object at the starting position and movementFactor goes smoothly from 0 to 1, we just add the current movement factor to the origin position
    }

    private void RotateRigidbody()
    {
        this.stoneRB.MoveRotation(this.stoneRB.rotation * Quaternion.Euler(new Vector3(rotationSpeedX, rotationSpeedY, rotationSpeedZ) * Time.fixedDeltaTime));
    }
}
