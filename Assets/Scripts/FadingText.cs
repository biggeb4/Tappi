using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingText : MonoBehaviour {

    public float fadeTime=2;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    IEnumerator Wait()
    {
        yield return new WaitForSeconds(fadeTime);
        gameObject.GetComponent<Text>().text = "";
    }
    
    public void ChangeText(string text)
    {
        gameObject.GetComponent<Text>().text = text;
        StartCoroutine(Wait());
    }
}
