using System.Collections.Generic;
using UnityEngine;

public class ScoopStack : MonoBehaviour
{
    public int MaxScoops = 3;

    private readonly List<Scoop> _scoops = new();

    private bool _addingScoop;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Scoop") && !_addingScoop)
        {
            AddScoopToQueue(collision.gameObject.GetComponent<Scoop>());
        }
    }

    private void AddScoopToQueue(Scoop scoop)
    {
        _addingScoop = true;
        if (_scoops.Count == MaxScoops)
        {
            var s = _scoops[_scoops.Count - 1];
            _scoops.Remove(s);
            Destroy(s);
        }

        _scoops.Add(scoop);
        scoop.AttachToPlayer(transform, (Scoop.Position)_scoops.Count);
        _addingScoop = false;
    }
}
