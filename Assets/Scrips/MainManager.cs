using System;
using System.Collections;
using System.Collections.Generic;
using EasyUI.PickerWheelUI;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    DatabaseReference databaseReference;
    FirebaseUser user;

    [SerializeField] private TMP_Text text_Score;
    [SerializeField] private Button btn_Spin;
    [SerializeField] private PickerWheel pickerWheel;

    int currentScore = 0;
    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        user = FirebaseManager.Instance.user;

        GetCurrentScore();

        btn_Spin.onClick.AddListener(() =>
        {
            btn_Spin.interactable=false;
            pickerWheel.OnSpinEnd(WheelPiece =>
            {
                //GetCurrentScore();
                SaveScore(currentScore+WheelPiece.Amount);
                btn_Spin.interactable = true;
                GetCurrentScore();
            });
            pickerWheel.Spin();
        });
    }



    public void SaveScore(int score)
    {
        string userId = user.UserId;
        DatabaseReference scoreRef = databaseReference.Child("users").Child(userId).Child("score");
        scoreRef.SetValueAsync(score);
    }


    private void Update()
    {
        ChangeScoreText();
    }

    public void GetCurrentScore()
    {
       
        StartCoroutine(GetScore((int temp) =>
        {
            currentScore = temp;
        }));
        ChangeScoreText();
    }

    public IEnumerator GetScore(Action<int> onCallback)
    {
        string userId = user.UserId;

        var scoreTask = databaseReference.Child("users").Child(userId).Child("score").GetValueAsync();

        yield return new WaitUntil(() => scoreTask.IsCompleted);

        if (scoreTask.Exception != null)
        {
            Debug.LogWarning("Failed to retrieve score: " + scoreTask.Exception);
            yield break;
        }

        DataSnapshot snapshot = scoreTask.Result;
        int currentScore = int.Parse(snapshot.Value.ToString());

        onCallback.Invoke(currentScore);
    }


    protected void ChangeScoreText()
    {
        text_Score.text = "Current Score : "+currentScore.ToString();
    }

    public void LogOut()
    {
        FirebaseAuthManager.Instance.LogOut();
    }



}
