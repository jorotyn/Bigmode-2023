using UnityEngine;

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
    [SerializeField] AbilityEnum ability = AbilityEnum.None;
    [SerializeField] private PlayerHealth playerHealth;
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
                // Set _canTakeDamage to false when the ability is Invincibility
                playerHealth.SetCanTakeDamage(false);
            }
            else
            {
                // Set _canTakeDamage to true for other abilities
                playerHealth.SetCanTakeDamage(true);
            }
        }
    }
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        if (playerHealth == null) playerHealth = GetComponent<PlayerHealth>();
    }
    #endregion
}
