using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaponSpawner : MonoBehaviour {

    public GameObject[] WeaposToSpawn;
    public float turnBeforeSpanw = 1;

    private float turnCounter = 1;

    // Update is called once per frame
    public void SpawnWeapon()
    {
        if (turnCounter == 0) {
            Vector3 rndPosWithin;
            rndPosWithin = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            rndPosWithin = transform.TransformPoint(rndPosWithin * .5f);
            int randomIndex = Random.Range(0, WeaposToSpawn.Length - 1);
            if (randomIndex < WeaposToSpawn.Length && randomIndex >= 0)
            {
                Instantiate(WeaposToSpawn[randomIndex], rndPosWithin, transform.rotation);
            }
            else
            {
                Debug.Log(randomIndex);
                Debug.Log(WeaposToSpawn.Length);
            }
            turnCounter = turnBeforeSpanw;
        }
        else
        {
            turnCounter--;
        }
    }
}
