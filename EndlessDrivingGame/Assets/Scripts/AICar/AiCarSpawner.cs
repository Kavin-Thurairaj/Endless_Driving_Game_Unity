using System.Collections;
using UnityEngine;

public class AiCarSpawner : MonoBehaviour
{

    [SerializeField]
    GameObject[] carAIPrefabs;
    
    [SerializeField]
    GameObject[] carAIPool = new GameObject[20];  // an array of game object with a size of 20.


    Transform playerCarTransform;

    WaitForSeconds wait = new WaitForSeconds(0.5f);
    float lastTimeCarSpawn = 0;



    void Start()
    {
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;  // here the script will search for the game object with tag Player and then get its transform.


        int prefabIndex = 0;

        for (int i=0; i<carAIPool.Length; i++)
        {
            carAIPool[i] = Instantiate(carAIPrefabs[prefabIndex]);  //here we create the carAIPrefab and store it in the carAIPool array
            carAIPool[i].SetActive(false);  // the AI car prefab will be set to false initally.

            prefabIndex++;

            if(prefabIndex > carAIPrefabs.Length - 1)  // here we make sure that the prefab index does not go over the carAI prefab length.
            {
                prefabIndex = 0;
            }
        }

        StartCoroutine(UpdateLessOftenCO());
    }

    IEnumerator UpdateLessOftenCO()  // this is a coroutine method where the method can stop and execute later.
    {
        while (true) {
            CleanUpCarBeyondView();
            SpawnNewCars();
            yield return wait;   // this says that wait for the 0.5s to finish and then execute the method.
        }
       
    }

    void SpawnNewCars()
    {
        if (Time.time-lastTimeCarSpawn<2)  // here we check if the car spawn in recently if less than 2 then don't spawn.
        {
            return;
        }

        GameObject carToSpawn = null;

        foreach (GameObject aiCar in carAIPool)  // here we loop through the array of gameobject of 20 and take the GameObject.
        {
            if (aiCar.activeInHierarchy)  // here we check if the gameObject is active in the hierarchy. If so then skip that one.
            {
                continue;
            }

            carToSpawn = aiCar;
            break; 
        }

        if (carToSpawn == null) {  // if no car to spawn.
            return;
        }

        Vector3 spawnPosition = new Vector3 (0, 0, playerCarTransform.transform.position.z*100);  // here this is the spawn position of the Ai car.

        carToSpawn.transform.position = spawnPosition;  // here we given the position to the AI car to spawn.
        carToSpawn.SetActive(true);  // then we set it to active true in the world.

        lastTimeCarSpawn = Time.time;
    }

    void CleanUpCarBeyondView()
    {
        foreach(GameObject aiCar in carAIPool) {
            if (!aiCar.activeInHierarchy) {  // if the car is not yet active then skip it.
                continue;
            }

            if (aiCar.transform.position.z - playerCarTransform.position.z > 200) {   // the ai car will not be shown if the distance between the player and aiCar is high.
                aiCar.SetActive(false);
            }

            if (aiCar.transform.position.z - playerCarTransform.transform.position.z < -50)
            {
                aiCar.SetActive(false);
            }
        
        }
    }
}
