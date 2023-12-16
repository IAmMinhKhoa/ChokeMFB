using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Application;

public class PanelUser : MonoBehaviour
{
    DatabaseReference databaseReference;

    public TMP_Text text_infor;
    public Button btn_Atk;
    public User user;
    private void Awake()
    {
        databaseReference = FirebaseManager.Instance.databaseReference;
    }
    private void Start()
    {
        Debug.Log(databaseReference);
        text_infor.text = user.name + " : " + user.score;
        btn_Atk.onClick.AddListener(AttackUserRealtime);
    }

    protected void AttackUserRealtime()
    {

        StartCoroutine(attackUser());
    }

    protected IEnumerator attackUser()
    {
        var minusScore = databaseReference.Child("users").Child(user.id).Child("score").SetValueAsync(user.score - 100) ;
        yield return new WaitUntil(() => minusScore.IsCompleted);
        user.score = user.score - 100;
        text_infor.text = user.name + " : " + user.score;

        if (minusScore.Exception != null)
        {
            Debug.LogWarning("Failed to : " + minusScore.Exception);
            yield break;
        }

     
    }
}
