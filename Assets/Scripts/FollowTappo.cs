using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTappo : MonoBehaviour {

    public Vector3 offset;
    public Vector3 angle;
    public Quaternion rotationOffset;
    private GameObject activeTappo=null;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetTappoToFollow(GameObject tappo)
    {
        transform.parent = null;
        transform.position = Vector3.zero;
        activeTappo = tappo;
        transform.position = activeTappo.transform.position + (activeTappo.transform.forward*offset.z) + (activeTappo.transform.up * offset.y);
        transform.eulerAngles = activeTappo.transform.eulerAngles + angle;
        transform.parent = activeTappo.transform;
    }
}
