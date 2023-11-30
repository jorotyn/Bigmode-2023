using static PieConstants;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Layers.Ground ||
	        collision.CompareTag(Tags.Player))
        {
            Destroy(gameObject);
        }
    }
}
