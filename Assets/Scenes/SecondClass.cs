using UnityEngine;
public class SecondClass : MonoBehaviour
{
    IInterface hey;   
    void Start()
    {
        GameObject[] interfaces = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in interfaces)
        {
           hey= obj.GetComponent<IInterface>();
            Debug.Log("getit");
        }
    } 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hey.JustDebug();
        }
    }
}
