using UnityEngine;

public class TrackPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    GameObject playerCar;

    [SerializeField]
    Rigidbody policeRb;

    Transform playerTransform;
    void Start()
    {
        playerCar = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerCar.transform;
    }

    // Update is called once per frame
    void Update()
    {
        policeRb.MovePosition(Vector3.Lerp(policeRb.transform.position,playerTransform.position, Time.deltaTime * 0.5f));
    }
}
