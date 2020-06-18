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

    private bool isFirst = true;

    // Start is called before the first frame update
    private void Start()
    {
#warning Descomentar isto quando implementado
        //StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        for(int i = 0; i < max; i++)
        {
            yield return new WaitForSeconds(timeBtwSpawns);

            Instantiate(isFirst ? enemyFirstType : enemySecondType, transform.position, transform.rotation);

            isFirst = isFirst ? false : true;
        }
    }
}
