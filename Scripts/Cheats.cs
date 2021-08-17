using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    [SerializeField] private Collider rocketCol;
    [SerializeField] private Collider baseCol;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            this.gameObject.GetComponent<LevelLoader>().LoadNextLevel();

        if (Input.GetKeyDown(KeyCode.C))
        {
            this.rocketCol.enabled = !this.rocketCol.enabled;
            this.baseCol.enabled = !this.baseCol.enabled;
        }
    }
}
