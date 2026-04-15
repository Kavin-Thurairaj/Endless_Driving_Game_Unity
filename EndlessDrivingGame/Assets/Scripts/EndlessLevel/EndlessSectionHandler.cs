using UnityEngine;

public class EndlessSectionHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    Transform playerCarTransform;
    void Start()
    {
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;  // here we get the Car object and its transform in the world.
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = transform.position.z - playerCarTransform.position.z;

        float lerpPercentage = 1.0f - ((distanceToPlayer - 100) / 150.0f);

        lerpPercentage = Mathf.Clamp01(lerpPercentage);  //here we clamp the lerpPercentage between 0 to 1.

        transform.position = Vector3.Lerp(new Vector3(transform.position.x,-20,transform.position.z), new Vector3(transform.position.x, 0 , transform.position.z),lerpPercentage);
    }
}
