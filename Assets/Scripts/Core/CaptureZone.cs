using UnityEngine;

public class CaptureZone : MonoBehaviour
{
    public float radius = 5f;
    [Range(0f, 1f)] public float progressA;
    [Range(0f, 1f)] public float progressB;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}