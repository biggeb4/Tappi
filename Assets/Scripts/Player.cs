using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public GameObject explosionPrefab;
    public int team;
    public Weapon[] Weapons;
    public Vector3 offsetFire;
    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update () {
    }
    
    public void FireWeapon()
    {
        Weapons[0].Fire(gameObject);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        gameObject.layer = 9;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Active" && !TurnManager.instance.IsCurrentTappo(gameObject))
        {
            LoseLife();
        }
    }

    public void LoseLife()
    {
        Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
        TurnManager.instance.Death(gameObject);
        Destroy(gameObject);
    }
}
