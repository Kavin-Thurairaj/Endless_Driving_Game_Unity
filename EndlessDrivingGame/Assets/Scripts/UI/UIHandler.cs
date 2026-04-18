using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI distanceTravelText;

    //reference
    CarHandler playerCarHandler;

    [SerializeField]
    TextMeshProUGUI gameOverText;

    [SerializeField]
    CanvasGroup gameOverCanvasGroup;

    private void Awake()
    {
        playerCarHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<CarHandler>();

        //THis is method registering not method Calling
        playerCarHandler.OnPlayerCrashed += PlayerCarHandler_OnPlayerCrashed;  // Here we register the listener method like """OnPlayerCrashed = OnPlayerCrashed + PlayerCarHandler_OnPlayerCrashed;"""

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverCanvasGroup.interactable = false;
        gameOverCanvasGroup.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        distanceTravelText.text = playerCarHandler.DistanceTravel.ToString("000000");
    }

    // This method will wait for three seconds and then execute.
    IEnumerator StartGameOverAnimation()
    {
        yield return new WaitForSecondsRealtime(3.0f);  //this is the 3 seconds.

        gameOverCanvasGroup.interactable = true;

        while (gameOverCanvasGroup.alpha<1f)
        {
            
            gameOverCanvasGroup.alpha = Mathf.MoveTowards(gameOverCanvasGroup.alpha , 1f, Time.deltaTime * 2);

            yield return null;
        }

    }


    void PlayerCarHandler_OnPlayerCrashed(CarHandler obj)  // here internally the C# script will pass the arugument to the register listener method.
    {
        gameOverText.text = $"DISTANCE TRAVELLED {distanceTravelText.text}";  // here we update the game over text with the distance travelled.


        StartCoroutine(StartGameOverAnimation());
    }

    public void OnRestartClicked()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
