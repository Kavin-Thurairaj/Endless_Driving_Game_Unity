using UnityEngine;
using UnityEngine.SceneManagement;

public class InputHandler : MonoBehaviour
{

    [SerializeField]
    CarHandler carHandler;
    // Update is called once per frame
    void Update()
    {
        Vector2 input = Vector2.zero;

        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        carHandler.setInput(input);

        if (Input.GetKeyDown(KeyCode.R))  // if R key is pressed then the scene will be reloaded.
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // load the scene again.
        }
    }
}
