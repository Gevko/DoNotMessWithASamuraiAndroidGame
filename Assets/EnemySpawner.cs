using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private int max = 15;

    [SerializeField]
    private float timeBtwSpawns = 3.5f;

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
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        for(int i = 0; i < max; i++)
        {
            yield return new WaitForSeconds(timeBtwSpawns);

            Instantiate(firstBoss, transform.position, transform.rotation);

            //Instantiate(isFirst ? enemyFirstType : enemySecondType, transform.position, transform.rotation);

            //isFirst = isFirst ? false : true;

            /*if(GameManager.Instance.level == 3 && !GameManager.Instance.firstBossSpawned && i == 2) {
                print("1º boss spawn");
                Instantiate(firstBoss, transform.position, transform.rotation);
                GameManager.Instance.firstBossSpawned = true;
            }

           if(GameManager.Instance.level == 5 && !GameManager.Instance.secondBossSpawned && i == 12) {
                Instantiate(secondBoss, transform.position, transform.rotation);
                GameManager.Instance.firstBossSpawned = false;
            }*/
        }
    }
}
