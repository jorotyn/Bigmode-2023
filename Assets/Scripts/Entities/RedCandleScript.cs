using UnityEngine;

public class RedCandleScript : BaseCandleScript
{

     private FMOD.Studio.EventInstance RedCandleInstance;
    public string RedEvent;

     void Start()
    {
        RedCandleInstance = FMODUnity.RuntimeManager.CreateInstance(RedEvent);

    }
    public override void Fire()
    {
        DoCircleAttack();
        RedCandleInstance.start();
    }

    private void DoCircleAttack()
    {
        const int radius = 2;

        var numberOfProjectiles = 8;

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
}

