using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InforObejct : MonoBehaviour
{
    public int _index;
    protected int _otherIndex;
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        InforObejct otherObject= collision.gameObject.GetComponent<InforObejct>();
        if(otherObject != null)
        {
            if(otherObject._index == _index && _index<ManagerMerge.instance.ListPrefabObject.Length-1)
            {
               
                int thisID = gameObject.GetInstanceID();
                int ortherID = collision.gameObject.GetInstanceID();
                
                if(thisID> ortherID)
                {

                    Vector3 midPosition = (transform.position + collision.transform.position) / 2;
                    GameObject newObject = ManagerMerge.instance.SpamNextObject(_index + 1);
                    newObject.transform.position = midPosition;

                    Destroy(this.gameObject);
                    Destroy(collision.gameObject);
                }
               
               
            }
        }
    }
  
   
}
