using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public bool bouncing=true;
    public bool explosive=false;
    public GameObject explosionPrefab;
    public float explosionTime = 1;
    public int tracking=0;
    public float maxFall = -10;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y < maxFall)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!bouncing && collision.gameObject.tag == "Wall")
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void Explode()
    {
        if (tracking > 0)
        {
            tracking--;
        }
        else
        {
            if (explosive)
            {
                GameObject exploasion = Instantiate(explosionPrefab, transform.position, transform.rotation);
                Destroy(exploasion, explosionTime);
                StartCoroutine(Wait());
            }
            else
            {
                StartCoroutine(Wait());
            }
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(explosionTime);
        Destroy(gameObject, 0.2f);
        TurnManager.instance.PassTurn();
    }
}
