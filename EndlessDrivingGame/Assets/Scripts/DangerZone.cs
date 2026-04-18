using UnityEngine;

public class DangerZone : MonoBehaviour
{

    [SerializeField]
    ExplodeHandler explodeHandler;
    private void OnCollisionEnter(Collision collision)
    {

        Vector3 velocity = collision.rigidbody.linearVelocity;
        if (gameObject.CompareTag("Player"))
        {
            explodeHandler.Explode(velocity * 45);
        }
    }
}
