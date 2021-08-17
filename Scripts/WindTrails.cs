using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTrails : MonoBehaviour
{
    [SerializeField] private TrailRenderer[] windTrails;
    [SerializeField] private float bigGapBetweenTrails = 3.0f;
    [SerializeField] private float smallGapBetweenTrails = 1.0f;

    public bool spawnWindTrails = true; //for debug purposes only

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        do
        {
            TrailRenderer wind1;
            TrailRenderer wind2;

            int trail1 = Random.Range(0, windTrails.Length);
            int trail2;

            do
            {
                trail2 = Random.Range(0, windTrails.Length);

            } while (trail2 == trail1);

            wind1 = this.windTrails[trail1];
            wind2 = this.windTrails[trail2];

            GameObject.Instantiate<TrailRenderer>(wind1, wind1.transform.position, Quaternion.identity);
            GameObject.Instantiate<TrailRenderer>(wind2, wind2.transform.position, Quaternion.identity);


            yield return new WaitForSeconds(this.bigGapBetweenTrails);

            TrailRenderer wind3;
            int trail3;

            do
            {
                trail3 = Random.Range(0, windTrails.Length);

            } while ((trail3 == trail1) || (trail3 == trail2));
            

            wind3 = this.windTrails[trail3];

            GameObject.Instantiate<TrailRenderer>(wind3, wind3.transform.position, Quaternion.identity);

            yield return new WaitForSeconds(this.smallGapBetweenTrails);

        } while(this.spawnWindTrails);
    }
}
