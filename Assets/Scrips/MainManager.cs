using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    public TMP_Text text_name;
    public TMP_InputField input_Score;

    DatabaseReference databaseReference;
    FirebaseAuth auth;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
       
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuthManager.Instance.auth;
    }

 

    public void SaveScore()
    {
        string score = input_Score.text;
        string userId = auth.CurrentUser.UserId;
        DatabaseReference scoreRef = databaseReference.Child("users").Child(userId).Child("score");

        scoreRef.SetValueAsync(score);



        GetCurrentScore();
    }


    

    public void GetCurrentScore()
    {
        StartCoroutine(GetScore((int ccc) =>
        {

            text_name.text = ccc.ToString();
        }));
    }
    public IEnumerator GetScore(Action<int> onCallback)
    {
        string userId = auth.CurrentUser.UserId;

        var score = databaseReference.Child("users").Child(userId).Child("score").GetValueAsync();


        yield return new WaitUntil(predicate: () => score.IsCompleted);

        if (score != null)
        {
            DataSnapshot snapshot =score.Result;
            //Debug.Log(snapshot.Value);
            onCallback.Invoke(int.Parse(snapshot.Value.ToString()));
        }
    }

    public void LogOut()
    {
        FirebaseAuthManager.Instance.LogOut();
    }

 

}
