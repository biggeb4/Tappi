using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public GameObject projectile;
    public string weaponName;
    public Sprite weaponImg;
    public bool bouncing = true;
    public bool explosive = false;
    public int tracking = 0;
    public bool canJump = true;
    public bool canMove = true;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public virtual void Fire(GameObject player)
    {
        player.GetComponent<Player>().Deactivate();
        GameObject spawnedProjectile = Instantiate(projectile, player.transform.position + player.GetComponent<Player>().offsetFire, player.transform.rotation);
        spawnedProjectile.tag = "Active";
        spawnedProjectile.GetComponent<TappoInputHandler>().canJump = canJump;
        spawnedProjectile.GetComponent<TappoInputHandler>().canMove = canMove;
        spawnedProjectile.GetComponent<Projectile>().bouncing = bouncing;
        spawnedProjectile.GetComponent<Projectile>().tracking = tracking;
        spawnedProjectile.GetComponent<Projectile>().explosive = explosive;
        TurnManager.instance.SetCamera(spawnedProjectile);
    }
}
