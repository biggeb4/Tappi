using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    public void Load1v1Level()
    {
        SceneManager.LoadScene("TestLevel");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
