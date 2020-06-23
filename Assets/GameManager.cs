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

    public int maxEnemiesPerLvl = 15;

    public bool IsPaused = false;

    // { get; private set; }

    private float oldTimeScale;

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

    public void showGameOver(bool val)
    {
        IsPaused = val;
        UIManager.Instance.ShowGameOver(val);
    }

    public void ResetGame()
    {
        IsPaused = false;
        level = 1;
        Time.timeScale = oldTimeScale;
        ResetScore();
        LoadNextLevel();
        UIManager.Instance.ShowPausePanel(false);
        UIManager.Instance.resetMessages();
        showGameOver(false);
    }

    public void LoadMainMenu()
    {
        IsPaused = false;
        level = 0;
        Time.timeScale = oldTimeScale;
        ResetScore();
        LoadNextLevel();
        UIManager.Instance.ShowPausePanel(false);
        UIManager.Instance.resetMessages();
        showGameOver(false);
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
        print("levelToLoad");
        print(levelToLoad);

        //EnemyManager.Instance.ResetEnemyCounter();
        AsyncOperation asyncLoad = null;
        if (levelToLoad == 0)
        {
            ResetScore();
            asyncLoad = SceneManager.LoadSceneAsync("Main Menu");
        }
        if (levelToLoad == 1)
        {
            HUD.SetActive(true);
            asyncLoad = SceneManager.LoadSceneAsync("StartScene");
        }
        else if(levelToLoad == 2)
        {
            HUD.SetActive(true);
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
            HUD.SetActive(true);
            asyncLoad = SceneManager.LoadSceneAsync("InsideScene_2");
        }
        else if (levelToLoad == 4)
        {
            HUD.SetActive(true);
            asyncLoad = SceneManager.LoadSceneAsync("InsideScene_3");

        } else if (levelToLoad == 5)
        {
            HUD.SetActive(true);
            asyncLoad = SceneManager.LoadSceneAsync("FinalScene");
        }

        while (asyncLoad == null || !asyncLoad.isDone)
        {
            print(asyncLoad.progress);
            yield return null;
        }
        yield return null;
    }

    public void EnemyKilled ()
    {
        enemiesKilled++;
        print("enemiesKilled: " + enemiesKilled.ToString());
        UpdateEnemiesKilled();
    }

    public void PauseGame(bool pause)
    {
        IsPaused = pause;
        if (pause)
        {
            oldTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            UIManager.Instance.ShowPausePanel(true);

        }
        else
        {
            Time.timeScale = oldTimeScale;
            UIManager.Instance.ShowPausePanel(false);
        }
    }

    private void ResetScore()
    {
        playerHP = 100;
        playerAP = 100;
        firstBossSpawned = false;
        secondBossSpawned = false;
        firstBossDead = false;
        secondBossDead = false;
        enemiesKilled = 0;
        IsPaused = false;
        UpdateEnemiesKilled();
    }

}
