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
    #endregion

    #region Properties
    public AbilityEnum CurrentAbility
    {
        get { return ability; }
        set { ability = value; }
    }
    #endregion
}
