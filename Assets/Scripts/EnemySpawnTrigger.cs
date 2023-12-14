using static PieConstants;
using System.Collections;
using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public ShipAttackType AttackPattern;
    public float SpawnDelay = 0f;// seconds
    public float SpawnEnterTime = 1f;// seconds

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
                                 new Vector3(Screen.width / 2,
                                             Screen.height + 30,
                                             0)
                               );
	    var ship = Instantiate(EnemyPrefab,
                               startPos,
		                       Quaternion.identity);

        var shipScript = ship.GetComponent<EnemyShip>();
        shipScript.SetValues(AttackPattern, 1);
		StartCoroutine(shipScript.EnterPlayArea(SpawnEnterTime, 0.5f));
    }

    private void OnDrawGizmos()
    {
        if (_collider != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(_collider.bounds.center, _collider.bounds.size);
        }
    }
}
