using UnityEngine;
using Unity.Cinemachine;
public class PlayerCarSpawner : MonoBehaviour
{
    [Header("Car Prefabs")]
    [SerializeField]
    GameObject[] carPrefabs;

    // Instantiated Car
    GameObject instantiatedCar = null;

    [Header("Camera")]
    [SerializeField]
    CinemachineCamera cinemachineCamera;

    [Header("Main Menu")]
    [SerializeField]
    bool isMainMenu = false;

    [SerializeField]
    Transform spawnPostion;

    // Which Car is Selected
    int carIndex = 0;


    //Selected Car from Main Menu
    static GameObject selectedCarPrefab;

    Quaternion carRotation = Quaternion.identity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (isMainMenu)
        {
            // here we get the car mesh renderer and store it as a game object
            //instantiatedCar = Instantiate(carPrefabs[carIndex].GetComponent<CarHandler>().CarMeshRenderer.gameObject);
            instantiatedCar = Instantiate(carPrefabs[carIndex], spawnPostion.position,spawnPostion.rotation);
            instantiatedCar.GetComponent<CarHandler>().enabled = false;
            // here we store the selected car game object.
            selectedCarPrefab = carPrefabs[carIndex];
        }
        else
        {
            if (selectedCarPrefab!=null)
            {
                // here we create the instance of the car that is selected.
                instantiatedCar = Instantiate(selectedCarPrefab);
            }
            else
            {
                // here if we skip the main menu we will spawn only the first car in the array of Car prefabs.
                selectedCarPrefab = Instantiate(carPrefabs[0]);
            }
        }

        //instantiatedCar = Instantiate(carPrefabs[0]);  // Here we get the first prefab from the array and create the instanite and store it in a variable.
        //Debug.Log("Hello From Player Spawner");

        if(cinemachineCamera != null)
        {
            // here the camera will follow the selected Car from the main menu in game.
            cinemachineCamera.Follow = instantiatedCar.transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isMainMenu)
        {
            //Debug.Log(instantiatedCar.name);
            instantiatedCar.transform.Rotate(new Vector3(0, 20, 0) * Time.deltaTime);

            carRotation = instantiatedCar.transform.rotation;
        }
    }

    void ChangeCar()
    {
        // here we destory the instantiate car game object.
        Destroy(instantiatedCar);
        // here we store the car game object mesh renderer in the variable when player chnage the car. 
        //instantiatedCar = Instantiate(carPrefabs[carIndex].GetComponent<CarHandler>().CarMeshRenderer.gameObject);
        instantiatedCar = Instantiate(carPrefabs[carIndex], spawnPostion.position, spawnPostion.rotation);
        instantiatedCar.GetComponent<CarHandler>().enabled = false;
        // here we store the selected car.
        selectedCarPrefab = carPrefabs[carIndex];

        instantiatedCar.transform.rotation = carRotation;
    }

    public void OnNextCarClicked()
    {
        carIndex++;

        if (carIndex>carPrefabs.Length-1)
        {
            carIndex = 0;
        }

        ChangeCar();
    }

    public void OnPreviousCarClicked()
    {
        carIndex--;

        if (carIndex < 0)
        {
            carIndex = carPrefabs.Length - 1;
        }

        ChangeCar();
    }
}
