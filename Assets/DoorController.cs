using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (
            other.CompareTag("Player") && 
            (GameManager.Instance.level == 3 && GameManager.Instance.firstBossDead) ||
            (GameManager.Instance.level == 4 && GameManager.Instance.enemiesKilled == (GameManager.Instance.maxEnemiesPerLvl*2)) ||
            (GameManager.Instance.level == 5 && GameManager.Instance.secondBossDead)
            )
        {
            GameManager.Instance.LoadNextLevel();
        }
    }
}
