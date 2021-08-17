using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Parameters to tune the different settings of the gameobject

    [Header("Movement Settings")]
    [SerializeField] float thrustForce = 20.0f;
    [SerializeField] float rotatingSensitivity;
    [SerializeField] float maxRotDeadzone;
    [SerializeField] float minRotDeadzone;
    private float shipRotAngle = 0.0f;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip thrustSFX;

    [Header("Particle Effects")]
    [SerializeField] GameObject mainEngineVFX;
    [SerializeField] GameObject leftEngineVFX;
    [SerializeField] GameObject rightEngineVFX;

    LayerMask rocketPadMask = 1 << 3;//LayerMask.GetMask("Rocket Pad");

    private bool isGrounded = true;


    //Cached references to components or other objects for readability or speed

    private Rigidbody rocketRB;
    private AudioSource audioSource;


    //state variables to control the different states

    private bool thrustHasBeenPressed = false;
    private bool rotateHasBeenActivated = false;


    // Start is called before the first frame update
    void Start()
    {
        this.rocketRB = this.gameObject.GetComponent<Rigidbody>();
        this.audioSource = this.gameObject.GetComponent<AudioSource>();

        this.mainEngineVFX.SetActive(false);

        this.LockAndHideCursor();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();

        ProcessRotation();
    }

    private void FixedUpdate()
    {
        ThrustShipForward();

        CheckIfGrounded();

        RotateShip();
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!this.thrustHasBeenPressed)
                this.thrustHasBeenPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space) ||
                 (!Input.GetKey(KeyCode.Space) && this.thrustHasBeenPressed))
        {
            if (this.thrustHasBeenPressed)
                this.thrustHasBeenPressed = false;
        }
    }

    private void ProcessRotation()
    {
        if (Input.GetAxis("Mouse X") > this.maxRotDeadzone ||
            Input.GetAxis("Mouse X") < this.minRotDeadzone)
        {
            this.rotateHasBeenActivated = true;
            //this.shipRotAngle = Input.GetAxis("Mouse X") * this.rotatingSensitivity;
        }
        else
        {
            this.rotateHasBeenActivated = false;
        }
    }

    private void ThrustShipForward()
    {
        if (this.thrustHasBeenPressed)
        {
            this.rocketRB.AddRelativeForce(Vector3.up * this.thrustForce, ForceMode.Force);

            this.PlayThrustingEffects();
        }
        else
        {
            this.StopThrustingEffects();
        }

    }

    private void CheckIfGrounded()
    {
        this.isGrounded = Physics.Raycast(this.gameObject.transform.position, Vector3.down, 2.10f, this.rocketPadMask);
    }

    private void RotateShip()
    {
        if (this.rotateHasBeenActivated && !this.isGrounded)
        {
            float shipRotAngle = Input.GetAxis("Mouse X") * this.rotatingSensitivity;

            this.rocketRB.AddRelativeTorque(Vector3.forward * shipRotAngle, ForceMode.Force);

            this.PlayRotatingEffects(this.shipRotAngle);
        }
        else
        {
            this.rocketRB.angularVelocity = Vector3.zero;

            this.StopRotatingEffects();
        }
    }

    private void PlayThrustingEffects()
    {
        if (!this.audioSource.isPlaying)
            this.audioSource.PlayOneShot(this.thrustSFX);

        if (!this.mainEngineVFX.activeSelf)
            this.mainEngineVFX.SetActive(true);
    }

    private void StopThrustingEffects()
    {
        if (this.audioSource.isPlaying)
            this.audioSource.Stop();

        if (this.mainEngineVFX.activeSelf)
            this.mainEngineVFX.SetActive(false);
    }

    private void PlayRotatingEffects(float shipRotAngle)
    {
        if (!this.leftEngineVFX.activeSelf && shipRotAngle > 0)
            this.leftEngineVFX.SetActive(true);

        if (!this.rightEngineVFX.activeSelf && shipRotAngle < 0)
            this.rightEngineVFX.SetActive(true);
    }

    private void StopRotatingEffects()
    {
        if (this.leftEngineVFX.activeSelf)
            this.leftEngineVFX.SetActive(false);

        if (this.rightEngineVFX.activeSelf)
            this.rightEngineVFX.SetActive(false);
    }

    private void LockAndHideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        if (this.audioSource.isPlaying)
            this.audioSource.Stop();
    }
}
