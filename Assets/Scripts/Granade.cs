using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : Weapon
{

    public GameObject projectile;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void Fire(GameObject player)
    {
        player.GetComponent<Player>().Deactivate();
        GameObject spawnedProjectile = Instantiate(projectile, player.transform.position + player.GetComponent<Player>().offsetFire, player.transform.rotation);
        spawnedProjectile.tag = "Active";
        spawnedProjectile.GetComponent<TappoInputHandler>().canJump = true;
        spawnedProjectile.GetComponent<TappoInputHandler>().canMove = false;

        TurnManager.instance.SetCamera(spawnedProjectile);
    }
}
