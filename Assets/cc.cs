using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cc : MonoBehaviour
{
    /* private void OnParticleCollision(GameObject other)
     {
         Debug.Log("hêh");
     }*/
    private void Start()
    {
        //Application.targetFrameRate = 30;
    }
    float hehe = 2;
    float time = 0;
    int count = 0;
    private void Update()
    {
         time += Time.deltaTime;
        count += 1;
        if (time > hehe)
        {
            time = 0;
            Debug.Log(count);
            count = 0;
            
        }
    }
}
