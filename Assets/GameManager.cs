using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; } = null;

    public int enemiesKilled = 0;

    public int level = 1;

    public bool allowMoving = true;

    public bool firstBossSpawned = false;

    public bool secondBossSpawned = false;

    public bool firstBossDead = false;

    public bool secondBossDead = false;

    public int playerHP = 100;

    public int playerAP = 100;

    public int maxEnemiesPerLvl = 2;

    [SerializeField]
    private GameObject HUD;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UpdateEnemiesKilled();
    }

    private void UpdateEnemiesKilled()
    {
        UIManager.Instance.UpdateEnemyCounter(enemiesKilled);
    }


    public void LoadNextLevel ()
    {
        print("LoadNextLevel");
        StartCoroutine(LoadNextLevelAsync(level));
        level++;
        allowMoving = false;
    }

    private IEnumerator LoadNextLevelAsync(int levelToLoad)
    {
        //HUD.SetActive(false);

        //EnemyManager.Instance.ResetEnemyCounter();
        AsyncOperation asyncLoad;
        if (levelToLoad == 0)
        {
            //ResetScore();
            asyncLoad = SceneManager.LoadSceneAsync("Menu");
        }
        if (levelToLoad == 1)
        {
            asyncLoad = SceneManager.LoadSceneAsync("StartScene");
        }
        else if(levelToLoad == 2)
        {
            asyncLoad = SceneManager.LoadSceneAsync("InsideScene_1");

            //print(levelToLoad);
            //print(Application.CanStreamedLevelBeLoaded("Level" + levelToLoad));
         /*   if (Application.CanStreamedLevelBeLoaded("Level" + levelToLoad))
            {
                HUD.SetActive(true);
                asyncLoad = SceneManager.LoadSceneAsync("Level" + levelToLoad);
            } */
        } else if (levelToLoad == 3)
        {
            asyncLoad = SceneManager.LoadSceneAsync("InsideScene_2");
        }
        else if (levelToLoad == 4)
        {
            asyncLoad = SceneManager.LoadSceneAsync("InsideScene_3");

        } else if (levelToLoad == 5)
        {
            asyncLoad = SceneManager.LoadSceneAsync("FinalScene");
        }

        /*while (!asyncLoad.isDone)
        {
            //print(asyncLoad.progress);
            yield return null;
        }*/
        yield return null;
    }

    public void EnemyKilled ()
    {
        enemiesKilled++;
        print("enemiesKilled: " + enemiesKilled.ToString());
        UpdateEnemiesKilled();
    }

}
