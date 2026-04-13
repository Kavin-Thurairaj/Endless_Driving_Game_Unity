using System.Collections;
using UnityEngine;

public class EndlessLevelHandler : MonoBehaviour
{
    [SerializeField]
    GameObject[] sectionPrefabs;

    GameObject[] sectionPool = new GameObject[20];

    GameObject[] section = new GameObject[10];

    Transform playerCarTransform;

    WaitForSeconds waitFor100ms = new WaitForSeconds(0.1f);

    const float sectionLength = 26;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCarTransform = GameObject.FindGameObjectWithTag("Player").transform;  // here we get the Car object in the world and access its transform.

        int prefabIndex = 0;

        for (int i = 0; i<sectionPool.Length; i++)
        {
            sectionPool[i] = Instantiate(sectionPrefabs[prefabIndex]);
            sectionPool[i].SetActive(false);

            prefabIndex++;

            if (prefabIndex>sectionPrefabs.Length-1)
            {
                prefabIndex = 0;
            }

        }

        for (int i = 0; i<section.Length;i++)
        {
            GameObject randomSelection = GetRandomSectionFromPool();  // random prefab object will be selected .

            randomSelection.transform.position = new Vector3(sectionPool[i].transform.position.x,0,i*sectionLength);
            randomSelection.SetActive(true);
            section[i] = randomSelection;
        }



        StartCoroutine(UpdateLessOften());
    }

    IEnumerator UpdateLessOften()
    {
        while (true)
        {
            yield return waitFor100ms;
        }

    }

    GameObject GetRandomSectionFromPool()
    {
        int randomInt = Random.Range(0, sectionPool.Length);

        bool isNewSectionFound = false;

        while (!isNewSectionFound)
        {
            if (!sectionPool[randomInt].activeInHierarchy)
            {
                isNewSectionFound = true;
            }
            else
            {
                randomInt++;
            }

            if (randomInt>sectionPool.Length-1)
            {
                randomInt = 0;
            }
        }

        return sectionPool[randomInt];
    }
}
