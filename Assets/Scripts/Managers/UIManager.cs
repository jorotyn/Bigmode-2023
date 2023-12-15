using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Serialized Fields
    [Header("Health")]
     
    [SerializeField] private Image Hearth1;
    [SerializeField] private Image Hearth2;
    [SerializeField] private Image Hearth3;

    [SerializeField] private Sprite[] _spritesHearth;


    


    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerAbilities playerAbilities;

    [Header("Death")]
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private GameObject AbbilityPannel;

    [SerializeField] private DeathPannelUi _deathPannelUi;

    private float Timer;
    private float ClimCount;
    private float AlamodeCount;

    [Header("Mode")]
    [SerializeField] private TextMeshProUGUI currentModeText;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        Timer = 0;
        ClimCount = 0;
        AlamodeCount = 0;

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
        if (deathPanel != null) 
        
        {
        _deathPannelUi.TimeAlive = Timer;
        deathPanel.gameObject.SetActive(true);
        AbbilityPannel.gameObject.SetActive(false);

        }

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

    
    void FixedUpdate()
    {
        Timer += Time.deltaTime;

        
    }

    public void UpdateHearth (int number)
    {
        if (number == 6)
        {
            Hearth1.sprite = _spritesHearth[2];
            Hearth2.sprite = _spritesHearth[2];
            Hearth3.sprite = _spritesHearth[2];
        }

        if (number == 5)
        {
            Hearth1.sprite = _spritesHearth[1];
            Hearth2.sprite = _spritesHearth[2];
            Hearth3.sprite = _spritesHearth[2];
        }

        if (number == 4)
        {
            Hearth1.sprite = _spritesHearth[0];
            Hearth2.sprite = _spritesHearth[2];
            Hearth3.sprite = _spritesHearth[2];
        }

        if (number == 3)
        {
            Hearth1.sprite = _spritesHearth[0];
            Hearth2.sprite = _spritesHearth[1];
            Hearth3.sprite = _spritesHearth[2];
        }

        if (number == 2)
        {
            Hearth1.sprite = _spritesHearth[0];
            Hearth2.sprite = _spritesHearth[0];
            Hearth3.sprite = _spritesHearth[2];
        }

        if (number == 1)
        {
            Hearth1.sprite = _spritesHearth[0];
            Hearth2.sprite = _spritesHearth[0];
            Hearth3.sprite = _spritesHearth[1];
        }

        if (number == 0)
        {
            Hearth1.sprite = _spritesHearth[0];
            Hearth2.sprite = _spritesHearth[0];
            Hearth3.sprite = _spritesHearth[0];
        }
    }
}
