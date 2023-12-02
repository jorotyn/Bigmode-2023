using UnityEngine;

public class PieGameManager : MonoBehaviour
{
    public Camera Camera;

    public GameObject ObservationPlatform;

    private void Update()
    {
        var newObservationPlatformY = Camera.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, 0)).y;
        ObservationPlatform.transform.position = new Vector3(
                ObservationPlatform.transform.position.x,
                newObservationPlatformY,
                0
            );
    }
}
