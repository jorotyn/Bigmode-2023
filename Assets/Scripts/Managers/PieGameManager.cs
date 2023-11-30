using System.Collections;
using UnityEngine;

public class PieGameManager : MonoBehaviour
{
    public Camera Camera;

    public GameObject ScoopPrefab;
    public GameObject SpikePrefab;

    public GameObject ObservationPlatform;
    public GameObject SpikeSpawnPlane;
    public BoxCollider2D SpikeSpawnPlaneCollider;

    public int ScoopCount = 0;

    private void Start()
    {
        for (int i = 0; i < ScoopCount; i++)
        {
            Instantiate(ScoopPrefab,
                        new Vector3(Random.Range(SpikeSpawnPlaneCollider.bounds.min.x, SpikeSpawnPlaneCollider.bounds.max.x),
                                    SpikeSpawnPlane.transform.position.y,
                                    0),
                        Quaternion.identity);
        }

        SpikeSpawnPlaneCollider = SpikeSpawnPlane.GetComponent<BoxCollider2D>();
        StartCoroutine(SpawnSpikes());
    }

    private void Update()
    {
        var newSpawnY = Camera.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
        SpikeSpawnPlane.transform.position = new Vector3(
                SpikeSpawnPlane.transform.position.x,
                newSpawnY,
                0
            );

        var newObservationPlatformY = Camera.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, 0)).y;
        ObservationPlatform.transform.position = new Vector3(
                ObservationPlatform.transform.position.x,
                newObservationPlatformY,
                0
            );
    }

    private IEnumerator SpawnSpikes()
    {
        while (true)//for now
        {
            yield return new WaitForSeconds(3f);

            Instantiate(SpikePrefab,
                        new Vector3(Random.Range(SpikeSpawnPlaneCollider.bounds.min.x, SpikeSpawnPlaneCollider.bounds.max.x),
                                    SpikeSpawnPlane.transform.position.y,
                                    0),
                        Quaternion.identity);
        }
    }
}
