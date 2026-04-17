using System.Collections;
using UnityEngine;

public class AICarHandler : MonoBehaviour
{

    [SerializeField]
    CarHandler carHandler;

    [SerializeField]
    MeshCollider meshCollider;

    //Collision Dectection
    RaycastHit[] raycastHits = new RaycastHit[1]; 
    bool isCarAhead = false;

    [SerializeField]
    LayerMask otherCarsLayerMask;

    WaitForSeconds wait = new WaitForSeconds(0.2f);
    private void Awake()
    {
        if (CompareTag("Player"))  // if this script is attached to a player gameObject then destroy it.
        {
            Destroy(this);
            return;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(UpdateLessOftenCO());
    }

    // Update is called once per frame
    void Update()
    {
        float accelerationInput = 1.0f;
        float steerInput = 0.0f;

        if (isCarAhead)
        {
            accelerationInput = -1;
        }
        steerInput = Mathf.Clamp(steerInput,-1.0f,1.0f);

        // steerInput x axis movement and accelerationInput is y axis
        carHandler.setInput(new Vector2 (steerInput, accelerationInput));
        
    }

    IEnumerator UpdateLessOftenCO()
    {
        while (true)
        {
            isCarAhead = CheckIfOtherCarIsAhead();
            yield return wait;
        }
    }

    bool CheckIfOtherCarIsAhead()
    {
        meshCollider.enabled = false;

        int numberOfHits = Physics.BoxCastNonAlloc(transform.position, Vector3.one * 0.25f, transform.forward, raycastHits,Quaternion.identity,2,otherCarsLayerMask);   // this will throw a box in front of the car and check if other cars are there.

        meshCollider.enabled = true;

        if(numberOfHits > 0)
        {
            return true;
        }

        return false;
    }

    private void OnEnable()
    {
        carHandler.SetMaxSpeed(Random.Range(2,4));
    }
}
