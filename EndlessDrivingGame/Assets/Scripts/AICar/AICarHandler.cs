using System.Collections;
using UnityEngine;

public class AICarHandler : MonoBehaviour
{

    [SerializeField]
    CarHandler carHandler;

    [SerializeField]
    MeshCollider meshCollider;


    // SFX
    [SerializeField]
    AudioSource honkAS;

    //Collision Dectection
    RaycastHit[] raycastHits = new RaycastHit[1]; 
    bool isCarAhead = false;

    float carAheadDistance =0;

    [SerializeField]
    LayerMask otherCarsLayerMask;

    WaitForSeconds wait = new WaitForSeconds(0.2f);

    //Lanes
    int drivingInLane = 0;

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
            if (carAheadDistance<10 && !honkAS.isPlaying)
            {
                honkAS.pitch = Random.Range(0.5f, 1.1f);
                honkAS.Play();
            }
        }

        float desiredPositionX = Utils.CarLanes[drivingInLane];

        float difference = desiredPositionX - transform.position.x;

        if (Mathf.Abs(difference) > 0.05f)
        {
            steerInput = 1.0f * difference;
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
            carAheadDistance = (transform.position - raycastHits[0].point).magnitude;  // if there is a gameobject in front of the ai car then the ditance will be calculated.
            return true;
        }

        return false;
    }

    private void OnEnable()
    {
        //Set random lane
        drivingInLane = Random.Range(0,Utils.CarLanes.Length);

        // Set random speed
        carHandler.SetMaxSpeed(Random.Range(2,4));
    }
}
