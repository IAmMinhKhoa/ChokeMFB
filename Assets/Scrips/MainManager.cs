using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using UnityEngine;

public class MainManager : MonoBehaviour
{

    public TMP_Text text_name;


    DatabaseReference databaseReference;
    FirebaseAuth auth;
    private void Start()
    {
        //text_name.text = FirebaseAuthManager.Instance.GetNameUser();
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuthManager.Instance.auth;
        SaveScore(6969);

        // StartCoroutine(LoadScore());
        GetD();
    }


    public void SaveScore(int score)
    {
        // L?y ID ng??i dùng hi?n t?i
        string userId = auth.CurrentUser.UserId;

        // T?o m?t ???ng d?n duy nh?t trong Realtime Database ?? l?u ?i?m s? c?a ng??i dùng
        DatabaseReference scoreRef = databaseReference.Child("scores").Child(userId);

        // L?u ?i?m s? vào Realtime Database
        scoreRef.SetValueAsync(score);
       

    }


    public void GetD()
    {
        StartCoroutine(Getcc((int ccc) =>
        {
            Debug.Log(ccc);
            text_name.text = ccc.ToString();
        }));
    }
    public IEnumerator Getcc(Action<int> onCallback)
    {
        string userId = auth.CurrentUser.UserId;

        var score = databaseReference.Child("scores").Child(userId).GetValueAsync();


        yield return new WaitUntil(predicate: () => score.IsCompleted);

        if (score != null)
        {
            DataSnapshot snapshot =score.Result;
            Debug.Log(snapshot.Value);
            onCallback.Invoke(int.Parse(snapshot.Value.ToString()));
        }
    }
   

    public void GoScence(string nameScence)
    {
        FirebaseAuthManager.Instance.LogOut();  
        UnityEngine.SceneManagement.SceneManager.LoadScene(nameScence);
    }
}
