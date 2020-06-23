using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private int max = 0;

    [SerializeField]
    private float timeBtwSpawns = 4.5f;

    [SerializeField]
    private GameObject enemyFirstType;

    [SerializeField]
    private GameObject enemySecondType;

    [SerializeField]
    private GameObject firstBoss;

    [SerializeField]
    private GameObject secondBoss;

    private bool isFirst = true;

    // Start is called before the first frame update
    private void Start()
    {
        max = GameManager.Instance.maxEnemiesPerLvl;
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        for(int i = 0; i < max; i++)
        {
            yield return new WaitForSeconds(timeBtwSpawns);

            Instantiate(isFirst ? enemyFirstType : enemySecondType, transform.position, transform.rotation);

            isFirst = isFirst ? false : true;

            if(GameManager.Instance.level == 3 && !GameManager.Instance.firstBossSpawned && i == max-1) {

                Instantiate(firstBoss, transform.position, transform.rotation);

                GameManager.Instance.firstBossSpawned = true;

                GameManager.Instance.allowMoving = false;
            }

           if(GameManager.Instance.level == 5 && !GameManager.Instance.secondBossSpawned && i == max-1) {
                Instantiate(secondBoss, transform.position, transform.rotation);

                GameManager.Instance.secondBossSpawned = true;

                GameManager.Instance.allowMoving = false;
            }
        }
    }
}
