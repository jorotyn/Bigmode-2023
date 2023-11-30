using System.Collections;
using UnityEngine;

public class PieGameManager : MonoBehaviour
{
    public GameObject ScoopPrefab;
    public GameObject SpikePrefab;

    public int ScoopCount = 0;

    private void Start()
    {
        for(int i = 0; i < ScoopCount; i++)
        {
			Instantiate(ScoopPrefab, new Vector3(Random.Range(-4, 4), 10, 0), Quaternion.identity);
    	}
        StartCoroutine(SpawnSpikes());
    }

    private IEnumerator SpawnSpikes()
    { 
        while(true)//for now
        {
            yield return new WaitForSeconds(3f);
			Instantiate(SpikePrefab, new Vector3(Random.Range(-4, 4), 10, 0), Quaternion.identity);
	    }
    }
}
