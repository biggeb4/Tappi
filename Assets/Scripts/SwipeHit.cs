using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeHit : MonoBehaviour {

    public float maxSpeed = 100;
    public float speed = 8;
    
    public void Move(Vector3 direction,float force)
    {
        force = force > maxSpeed ? maxSpeed : force;
        GetComponent<Rigidbody>().AddForce(direction * force * speed);
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        while (!GetComponent<Rigidbody>().IsSleeping())
        {
            yield return null;
        }
        TurnManager.instance.Moved(gameObject);
    }
}
