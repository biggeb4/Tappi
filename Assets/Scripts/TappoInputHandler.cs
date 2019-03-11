using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TappoInputHandler : MonoBehaviour {

    public float maxSpeed = 200;
    public float speed = 8;
    public float speedRotation = 1.5f;
    public float jumpSpeed = 1;
    public float maxJumpSpeed = 200;
    public bool canJump = true;
    public float junpAngle = -45;
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

    public void Rotate(Quaternion targetRotation)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, speedRotation*Time.deltaTime * 50);
    }

    public void Jump(Vector3 direction)
    {
        Debug.Log("jump");
        //direction = Quaternion.AngleAxis(junpAngle, Vector3.up) * direction;
        float force = (direction * jumpSpeed).magnitude;
        force = force > maxSpeed ? maxSpeed : force;
        GetComponent<Rigidbody>().AddForce(direction * force);
        //StartCoroutine(Wait());
    }
}
