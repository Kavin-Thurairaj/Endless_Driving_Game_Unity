using UnityEngine;

public class CarPartHandler : MonoBehaviour
{
    AudioSource bounceAs;


    private void Awake()
    {
        bounceAs = GetComponent<AudioSource>();


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!bounceAs.isPlaying)
        {
            bounceAs.pitch = collision.relativeVelocity.magnitude * 0.5f;

            bounceAs.pitch = Mathf.Clamp(bounceAs.pitch, 0.5f, 1f);
            bounceAs.Play();
        }
    }
}
