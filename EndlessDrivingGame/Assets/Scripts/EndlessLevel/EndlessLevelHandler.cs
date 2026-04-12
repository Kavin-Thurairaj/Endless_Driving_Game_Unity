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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
