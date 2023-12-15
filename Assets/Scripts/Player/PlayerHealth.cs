using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public int currentLife = 6;
    #region Serialized Fields
    [Header("References")]
    [SerializeField] private PieAudioManager audioPieManager;
    [SerializeField] private UIManager _uimanager;
    #endregion

    #region Private Fields
    private bool _canTakeDamage = true;
    private SpriteRenderer _spriteRenderer;
    #endregion

    #region Events
    [Header("Events")]
    public UnityEvent onPlayerDeath;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    #endregion

    #region Private Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spike"))
        {
            HandleSpikeDamage();
        }

        if (collision.CompareTag("Death"))
        {
            Die();
        }
    }

    private void HandleSpikeDamage()
    {
        if (!_canTakeDamage) return;
        StartCoroutine(DisableDamage());
        currentLife -= 1;
        _uimanager.UpdateHearth(currentLife);
        audioPieManager.Hurt();
    }
    #endregion

    #region Coroutines
    private IEnumerator DisableDamage(float seconds = 1.5f)
    {
        _canTakeDamage = false;
        StartCoroutine(ShowInvincibility());
        yield return new WaitForSeconds(seconds);
        _canTakeDamage = true;
    }

    private IEnumerator ShowInvincibility()
    {
        while (!_canTakeDamage)
        {
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
        }
        _spriteRenderer.enabled = true;
    }
    #endregion

    #region Public Methods
    public void Die()
    {
        onPlayerDeath.Invoke();
        gameObject.SetActive(false);
        audioPieManager.DieWarp();
    }

    public void SetCanTakeDamage(bool canTakeDamage)
    {
        _canTakeDamage = canTakeDamage;
    }
    #endregion
}
