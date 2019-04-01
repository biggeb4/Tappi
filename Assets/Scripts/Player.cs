using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public GameObject weaponButton;
    private GameObject inventory;
    public GameObject explosionPrefab;
    public int team;
    public Weapon[] Weapons;
    public Vector3 offsetFire;
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
    }

    public void ChooseWeapon()
    {
        for (int i = 0; i < Weapons.Length; i++)
        {
            GameObject go = Instantiate(weaponButton,new Vector3(i*50+285/Weapons.Length, inventory.transform.position.y,0f),Quaternion.identity,inventory.transform);
            int wpIndex = i;
            go.GetComponent<Button>().onClick.AddListener(() => FireWeapon(wpIndex));
            //go.GetComponentInChildren<Text>().text = Weapons[i].weaponName;
            go.GetComponentInChildren<Image>().sprite = Weapons[i].weaponImg;
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
        if (collision.gameObject.GetComponent<Projectile>() || collision.gameObject.tag=="CanDestroy")
        {
            LoseLife();
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<Projectile>() || collision.gameObject.tag == "CanDestroy")
        {
            if (TurnManager.instance.IsCurrentTappo(gameObject))
            {
                TurnManager.instance.PassTurn();
            }
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
