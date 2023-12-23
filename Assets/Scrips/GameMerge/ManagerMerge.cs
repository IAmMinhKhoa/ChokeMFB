using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerMerge : MonoBehaviour
{
    public static ManagerMerge instance;
    public GameObject[] ListPrefabObject;

    public Transform point_Spam;
    private void Start()
    {
        instance = this;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int randomNumber = Random.Range(0, 8);
            Instantiate(ListPrefabObject[randomNumber], point_Spam);
        }
    }

    public GameObject SpamNextObject(int index)
    {
        return Instantiate(ListPrefabObject[index]);
    }
}
