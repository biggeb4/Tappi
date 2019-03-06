using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    private Vector2 firstPressPos;
    private float fire_start_time = 0;
    private Vector3 firstPressHit;
    public bool swipeAllowed = true;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        if (swipeAllowed)
        {
                Swipe();
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
                        vHit.transform.gameObject.GetComponent<SwipeHit>().Move(direction.normalized, force);

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
    public void Zoom()
    {
        if (swipeAllowed)
        {
            swipeAllowed = false;
            GameObject.FindGameObjectWithTag("MapCamera").GetComponent<Camera>().enabled = true;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = false;
        }
        else
        {
            swipeAllowed = true;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().enabled = true;
            GameObject.FindGameObjectWithTag("MapCamera").GetComponent<Camera>().enabled = false;
        }

    }
}
