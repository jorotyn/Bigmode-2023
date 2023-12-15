using static PieConstants;
using System.Collections;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    private bool _firing;

    private ShipAttackType _attackType;
    private int _intensity = 1;

    private FMOD.Studio.EventInstance IdleInstance;
    public string IdleEvent;

    private FMOD.Studio.EventInstance FireInstance;
    public string FireEvent;


    void Start ()
    {
        IdleInstance = FMODUnity.RuntimeManager.CreateInstance(IdleEvent);

        FireInstance = FMODUnity.RuntimeManager.CreateInstance(FireEvent);

        IdleInstance.start();


    }

    public void SetValues(ShipAttackType attackType,
                          int intensity)
    {
        _attackType = attackType;
        _intensity = intensity;
    }

    private float ProjectileVelocity => _intensity switch
    {
        _ => 1f
    };

    private float FireFrequency => _intensity switch
    {
        _ => 1f
    };

    private IEnumerator BeginFiring()
    {

        FireInstance.start();
        _firing = true;
        while (_firing)
        {
            switch (_attackType)
            {
                case ShipAttackType.Circle:
                    DoCircleAttack();
                    break;
            }


            yield return new WaitForSeconds(FireFrequency);
        }
    }

    public void StopFiring() => _firing = false;

    public IEnumerator EnterPlayArea(float enterTime, float fireDelayTime)
    {
        var startPos = transform.position;
        var finalPos = transform.position + ((Vector3)Vector2.down * 2);
        finalPos.z = 0;

        float elapsedTime = 0;

        while (elapsedTime < enterTime)
        {
            transform.position = Vector3.Lerp(startPos, finalPos, elapsedTime / enterTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(fireDelayTime);
        StartCoroutine(BeginFiring());
    }

    public void ExitPlayArea()
    {
    }

    #region Circle attack
    private int NumberOfBulletsForCircleAttack => _intensity switch
    {
        (<=3) and (>0) => 4, 
        (<=5) and (>3) => 6,
        (<=7) and (>5) => 8,
        <7 => 12,
        _ => 4
    };

    private void DoCircleAttack()
    {
        const int radius = 2;

        var numberOfProjectiles = NumberOfBulletsForCircleAttack;

        var startPoint = (Vector2)transform.position;
        float angleStep = 360f / numberOfProjectiles;
        float angle = 0f;

        for (int i = 0; i <= numberOfProjectiles - 1; i++)
        {
            var projectileDirXPosition = startPoint.x + Mathf.Cos((angle * Mathf.PI) / 180) * radius;
            var projectileDirYPosition = startPoint.y + Mathf.Sin((angle * Mathf.PI) / 180) * radius;

            var projectileVector = new Vector2(projectileDirXPosition, projectileDirYPosition);
            var projectileMoveDirection = (projectileVector - startPoint).normalized * ProjectileVelocity;

            var tmpObj = Instantiate(ProjectilePrefab, startPoint, Quaternion.identity);
            tmpObj.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);

            angle += angleStep;
        }
    }
    #endregion


    
    void OnDestroy()
    {
        FireInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        
        IdleInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        
    }
}
