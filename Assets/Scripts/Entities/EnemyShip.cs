using System.Collections;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    public GameObject ProjectilePrefab;

    private bool _firing;

    private float _currentX = 0;
    private float _dx = 1f;

    private void Start()
    {
        StartCoroutine(DelayAndBeginFiring());
    }

    public IEnumerator DelayAndBeginFiring()
    {
        yield return new WaitForSeconds(3);
        _firing = true;
        StartCoroutine(BeginFiring());
    }

    public IEnumerator BeginFiring()
    {
        while (_firing)
        {
            var spikeObject = Instantiate(ProjectilePrefab,
                                          transform.position,
                                          Quaternion.identity);

            var x = Mathf.Cos(_currentX);

            spikeObject.GetComponent<Spike>().Fire(new Vector2(x, 0));

            _currentX += _dx;
            if(_currentX > 360)
            {
                _currentX = 0;
	        }

            yield return new WaitForSeconds(0.35f);
        }
    }

    public void StopFiring() => _firing = false;
}
