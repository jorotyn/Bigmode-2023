using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Serialized Fields
    [Header("References")]
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private PlayerHealth playerHealth;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        if (playerHealth == null) playerHealth = FindObjectOfType<PlayerHealth>();

        // Subscribe to the player's death event
        playerHealth.onPlayerDeath.AddListener(OnPlayerDeath);

        // Disable the deathPanel UI element at the start of the game
        if (deathPanel != null) deathPanel.gameObject.SetActive(false);
    }
    #endregion

    #region Event Driven Methods
    private void OnPlayerDeath()
    {
        // Enable the deathPanel UI element when the player dies
        if (deathPanel != null) deathPanel.gameObject.SetActive(true);
    }
    #endregion
}
