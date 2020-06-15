using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; } = null;

    private int level = 1;

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
        else
        {
            print(levelToLoad);
            print(Application.CanStreamedLevelBeLoaded("Level" + levelToLoad));
            if (Application.CanStreamedLevelBeLoaded("Level" + levelToLoad))
            {
                //HUD.SetActive(true);
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
}
