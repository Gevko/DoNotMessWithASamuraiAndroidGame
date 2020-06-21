using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; } = null;

    private int enemiesKilled = 0;

    private int level = 1;

    public bool allowMoving = true;

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
            asyncLoad = SceneManager.LoadSceneAsync("InsideScene");
        }
        else
        {
            //print(levelToLoad);
            //print(Application.CanStreamedLevelBeLoaded("Level" + levelToLoad));
            if (Application.CanStreamedLevelBeLoaded("Level" + levelToLoad))
            {
                HUD.SetActive(true);
                asyncLoad = SceneManager.LoadSceneAsync("Level" + levelToLoad);
            }
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

    public void HandleNextMessage() {
        
    }
}
