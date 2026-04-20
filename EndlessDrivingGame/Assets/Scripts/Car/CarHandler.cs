using System.Collections;
using UnityEngine;
using System;

public class CarHandler : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    Transform gameModel;

    [SerializeField]
    ExplodeHandler explodeHandler;

    [SerializeField]
    MeshRenderer carMeshRenderer;

    public MeshRenderer CarMeshRenderer => carMeshRenderer;

    //SFX
    [SerializeField]
    AudioSource carEngineAS;

    [SerializeField]
    AnimationCurve carPitchAnimationCurve;


    [SerializeField]
    AudioSource CarBrakeAs;

    [SerializeField]
    AudioSource carCrashAS;

    bool isExploded = false;

    float accelerationMultiplier = 5;  // Accelaration speed
    float brakeMultiplier = 3;  // Brake Speed
    float steerMultiplier = 5;   // Streeing Speed

    //Input
    Vector2 input = Vector2.zero;

    // Velocity
    private float maxSteerVelocity = 2f;
    private float maxForwardVelocity = 10;
    float carMaxSpeedPercentage;

    bool isPlayer = true;

    //Stats
    float carStartPositionZ;
    float distanceTravelled = 0;
    public float DistanceTravel =>distanceTravelled; // what ever the value in distanceTravelled will be set to the DistanceTravel.


    //Events
    public event Action <CarHandler> OnPlayerCrashed;  // this is the event action that will throw the car handler refer at run time.
    

    void Start()
    {
        
        isPlayer = CompareTag("Player");
        if(isPlayer)
        {
            carEngineAS.Play();
            
        }
        carStartPositionZ = transform.position.z;

    }

    // Update is called once per frame
    void Update()
    {
        if (isExploded)
        {
            FadeOutCarAudio();
            return;
        }
        gameModel.transform.rotation = Quaternion.Euler(0, rb.linearVelocity.x * 5, 0);

        UpdatedCarAudio();

        // Update the Car distance travelled
        distanceTravelled = transform.position.z - carStartPositionZ; // here we get the distance travelled difference.
    }

    private void FixedUpdate()  // This function will fire at a fixed time
    {

        if (isExploded)
        {
            rb.linearDamping = rb.angularVelocity.z * 0.1f;
            rb.linearDamping = Mathf.Clamp(rb.linearDamping, 1.5f, 10);
            //Debug.Log("Linear: " + rb.linearDamping);


            rb.MovePosition(Vector3.Lerp(transform.position, new Vector3(0,0,transform.position.z),Time.deltaTime * 0.5f));
            return;
        }

        if (input.y > 0)  // if the vertical y axis increase then the car will accelerate.
        {
            Accerlerate();
        }
        else
        {
            rb.linearDamping = 0.2f;
        }

        if (input.y < 0)  // here it will execute if the y input is negative 
        {
            Brake();
        }

        Steer();
        
    }

    void Accerlerate()
    {
        rb.linearDamping = 0;

        if (rb.linearVelocity.z>=maxForwardVelocity)
        {
            return;
        }

        rb.AddForce(rb.transform.forward * accelerationMultiplier * input.y);  // the car will move forward at speed of 5
    }

    void Brake()
    {
        if (rb.linearVelocity.z < 0)  // here we check if the car is moving forward and then only apply brake and stop the car  
        {                             // rb.linearVelocity.z is positive then the car is moving forward and bout to stop
            return;   // this will not allow the car to move backward.
        }  
        rb.AddForce(rb.transform.forward * brakeMultiplier * input.y);  // This will slow the car the input.y will be negative value.
    }

    void Steer()
    {
        if (Mathf.Abs(input.x)>0)  // so here we convert the input.x to absolute value and ignore the positive and negative sign.
        {
            float speedBaseSteerLimit = rb.linearVelocity.z / 5.0f;  // if the car is stationary not moving then the steer will not work. 
                                                                     // The the rb.linearVelocity.z is 0 then the force added will be 0.
            speedBaseSteerLimit = Mathf.Clamp01(speedBaseSteerLimit);
            rb.AddForce(rb.transform.right * steerMultiplier * input.x*speedBaseSteerLimit);  // the car will more left and right.
                                                                                              // if the input.x is positive moves right and input.x is negative move left.

            float normalizedX = rb.linearVelocity.x / maxSteerVelocity;
            normalizedX = Mathf.Clamp(normalizedX,-1.0f,1.0f);

            // here we update the rigid body vector to be x,y,z x is the speed to move left and right y is 0 and z is the same.
            rb.linearVelocity = new Vector3(normalizedX * maxSteerVelocity, 0 ,rb.linearVelocity.z);

        }
        else
        {   // Auto center the car if the input.y is press and released lerp(current position , center position , smooth animation to center.)  Reset the position and rotation of the car after the turn.
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, new Vector3(0,0,rb.linearVelocity.z), Time.fixedDeltaTime*3);
        }
    }

    void UpdatedCarAudio()
    {
        if (!isPlayer)  // if it is a ai car then do nothing
        {
            return;
        }

        carMaxSpeedPercentage = rb.linearVelocity.z / maxForwardVelocity;

        carEngineAS.pitch = carPitchAnimationCurve.Evaluate(carMaxSpeedPercentage);

        if(input.y < 0 && carMaxSpeedPercentage > 0.2f)
        {
            //Debug.Log(carMaxSpeedPercentage);

            if(carMaxSpeedPercentage > 0 && carMaxSpeedPercentage < 0.3)  //if the percentage is greater than 0 and less than 0.3
            {
                if (!CarBrakeAs.isPlaying)
                {
                    CarBrakeAs.Play();
                }
            }
           

            CarBrakeAs.volume = Mathf.Lerp(CarBrakeAs.volume, 1.0f, Time.deltaTime * 10);
        }
        else
        {
            CarBrakeAs.volume = Mathf.Lerp(CarBrakeAs.volume,0, Time.deltaTime * 30);
        }
    }

    void FadeOutCarAudio()
    {
        if (!isPlayer)
        {
            return;
        }

        carEngineAS.volume = Mathf.Lerp(carEngineAS.volume,0,Time.deltaTime * 10);
        CarBrakeAs.volume = Mathf.Lerp(CarBrakeAs.volume, 0, Time.deltaTime * 10);
    }


    public void setInput(Vector2 inputVector)  // This is a setter method where the input Vector will be normalize and stored in the input.
    {
        inputVector.Normalize();  // here we normalise the vector value from user input
        input = inputVector;
    }


    public void SetMaxSpeed(float maxSpeed)  // this is the setter method for maxSpeed.
    {
        maxForwardVelocity = maxSpeed;
    }

    IEnumerator SlowDownTimeCO()
    {
        while (Time.timeScale>0.2f)
        {
            Time.timeScale -= Time.deltaTime * 2;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        while (Time.timeScale <= 1.0f)
        {
            Time.timeScale += Time.deltaTime;
            yield return null;
        }

        Time.timeScale = 1.0f;
    }

    private void OnCollisionEnter(Collision collision)  // when the car enters a collision then this method will be fired.
    {
        Debug.Log($"Hit {collision.collider.name}");

        if (!isPlayer)
        {
            if (collision.transform.root.CompareTag("Untagged"))
            {
                return;
            }

            if (collision.transform.root.CompareTag("CarAI"))
            {
                return;
            }
        }
        StartCoroutine(SlowDownTimeCO());

        Vector3 velocity = rb.linearVelocity + rb.angularVelocity;  // here we get the rigidbody of the car
        explodeHandler.Explode(velocity * 45);  // here we call the explode method in the explode handler script to explode the car
        isExploded = true;

        Vector3 impactDirection = -rb.transform.forward;
        rb.AddForce(impactDirection *4f, ForceMode.Impulse);  // this moves the car object backward gives you a backward effect.
        

        carCrashAS.volume = carMaxSpeedPercentage;
        carCrashAS.volume = Mathf.Clamp(carCrashAS.volume, 0.25f,1f);

        carCrashAS.pitch = carMaxSpeedPercentage;
        carCrashAS.pitch = Mathf.Clamp(carCrashAS.pitch,0.3f,1f);

        carCrashAS.Play();

        Debug.Log("crash sound" + carCrashAS);

        //Trigger Event
        OnPlayerCrashed?.Invoke(this);  // here we pass the car handler reference to the UI Handler. Internally it call the method register to it the listener in UI.

        
    }
}