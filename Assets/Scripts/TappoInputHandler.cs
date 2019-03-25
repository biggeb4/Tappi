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
    public float junpAngle = -45;


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
            force = force > maxSpeed ? maxSpeed : force;
            GetComponent<Rigidbody>().AddForce(transform.forward * force * speed);
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
        if (Mathf.Abs(transform.eulerAngles.x) > 100)
        {
            transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,transform.eulerAngles.z);
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
            Vector3 direction = transform.forward;
            if (Mathf.Abs(transform.eulerAngles.x) > 100)
            {
                direction = Quaternion.AngleAxis(-junpAngle, Vector3.right) * direction;
            }
            else
            {
                direction = Quaternion.AngleAxis(junpAngle, Vector3.right) * direction;
            }
            force = force+(direction * jumpSpeed).magnitude;
            force = force > maxJumpSpeed ? maxJumpSpeed : force;
            GetComponent<Rigidbody>().AddForce(direction * force);
            StartCoroutine(Wait());
        }
    }
}
