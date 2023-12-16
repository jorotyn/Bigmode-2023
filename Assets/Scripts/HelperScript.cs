using System.Collections;
using UnityEngine;

public class HelperScript : MonoBehaviour
{
    public GameObject[] ScoopPrefabs;
    public float FireFrequency = 1f;

    private FMOD.Studio.EventInstance ScoopThrowInstance;
    public string ScoopThrowEvent = "event:/ScoopThrow";

    // cumulative amount of deltaTime it should take for
    // object to go from one side of the screen to the other
    private float time => Screen.width / 4;

    private float _cameraTopWorldY => Camera.main.ScreenToWorldPoint(
                                                    new Vector3(0,
                                                                Camera.main.pixelHeight,
                                                                0)).y;
    private bool _dead;

    void Start()
    {
        ScoopThrowInstance = FMODUnity.RuntimeManager.CreateInstance(ScoopThrowEvent);

    }

    public IEnumerator Enter()
    {
        StartCoroutine(BeginFiring());
        var startPos = transform.position;
        var finalPos = new Vector3(transform.position.x - Camera.main.pixelWidth, 0, 0);

        float elapsedTime = 0;

        while (elapsedTime < time && !_dead)
        {
            transform.position = Vector3.Lerp(startPos,
                                              finalPos,
                                              elapsedTime / time);

            var pos = transform.position;
            pos.y = _cameraTopWorldY;
            pos.z = 0;

            transform.position = pos;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator BeginFiring()
    { 
        while (!_dead)
        {
            var prefab = ScoopPrefabs[Random.Range(0, ScoopPrefabs.Length)];
            var tmpObj = Instantiate(prefab, transform.position, Quaternion.identity);
            tmpObj.GetComponent<Rigidbody2D>().velocity = new Vector2(0, Vector2.down.y);
            ScoopThrowInstance.start();
            yield return new WaitForSeconds(FireFrequency);
        }
    }

    // this might get destroyed while it's moving by a DeathPlane
    // (in fact, at the moment it always gets destroyed this way)
    // we'll use this flag to bail out of the coroutines to avoid an NRE
    private void OnDestroy() => _dead = true;
}
