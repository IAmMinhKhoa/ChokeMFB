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
            // N?u ch?a có phiên b?n nào t?n t?i, gán phiên b?n hi?n t?i là Instance
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // N?u ?ã có phiên b?n t?n t?i, h?y b? phiên b?n hi?n t?i
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Kh?i t?o Firebase và các bi?n liên quan ? ?ây
    }

    // Update is called once per frame
    void Update()
    {
        // C?p nh?t logic c?a FirebaseManager ? ?ây
    }
}
