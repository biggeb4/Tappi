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
            GameObject go = Instantiate(weaponButton);
            go.GetComponent<Button>().onClick.AddListener(() => FireWeapon(i));
            go.GetComponentInChildren<Text>().text = Weapons[i].weaponName;
            go.transform.SetParent(inventory.transform);
            inventory.SetActive(true);
        }
    }
    public void FireWeapon(int weampnIndex)
    {
        inventory.SetActive(false);
        Weapons[weampnIndex].Fire(gameObject);
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
