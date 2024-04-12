using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaActivator : MonoBehaviour
{
    //Variável para impedir de ativar mais de uma vez
    private bool hasActivated;
    //Variáveis para pegar e ativar os portões
    private int childNumber;
    //Variáveis para setar spawn
    public float maxXSpawnRange, maxZSpawnRange;
    //Variáveis para setar as waves
    public int numberOfWaves, enemyNumber;
    public GameObject enemy1;
    public GameObject enemyPool;
    private int currentWave = 0;


    // Start is called before the first frame update
    void Start()
    {
        childNumber = this.gameObject.transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        if(hasActivated && enemyPool.transform.childCount == 0)
        {
            nextWave();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasActivated)
        {
            gateAwake();
            nextWave();
        }
    }

    private void gateAwake()
    {
        hasActivated = true;
        for(int childCount = 1; childCount < childNumber; childCount++)
        {
            this.gameObject.transform.GetChild(childCount).gameObject.SetActive(true);
        }
    }
    private void nextWave()
    {
        if (currentWave < numberOfWaves)
        {
            for (int i = 0; i < enemyNumber; i++)
            {
                float spawnRangeX = Random.Range(-maxXSpawnRange, maxXSpawnRange);
                float spawnRangeZ = Random.Range(-maxZSpawnRange, maxZSpawnRange);
                GameObject instace = Instantiate(enemy1, this.transform.position + new Vector3(spawnRangeX, 1, spawnRangeZ), Quaternion.identity);
                instace.transform.parent = enemyPool.transform;
            }
            currentWave++;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
