using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    private Vector2 firstPressPos;
    private float fire_start_time = 0;
    private Vector3 firstPressHit;
    public bool inputAllowed = true;
    public float minimunAccelerationForJump=1.5f;
    private GameObject activeTappo = null;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (inputAllowed)
        {
            activeTappo=GameObject.FindWithTag("Active");
            if (activeTappo)
            {
                Swipe();
            }
        }
    }

    public void Swipe()
    {
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);

            if (t.position.x < Screen.width / 4)
            {
                Vector3 targetDirection = new Vector3(0f, -1.0f, 0f);
                activeTappo.GetComponent<TappoInputHandler>().Rotate(targetDirection);
            }
            else
            {
                if (t.position.x > Screen.width * 0.75f)
                {
                    Vector3 targetDirection = new Vector3(0f, 1.0f, 0f);
                    activeTappo.GetComponent<TappoInputHandler>().Rotate(targetDirection);
                }
                else
                {

                    if (t.phase == TouchPhase.Began)
                    {
                        firstPressPos = t.position;
                        fire_start_time = Time.time;
                    }

                    if (t.phase == TouchPhase.Ended && fire_start_time>0)
                    {
                        //save ended touch 2d point
                        Vector2 secondPressPos = new Vector2(t.position.x, t.position.y);

                        //create vector from the two points
                        Vector3 currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                        float swipeTime = Time.time - fire_start_time;
                        float distance = Vector2.Distance(firstPressPos, secondPressPos);
                        float force = distance / swipeTime;

                        //normalize the 2d vector
                        currentSwipe.Normalize();

                        //swipe upwards
                        if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                        {
                            if (distance > 1 && swipeTime > 0.2)
                            {
                                activeTappo.gameObject.GetComponent<TappoInputHandler>().MoveFoward(force);
                            }
                        }
                        //swipe down
                        if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                        {
                            if (distance > 1 && swipeTime > 0.2)
                            {
                                activeTappo.gameObject.GetComponent<TappoInputHandler>().Jump(force);
                            }
                        }
                        //swipe left
                        if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                        {
                        }
                        //swipe right
                        if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                        {
                        }
                        fire_start_time = 0;
                    }
                }
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
