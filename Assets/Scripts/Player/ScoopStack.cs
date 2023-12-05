using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using static PieConstants;

public class ScoopStack : MonoBehaviour
{
    public int MaxScoops = 1;

    private PlayerAbilities playerAbilities;

    private readonly List<Scoop> _scoops = new();

    private bool _addingScoop;

    [SerializeField]
    private FMODUnity.EventReference ScoopSfx;

    private void Start()
    {
        playerAbilities = GetComponent<PlayerAbilities>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Scoop) && !_addingScoop)
        {
            GetAbility(collision.gameObject.GetComponent<Ability>());
            AddScoopToQueue(collision.gameObject.GetComponent<Scoop>());
            PlayScoopSound(ScoopSfx);
        }
    }

    private void GetAbility(Ability ability)
    {
        if (ability == null) return;
        playerAbilities.CurrentAbility = ability.abilityType;
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

    private void PlayScoopSound(FMODUnity.EventReference sound)
    {
        
            FMOD.Studio.EventInstance Scoopinstance = RuntimeManager.CreateInstance(sound);
            Scoopinstance.start();
            Scoopinstance.release();

        

    }
}
