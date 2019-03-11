using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTappo : MonoBehaviour {

    public Vector3 offset;
    public Quaternion rotationOffset;
    private GameObject activeTappo=null;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LateUpdate()
    {
        if (activeTappo)
        {
            // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
            transform.position = activeTappo.transform.position + offset;
            transform.eulerAngles=new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, activeTappo.transform.eulerAngles.y);
        }
    }

    public void SetTappoToFollow(GameObject tappo)
    {
        activeTappo = tappo;
    }
}
