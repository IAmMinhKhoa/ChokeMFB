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
            // N?u ch?a có phiên b?n nào t?n t?i, gán phiên b?n hi?n t?i là Instance
            Instance = this;
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // N?u ?ã có phiên b?n t?n t?i, h?y b? phiên b?n hi?n t?i
            Destroy(gameObject);
        }
    }


}
