using System.Collections;
using UnityEngine;

public class CarHandler : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    Transform gameModel;

    [SerializeField]
    ExplodeHandler explodeHandler;

    bool isExploded = false;

    float accelerationMultiplier = 5;  // Accelaration speed
    float brakeMultiplier = 3;  // Brake Speed
    float steerMultiplier = 5;   // Streeing Speed

    Vector2 input = Vector2.zero;
    private float maxSteerVelocity = 2f;

    private float maxForwardVelocity = 10;

    bool isPlayer = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1.0f;
        isPlayer = CompareTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (isExploded)
        {
            return;
        }
        gameModel.transform.rotation = Quaternion.Euler(0, rb.linearVelocity.x * 5, 0);
    }

    private void FixedUpdate()  // This function will fire at a fixed time
    {

        if (isExploded)
        {
            rb.linearDamping = rb.angularVelocity.z * 0.1f;
            rb.linearDamping = Mathf.Clamp(rb.linearDamping, 1.5f, 10);

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

        Vector3 velocity = rb.angularVelocity;  // here we get the rigidbody of the car
        explodeHandler.Explode(velocity * 45);  // here we call the explode method in the explode handler script to explode the car
        isExploded = true;

        StartCoroutine(SlowDownTimeCO());
    }
}