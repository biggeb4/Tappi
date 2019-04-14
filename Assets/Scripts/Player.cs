using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public GameObject weaponButton;
    private GameObject inventory;
    public GameObject explosionPrefab;
    public int team;
    public List<Weapon> Weapons;
    private List<string> WeaponsNames = new List<string>();
    public Vector3 offsetFire;
    public float maxFall =-10;
    public float pusingWall = 10;
    // Use this for initialization
    void Awake()
    {
        inventory = GameObject.FindGameObjectWithTag("Inventory");
    }
    void Start ()
    {
        for(int i =0; i<Weapons.Count; i++)
        {
            WeaponsNames.Add(Weapons[i].weaponName);
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (transform.position.y < maxFall)
        {
            LoseLife();
        }
    }

    public void ChooseWeapon()
    {
        foreach (Transform child in inventory.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < Weapons.Count; i++)
        {
            GameObject go = Instantiate(weaponButton,new Vector3(inventory.transform.position.x-((Weapons.Count/2)*50) + i*50, inventory.transform.position.y,0f),Quaternion.identity,inventory.transform);
            int wpIndex = i;
            go.GetComponent<Button>().onClick.AddListener(() => FireWeapon(wpIndex));
            go.GetComponentInChildren<Image>().sprite = Weapons[i].weaponImg;
        }
        inventory.SetActive(true);
    }
    public void FireWeapon(int weampnIndex)
    {
        inventory.SetActive(false);
        Weapons[weampnIndex].Fire(gameObject);
        TurnManager.instance.SetInputAllowed(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        gameObject.layer = 9;
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject hitted = collision.gameObject;
        if (hitted.GetComponent<Projectile>() || hitted.tag=="CanDestroy")
        {
            LoseLife();
        }
        else
        {
            if (hitted.GetComponent<Weapon>() && !WeaponsNames.Contains(hitted.GetComponent<Weapon>().weaponName))
            {
                Weapons.Add(hitted.GetComponent<Weapon>());
                WeaponsNames.Add(hitted.GetComponent<Weapon>().weaponName);
                hitted.SetActive(false);
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        GameObject hitted = collision.gameObject;
        if (hitted.tag == "Wall")
        {
            // Calculate Angle Between the collision point and the player
            Vector3 dir = collision.contacts[0].point - transform.position;
            // We then get the opposite (-Vector3) and normalize it
            dir = -dir.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the player

            float pointingUp = Vector3.Dot(dir.normalized, Vector3.up);
            if (pointingUp<0)
            {
                GetComponent<Rigidbody>().AddForce(dir * pusingWall);
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        GameObject hitted = collision.gameObject;
        if (hitted.GetComponent<Projectile>() || hitted.tag == "CanDestroy")
        {
            LoseLife();
        }
    }

    public void LoseLife()
    {
        Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
        TurnManager.instance.Death(gameObject);
        foreach (Transform child in transform)
        {
            child.parent = null;
        }
        Destroy(gameObject);
    }
}
