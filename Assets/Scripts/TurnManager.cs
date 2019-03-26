﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private GameObject currentTappo;
    private GameObject pauseMenu;
    private List<GameObject> spawnedTappi;
    private int currentTappoIndex = 0;
    private Camera tappoCamera;

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
        tappoCamera = Camera.main;
        Assert.AreNotEqual(0, spawnPoints.Length);
        Assert.AreNotEqual(0, teamTappi.Length);
        int numberOfTappiForTeam = spawnPoints.Length / teamTappi.Length;
        spawnedTappi = new List<GameObject>(numberOfTappiForTeam* teamTappi.Length);
        for (int j = 0; j < numberOfTappiForTeam; j++)
        {
            for (int i = 0; i < teamTappi.Length; i++)
            {
                teamTappi[i].GetComponent<Player>().team=i;
                spawnedTappi.Add(Instantiate(teamTappi[i], spawnPoints[i * numberOfTappiForTeam + j].transform.position, Quaternion.identity));
            }
        }
        currentTappoIndex = Random.Range(0, spawnedTappi.Count);
        currentTappo = spawnedTappi[currentTappoIndex];
        SetCamera(currentTappo);
        currentTappo.tag = "Active";
        GameObject.FindGameObjectWithTag("NumberTappiText").GetComponent<Text>().text = "Tappi rimanenti: "+spawnedTappi.Count;
        GameObject.FindGameObjectWithTag("PlayerTurnText").GetComponent<Text>().text = "Giocatore: " + (currentTappo.GetComponent<Player>().team + 1);
        GameObject.FindGameObjectWithTag("Inventory").SetActive(false);
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        pauseMenu.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PassTurn()
    {
        moving = true;

        currentTappoIndex++;

        if (currentTappoIndex >= spawnedTappi.Count)
        {
            GameObject.FindGameObjectWithTag("PlayerTurnText").GetComponent<Text>().text="Giocatore:"+(currentTappo.GetComponent<Player>().team+1);
            currentTappoIndex = 0;
            currentTappo = spawnedTappi[currentTappoIndex];
            if (currentTappo != null)
            {
                SetCamera(currentTappo);
                currentTappo.tag = "Active";
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
                SetCamera(currentTappo);
                currentTappo.tag = "Active";
            }
            else
            {
                PassTurn();
            }
        }
    }

    public void Moving()
    {
        GetComponent<InputManager>().inputAllowed = false;

        if (!moving)
        {
            ActivateCurrent();
        }
    }

    public void Moved(GameObject go)
    {
        GetComponent<InputManager>().inputAllowed = true;
        if (moving)
        {
            moving = false;
            currentTappo.tag = "Untagged";
            currentTappo.GetComponent<Player>().ChooseWeapon();
        }
        else
        {
            currentTappo.layer = 0;
            PassTurn();
            Destroy(go);
        }
    }


    public void SetCamera(GameObject tappo)
    {
        tappoCamera.GetComponent<FollowTappo>().SetTappoToFollow(tappo);
    }

    public void ActivateCurrent()
    {
        currentTappo.SetActive(true);
    }

    public bool IsCurrentTappo(GameObject tappo)
    {
        return currentTappo == tappo;
    }

    public void Death(GameObject diedObject)
    {
        int team = diedObject.GetComponent<Player>().team;
        spawnedTappi.Remove(diedObject);
        bool companionFound = false;
        bool Win = true;
        int oldTeam = spawnedTappi[0].GetComponent<Player>().team;
        foreach (GameObject tappo in spawnedTappi)
        {
            if(tappo.GetComponent<Player>().team== team)
            {
                companionFound = true;
            }
            if(oldTeam!= tappo.GetComponent<Player>().team)
            {
                Win = false;
            }
        }
        if (companionFound)
        {
            GameObject.FindGameObjectWithTag("EventText").GetComponent<FadingText>().ChangeText("Il giocatore " + (team + 1) + " ha perso un tappo!");
        }
        else
        {
            if (!Win)
            {
                GameObject.FindGameObjectWithTag("EventText").GetComponent<FadingText>().ChangeText("Il giocatore " + (team + 1) + "è stato eliminato!");
            }
            else
            {

                GameObject.FindGameObjectWithTag("EventText").GetComponent<FadingText>().ChangeText("Il giocatore " + (oldTeam + 1) + "ha vinto!");
                Pause();
                GameObject.Find("Riprendi").SetActive(false);
            }
        }
        GameObject.FindGameObjectWithTag("NumberTappiText").GetComponent<Text>().text = "Tappi rimanenti: " + spawnedTappi.Count;
    }

    public void Pause()
    {
        GetComponent<InputManager>().inputAllowed = false;
        pauseMenu.SetActive(true);
    }

    public void UnPause()
    {
        GetComponent<InputManager>().inputAllowed = true;
        pauseMenu.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Additive);
    }
}
