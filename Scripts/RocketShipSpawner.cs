using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketShipSpawner : MonoBehaviour
{
    [SerializeField] EnemyRocketShip[] enemyRocket;
    [SerializeField] float maxTimeGap = 2.0f;
    [SerializeField] float minTimeGap = 1.0f;
    [SerializeField] float timeBetweenRockets;

    public bool shouldRocketsSpawn = true; // only for debugging purposes

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        do
        {
            int rocketNumber = Random.Range(0, this.enemyRocket.Length);
            EnemyRocketShip rocketToSpawn = this.enemyRocket[rocketNumber];
            GameObject.Instantiate<EnemyRocketShip>(rocketToSpawn, this.gameObject.transform.position, Quaternion.identity);

            this.timeBetweenRockets = Random.Range(this.minTimeGap, this.maxTimeGap);

            yield return new WaitForSeconds(this.timeBetweenRockets);

        } while (this.shouldRocketsSpawn);
    }
}
