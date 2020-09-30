using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    enum EnemyType
    {
        Normal,
        Fast,
        Bounce
    };

    public GameObject enemyPrefab;
    public GameObject fastEnemyPrefab;
    public GameObject bounceEnemyPrefab;
    public float respawnTime = 2.0f;
    public float fastRespawnTime = 5f;
    public float bounceRespawnTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(enemyWave());
        StartCoroutine(fastEnemyWave());
        StartCoroutine(bounceEnemyWave());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemies(EnemyType type)
    {
        GameObject a;
        switch (type)
        {
            case EnemyType.Normal:
                a = Instantiate(enemyPrefab) as GameObject;
                a.transform.position = new Vector2(15, Random.Range(-2.5f, 2.5f));
                break;
            case EnemyType.Fast:
                a = Instantiate(fastEnemyPrefab) as GameObject;
                a.transform.position = new Vector2(15, Random.Range(-3f, 3f));
                break;
            case EnemyType.Bounce:
                a = Instantiate(bounceEnemyPrefab) as GameObject;
                a.transform.position = new Vector2(15, Random.Range(-2.5f, 2.5f));
                a.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 6f * ((Random.Range(0, 2) - 0.5f) * 2f), // -1 or 1
                    ForceMode2D.Impulse);
                break;
        }
        
    }

    IEnumerator enemyWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnTime);
            SpawnEnemies(EnemyType.Normal);
        }
    }

    IEnumerator fastEnemyWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(fastRespawnTime);
            SpawnEnemies(EnemyType.Fast);
        }
    }

    IEnumerator bounceEnemyWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(bounceRespawnTime);
            SpawnEnemies(EnemyType.Bounce);
        }
    }
}
