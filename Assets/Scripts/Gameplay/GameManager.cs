using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    PlayerController player;

    #region Unity_functions
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region Scene_transitions
    public void Level_0()
    {
        SceneManager.LoadScene("Level_0");
    }

    public void Level_1()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void MissionComplete()
    {
        SceneManager.LoadScene("MissionComplete");
    }

    public void Office()
    {
        //SceneManager.LoadScene("Office");
        StartCoroutine(SwitchScene());
    }

    IEnumerator SwitchScene()
    {
        Debug.Log("going into coroutine");
        yield return SceneManager.LoadSceneAsync("Office");
        var destPortal = FindObjectOfType<Portal>();
        player.transform.position = destPortal.SpawnPoint.position;
    }
    #endregion
}
