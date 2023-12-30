using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cc : MonoBehaviour
{
    int a = 0;
    float time = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        Debug.Log("awake");
    }
    private void OnEnable()
    {
        Debug.Log("OnEnable");
    }
    void Start()
    {
        Debug.Log("start");
        Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    void Update()
    {
        
      
    }
    private void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time < 1)
        {
            a = a + 1;
             Debug.Log(a);
        }
       // Debug.Log(Time.fixedDeltaTime);
    }
}
