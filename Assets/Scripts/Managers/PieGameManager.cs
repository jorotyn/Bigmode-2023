using UnityEngine;

public class PieGameManager : MonoBehaviour
{
    public GameObject ScoopPrefab;
    public int ScoopCount = 3;

    private void Start()
    {
        for(int i = 0; i < ScoopCount; i++)
        {
			Instantiate(ScoopPrefab, new Vector3(Random.Range(-4, 4), 10, 0), Quaternion.identity);
    	}
    }
}
