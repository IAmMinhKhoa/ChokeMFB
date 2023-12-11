using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FirebaseManager : MonoBehaviour
{
    DatabaseReference databaseReference;
    string userid;


    public TMP_InputField input;

    public TMP_Text text;
    void Start()
    {

        userid = SystemInfo.deviceUniqueIdentifier;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        GetData();
    }


    private void Update()
    {
        GetData();
    }

    public void SaveData()
    {
        string temp = input.text;


        databaseReference.Child("data").Child(userid).SetValueAsync(temp)
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {

                    Debug.LogError("Error to Firebase: " + task.Exception);
                }
                else if (task.IsCompleted)
                {

                    Debug.Log("Success to Firebase");
                }
            });
    }



    public void GetData()
    {
        StartCoroutine(GetDataFromFirebase());
    }

    public IEnumerator GetDataFromFirebase()
    {
        var dataTask = databaseReference.Child("data").Child(userid).GetValueAsync();

        yield return new WaitUntil(() => dataTask.IsCompleted);

        if (dataTask.Exception != null)
        {
            // X?y ra l?i khi l?y d? li?u
            Debug.LogError("L?i khi l?y d? li?u t? Firebase: " + dataTask.Exception);
        }
        else if (dataTask.Result != null && dataTask.Result.Exists)
        {
            string data = dataTask.Result.Value.ToString();
            text.text = data;
            Debug.Log("Firebase: " + data);
        }
        else
        {
            Debug.Log("Không tìm th?y d? li?u trên Firebase");
        }
    }
}