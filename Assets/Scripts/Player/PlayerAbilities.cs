using UnityEngine;
using UnityEngine.Events;

public enum AbilityEnum
{
    None,
    WallJump,
    DoubleJump,
    Invincibility
}

public class PlayerAbilities : MonoBehaviour
{
    #region Serialized Fields
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Ability")]
    [SerializeField] AbilityEnum ability = AbilityEnum.None;
    #endregion

    #region Properties
    public AbilityEnum CurrentAbility
    {
        get { return ability; }
        set
        {
            ability = value;
            if (ability == AbilityEnum.Invincibility)
            {
                playerHealth.SetCanTakeDamage(false);
            }
            else
            {
                playerHealth.SetCanTakeDamage(true);
            }
            onAbilityChange.Invoke(ability);
        }
    }
    #endregion

    #region Events
    [Header("Events")]
    public UnityEvent<AbilityEnum> onAbilityChange = new UnityEvent<AbilityEnum>();
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        if (playerHealth == null) playerHealth = GetComponent<PlayerHealth>();
    }
    #endregion
}
