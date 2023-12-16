using UnityEngine;
using FMODUnity;
using static PieConstants;

public class ScoopStack : MonoBehaviour
{
    private PlayerAbilities playerAbilities;

    private Scoop _currentScopp;

    [SerializeField]
    private EventReference ScoopSfx;
    public float scoopcount;

    private void Start()
    {
        playerAbilities = GetComponent<PlayerAbilities>();
        scoopcount = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Scoop))
        {
            GetAbility(collision.gameObject.GetComponent<Ability>());
            AddScoop(collision.gameObject.GetComponent<Scoop>());
            PlayScoopSound(ScoopSfx);
            scoopcount ++ ;
        }
    }

    private void GetAbility(Ability ability)
    {
        if (ability == null) return;
        playerAbilities.CurrentAbility = ability.abilityType;
    }

    private void AddScoop(Scoop scoop)
    {
        if (_currentScopp != null)
        {
            Destroy(_currentScopp.gameObject);
        }
        scoop.AttachToPlayer(transform);
        _currentScopp = scoop;
    }

    private void PlayScoopSound(EventReference sound)
    {
        FMOD.Studio.EventInstance Scoopinstance = RuntimeManager.CreateInstance(sound);
        Scoopinstance.start();
        Scoopinstance.release();
    }
}
