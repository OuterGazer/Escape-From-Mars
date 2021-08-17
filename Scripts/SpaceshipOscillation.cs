using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipOscillation : MonoBehaviour
{
    [SerializeField] private float cyclesFactor;
    [SerializeField] private Vector3 movementLength;
    private float movementFactor;
    private Vector3 originPos;


    // Start is called before the first frame update
    void Start()
    {
        this.originPos = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // GameDev.tv way

        if(Mathf.Approximately(this.cyclesFactor, 0)) { return; }    // prevents dividing by zero error when cyclesFactor is zero

        float cycles = Time.time / this.cyclesFactor; // The smaller the cycle factor, the fastest the obstacle will pendulate (e.g. 10.34 seconds / 10 factor = 1 cycle).
        float rawSinWave = Mathf.Sin(cycles * Mathf.PI * 2);  // returns numbers between 1 and -1 due to Time.time continuousy running

        this.movementFactor = (rawSinWave + 1f) / 2f;     // converts rawSinWave to a number between 0 and 1

        Vector3 offset = this.movementLength * this.movementFactor;   // movementLength is how far away the obstacles moves. movementFactor is the percentage of how far away or close is to the endpoint
        this.gameObject.transform.position = this.originPos + offset;     // Because we place the object at the starting position and movementFactor goes smoothly from 0 to 1, we just add the current movement factor to the origin position
    }
}
