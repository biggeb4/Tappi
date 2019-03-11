using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    private Vector2 firstPressPos;
    private float fire_start_time = 0;
    private Vector3 firstPressHit;
    public bool inputAllowed = true;
    public float minimunAccelerationForJump=1.5f;
    protected Joystick RotationJoystick;

    // Use this for initialization
    void Start()
    {
        RotationJoystick = GameObject.Find("RotationJoystick").GetComponent<Joystick>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputAllowed)
        {
            Swipe();
            Rotate();
            Jump();
        }
    }

    public void Swipe()
    {
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);
            Vector2 vTouchPos = t.position;
            if (t.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(vTouchPos);

                // Your raycast handling
                RaycastHit vHit;
                if (Physics.Raycast(ray.origin, ray.direction, out vHit))
                {
                    firstPressHit = vHit.point;
                    //save began touch 2d point
                    firstPressPos = vTouchPos;
                    fire_start_time = Time.time;
                }
            }

            if (t.phase == TouchPhase.Moved && fire_start_time > 0)
            {
                // The ray to the touched object in the world
                Ray ray = Camera.main.ScreenPointToRay(vTouchPos);

                // Your raycast handling
                RaycastHit vHit;
                if (Physics.Raycast(ray.origin, ray.direction, out vHit))
                {
                    if (vHit.transform.tag == "Active")
                    {
                        float distance = Vector2.Distance(firstPressPos, vTouchPos);
                        float swipeTime = Time.time - fire_start_time;
                        Vector3 direction = vHit.transform.position - new Vector3(firstPressHit.x, vHit.transform.position.y, firstPressHit.z);
                        float force = distance / swipeTime;
                        vHit.transform.gameObject.GetComponent<TappoInputHandler>().Move(direction.normalized, force);
                        
                        fire_start_time = 0;
                    }
                }
            }

            if (t.phase == TouchPhase.Ended)
            {
                fire_start_time = 0;
            }
        }
    }
    private void Jump()
    {
        if (Input.acceleration.magnitude> minimunAccelerationForJump)
        {
            Debug.Log("accel:" + Input.acceleration.magnitude);
            GameObject tappoActive = GameObject.FindGameObjectWithTag("Active");
            tappoActive.GetComponent<TappoInputHandler>().Jump(Input.acceleration);
        }
    }
    private void Rotate()
    {
        if (RotationJoystick.Horizontal != 0 || RotationJoystick.Vertical != 0)
        {
            Vector3 targetDirection = new Vector3(RotationJoystick.Horizontal, 0f, RotationJoystick.Vertical);

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            GameObject tappoActive = GameObject.FindGameObjectWithTag("Active");
            if (tappoActive)
            {
                tappoActive.GetComponent<TappoInputHandler>().Rotate(targetRotation);
            }
        }
    }
    public void Zoom()
    {
        if (inputAllowed)
        {
            inputAllowed = false;
            GameObject.FindGameObjectWithTag("MapCamera").GetComponent<Camera>().enabled = true;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = false;
        }
        else
        {
            inputAllowed = true;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = true;
            GameObject.FindGameObjectWithTag("MapCamera").GetComponent<Camera>().enabled = false;
        }

    }
}
