using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTest : MonoBehaviour
{
   
   public GameObject prefab;


     private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SpawnMore();
        }

       
    }

    private void SpawnMore()
    {

        Instantiate(prefab, new Vector3(-1, gameObject.transform.position.y +19, 0), Quaternion.identity);

    }
}
