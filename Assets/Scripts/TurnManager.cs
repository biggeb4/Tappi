using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Tappo
{
    public int team;
    public GameObject tappo;
    public Tappo(int t,GameObject tap)
    {
        team = t;
        tappo = tap;
    }
}

public class TurnManager : MonoBehaviour {

    public static TurnManager instance = null;
    public GameObject[] teamTappi;
    public GameObject[] spawnPoints;
    public bool moving = true;
    private Tappo currentTappo;
    private Tappo[] spawnedTappi;
    private int currentTappoIndex = 0;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }
    // Use this for initialization
    void Start () {
        Assert.AreNotEqual(0, spawnPoints.Length);
        Assert.AreNotEqual(0, teamTappi.Length);
        int numberOfTappiForTeam = spawnPoints.Length / teamTappi.Length;
        spawnedTappi = new Tappo[numberOfTappiForTeam*teamTappi.Length];
        for (int i = 0; i < teamTappi.Length; i++)
        {
            for (int j = 0; j < numberOfTappiForTeam; j++)
            {
                spawnedTappi[i * numberOfTappiForTeam + j]= new Tappo(i,Instantiate(teamTappi[i], spawnPoints[i * numberOfTappiForTeam + j].transform.position, Quaternion.identity));
            }
        }
        currentTappoIndex = Random.Range(0, spawnedTappi.Length);
        currentTappo = spawnedTappi[currentTappoIndex];
        SetCamera(currentTappo.tappo);
        currentTappo.tappo.tag = "Active";
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PassTurn()
    {
        moving = true;

        currentTappoIndex++;

        if (currentTappoIndex >= spawnedTappi.Length)
        {
            currentTappo.tappo.tag = "Untagged";
            currentTappoIndex = 0;
            currentTappo = spawnedTappi[currentTappoIndex];
            if (currentTappo != null)
            {
                SetCamera(currentTappo.tappo);
                currentTappo.tappo.tag = "Active";
            }
            else
            {
                PassTurn();
            }
        }
        else
        {
            currentTappo = spawnedTappi[currentTappoIndex];
            if (currentTappo != null)
            {
                SetCamera(currentTappo.tappo);
            currentTappo.tappo.tag = "Active";
            }
            else
            {
                PassTurn();
            }
        }
    }

    public void Moved(GameObject go)
    {
        if (moving)
        {
            moving = false;
            currentTappo.tappo.tag = "Untagged";
            currentTappo.tappo.GetComponent<Player>().FireWeapon();
        }
        else
        {
            Destroy(go);
            ActivateCurrent();
            PassTurn();
        }
    }


    public void SetCamera(GameObject tappo)
    {
        Camera.main.GetComponent<FollowTappo>().SetTappoToFollow(tappo);
    }

    public void ActivateCurrent()
    {
        currentTappo.tappo.SetActive(true);
    }
}
