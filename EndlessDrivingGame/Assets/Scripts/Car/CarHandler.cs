using UnityEngine;

public class CarHandler : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    float accelerationMultiplier = 5;  // Accelaration speed
    float brakeMultiplier = 3;  // Brake Speed
    float steerMultiplier = 5;   // Streeing Speed

    Vector2 input = Vector2.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()  // This function will fire at a fixed time
    {
        if (input.y > 0)  // if the vertical y axis increase then the car will accelerate.
        {
            Accerlerate();
        }
        else
        {
            rb.linearDamping = 0.2f;
        }

        if (input.y < 0)
        {
            Brake();
        }

        Steer();
        
    }

    void Accerlerate()
    {
        rb.linearDamping = 0;

        rb.AddForce(rb.transform.forward * accelerationMultiplier * input.y);  // the car will move forward at speed of 5
    }

    void Brake()
    {
        if (rb.angularVelocity.z < 0)
        {
            return;
        }  
        rb.AddForce(rb.transform.forward * brakeMultiplier * input.y);
    }

    void Steer()
    {
        if (Mathf.Abs(input.x)>0)  // so here we convert the input.x to absolute value and ignore the positive and negative sign.
        {
            rb.AddForce(rb.transform.right * steerMultiplier * input.x);  // the car will more left and right.
                                                                            // if the input.x is positive moves right and input.x is negative move left.
        }
    }


    public void setInput(Vector2 inputVector)  // This is a setter method where the input Vector will be normalize and stored in the input.
    {
        inputVector.Normalize();  // here we normalise the vector value from user input
        input = inputVector;
    }
}