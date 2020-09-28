using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float respawnTime = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(enemyWave());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemies()
    {
        GameObject a = Instantiate(enemyPrefab) as GameObject;
        a.transform.position = new Vector2(15, Random.Range(-2.5f, 2.5f));
    }

    IEnumerator enemyWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnTime);
            SpawnEnemies();
        }
    }
}
