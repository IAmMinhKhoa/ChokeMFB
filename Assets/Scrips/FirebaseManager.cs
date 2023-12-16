using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase;
using UnityEngine;
using Firebase.Database;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;

    // Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;


    public DatabaseReference databaseReference;
    private void Awake()
    {
        if (Instance == null)
        {
            // N?u ch?a c� phi�n b?n n�o t?n t?i, g�n phi�n b?n hi?n t?i l� Instance
            Instance = this;
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // N?u ?� c� phi�n b?n t?n t?i, h?y b? phi�n b?n hi?n t?i
            Destroy(gameObject);
        }
    }


}
