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
    public TMP_InputField input_Score;

    DatabaseReference databaseReference;
    FirebaseUser user;

   
    private void Start()
    {
       
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        user = FirebaseManager.Instance.user;

        Debug.Log(FirebaseManager.Instance.user+"/"+ FirebaseManager.Instance.auth);
    }

 

    public void SaveScore()
    {
        string score = input_Score.text;
        string userId = user.UserId;
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
        string userId = user.UserId;

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
