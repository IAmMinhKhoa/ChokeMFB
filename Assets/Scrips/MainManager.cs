using System;
using System.Collections;
using System.Collections.Generic;
using EasyUI.PickerWheelUI;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static UnityEngine.Application;

public class MainManager : MonoBehaviour
{
    DatabaseReference databaseReference;
    FirebaseUser user;

    [SerializeField] private TMP_Text text_Score;
    [SerializeField] private Button btn_Spin;
    [SerializeField] private PickerWheel pickerWheel;

    [SerializeField] private GameObject parent_PanelListUser;
    [SerializeField] private GameObject child_PanelUser;


    public List<User> ListAllUser =new List<User>();

    int currentScore = 0;
    private void Awake()
    {
        databaseReference = FirebaseManager.Instance.databaseReference;
    }
    private void Start()
    {
       
        user = FirebaseManager.Instance.user;

        GetCurrentScore();
        GetAllCurrentUser();
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
        //Debug.Log(ListAllUser.Count);
        ChangeScoreText();
    }
    private void FixedUpdate()
    {
        GetAllCurrentUser();
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

    public void GetAllCurrentUser()
    {
        StartCoroutine(GetAllUsers((List<User> ListAllUserRealTime) =>
        {
            ListAllUser = ListAllUserRealTime;

            foreach (User Temp_User in ListAllUserRealTime)
            {
               // Debug.Log("User: " + Temp_User.name + " - Score: " + Temp_User.score);

                GameObject instantiatedChild = Instantiate(child_PanelUser, parent_PanelListUser.transform);
                instantiatedChild.GetComponent<PanelUser>().user= Temp_User;
            }
        }));
       
       
    }
    private IEnumerator GetAllUsers(Action<List<User>> onCallback)
    {
        var allUser = databaseReference.Child("users").GetValueAsync();
        yield return new WaitUntil(() => allUser.IsCompleted);
        if (allUser.Exception != null)
        {
            Debug.LogWarning("Failed to retrieve : " + allUser.Exception);
            yield break;
        }
        List<User> userList = new List<User>();
        DataSnapshot snapshot = allUser.Result;
        foreach (DataSnapshot userSnapshot in snapshot.Children)
        {
            Dictionary<string, object> userData = (Dictionary<string, object>)userSnapshot.Value;
            string id = userSnapshot.Key;
            string username = userData["name"].ToString();
            int score = int.Parse(userData["score"].ToString());
            User tempUser = new User(id,username, score);
            userList.Add(tempUser);

        }
       
        onCallback.Invoke(userList);
        
    }





    public void LogOut()
    {
        FirebaseAuthManager.Instance.LogOut();
    }



}
