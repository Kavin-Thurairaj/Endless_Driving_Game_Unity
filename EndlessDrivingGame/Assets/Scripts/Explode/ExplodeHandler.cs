using UnityEngine;

public class ExplodeHandler : MonoBehaviour
{

    [SerializeField]
    GameObject originalObject;

    [SerializeField]
    GameObject model;


    Rigidbody[] rigidbodies;

    private void Awake()
    {
        rigidbodies = model.GetComponentsInChildren<Rigidbody>(true);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Explode(Vector3 externalForce)
    {
        originalObject.SetActive(false);  //the original car body will be set to false when the car collide with something.

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.transform.parent = null;

            rb.GetComponent<MeshCollider>().enabled = true;  // here we get the mesh collider of the car pieces and set to true so it can collide.

            rb.gameObject.SetActive(true);  // here the car pieces will be set to true so it will be visible when collided.

            rb.isKinematic = false;

            rb.AddForce(Vector3.up*200 + externalForce,ForceMode.Force);
        }
    }
    
}
