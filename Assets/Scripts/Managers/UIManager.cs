using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Serialized Fields
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerAbilities playerAbilities;

    [Header("Death")]
    [SerializeField] private GameObject deathPanel;

    [Header("Mode")]
    [SerializeField] private TextMeshProUGUI currentModeText;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        if (playerHealth == null) playerHealth = FindObjectOfType<PlayerHealth>();

        playerHealth.onPlayerDeath.AddListener(OnPlayerDeath);

        if (deathPanel != null) deathPanel.gameObject.SetActive(false);

        if (playerAbilities == null) playerAbilities = FindObjectOfType<PlayerAbilities>();
        if (playerAbilities != null)
        {
            playerAbilities.onAbilityChange.AddListener(OnAbilityChange);
            currentModeText.text = "MODE: " + playerAbilities.CurrentAbility.ToString();
        }
    }
    #endregion

    #region Event Driven Methods
    private void OnPlayerDeath()
    {
        if (deathPanel != null) deathPanel.gameObject.SetActive(true);
    }

    private void OnAbilityChange(AbilityEnum newAbility)
    {
        if (newAbility == AbilityEnum.None)
        {
            currentModeText.text = "MODE: Pizza";
        }
        else
        {
            currentModeText.text = "MODE: " + newAbility.ToString();
        }
    }
    #endregion
}
