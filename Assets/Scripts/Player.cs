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
    public Vector3 offsetFire;
    public float maxFall =-2;
    // Use this for initialization
    void Awake()
    {
        inventory = GameObject.FindGameObjectWithTag("Inventory");
    }
    void Start ()
    {
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
            GameObject go = Instantiate(weaponButton,new Vector3((i+1)* (520 / (Weapons.Count + 1)), inventory.transform.position.y,0f),Quaternion.identity,inventory.transform);
            int wpIndex = i;
            go.GetComponent<Button>().onClick.AddListener(() => FireWeapon(wpIndex));
            //go.GetComponentInChildren<Text>().text = Weapons[i].weaponName;
            go.GetComponentInChildren<Image>().sprite = Weapons[i].weaponImg;
            Debug.Log(Weapons[i].weaponName);
            //go.transform.SetParent(inventory.transform);
            inventory.SetActive(true);
        }
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
            if (hitted.GetComponent<Weapon>())
            {
                Weapons.Add(hitted.GetComponent<Weapon>());
                hitted.SetActive(false);
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
        Destroy(gameObject);
    }
}
