using static PieConstants;
using System.Collections;
using UnityEngine;

public class HelperSpawnTrigger : MonoBehaviour
{
    public GameObject HelperPrefab;
    public float SpawnDelay = 0f;// seconds

    private bool _hasSpawned;
    private Camera _camera => Camera.main;
    private BoxCollider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.CompareTag(Tags.Player) && !_hasSpawned)
        {
            _hasSpawned = true;
            StartCoroutine(Spawn());
	    }
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(SpawnDelay);

        var startPos = _camera.ScreenToWorldPoint(
                                 new Vector3(Screen.width + 20,
                                             Screen.height - 20,
                                             0)
                               );
	    var helper = Instantiate(HelperPrefab,
                                 startPos,
		                         Quaternion.identity);

        var helperScript = helper.GetComponent<HelperScript>();
		StartCoroutine(helperScript.Enter());
    }

    private void OnDrawGizmos()
    {
        if (_collider != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(_collider.bounds.center, _collider.bounds.size);
        }
    }
}
