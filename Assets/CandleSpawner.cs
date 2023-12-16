using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleSpawner : MonoBehaviour
{
    public GameObject[] candles;
    private int rand;

    private int randomize;
    void Start()
    {
        rand = Random.Range(0,3);
        randomize = Random.Range(0,6);

        if (randomize <= 1)
        {
            SpawnCandle();
        }





        
    }

    private void SpawnCandle()
    {
          Instantiate(candles[rand],gameObject.transform.position, Quaternion.identity);

    }

    
}
