using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase;
using UnityEngine;
public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;

    // Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

    private void Awake()
    {
        if (Instance == null)
        {
            // N?u ch?a c� phi�n b?n n�o t?n t?i, g�n phi�n b?n hi?n t?i l� Instance
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // N?u ?� c� phi�n b?n t?n t?i, h?y b? phi�n b?n hi?n t?i
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Kh?i t?o Firebase v� c�c bi?n li�n quan ? ?�y
    }

    // Update is called once per frame
    void Update()
    {
        // C?p nh?t logic c?a FirebaseManager ? ?�y
    }
}
