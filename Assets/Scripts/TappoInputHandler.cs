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
    public bool canMove = true;
    public float JumpUpComponent = 0.75f;
    public float JumpForwardComponent = 0.25f;


    public void Move(Vector3 direction,float force)
    {
        if (canMove)
        {
            force = force > maxSpeed ? maxSpeed : force;
            GetComponent<Rigidbody>().AddForce(direction * force * speed);
            StartCoroutine(Wait());
        }
    }

    public void MoveFoward( float force)
    {
        if (canMove)
        {
            force *= speed;
            force = force > maxSpeed ? maxSpeed : force;
            GetComponent<Rigidbody>().AddForce(transform.forward * force);
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        TurnManager.instance.Moving();
        while (!GetComponent<Rigidbody>().IsSleeping())
        {
            yield return null;
        }
        if (Mathf.Abs(transform.eulerAngles.x) > 90 || Mathf.Abs(transform.eulerAngles.z) > 90)
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
        TurnManager.instance.Moved(gameObject);
    }

    public void Rotate(Vector3 targetRotation)
    {
        transform.Rotate(targetRotation * speedRotation);
    }

    public void Jump(float force)
    {
        if (canJump)
        {
            force = force*jumpSpeed;
            force = force > maxJumpSpeed ? maxJumpSpeed : force;
            GetComponent<Rigidbody>().AddForce(transform.up * force* JumpUpComponent);
            GetComponent<Rigidbody>().AddForce(transform.forward * force* JumpForwardComponent);
            StartCoroutine(Wait());
        }
    }
}
