using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using TMPro;
using Firebase.Database;
using UnityEditor.Rendering;

public class FirebaseAuthManager : MonoBehaviour
{
    public static FirebaseAuthManager Instance;
    DatabaseReference databaseReference;
    // Firebase variable
   /* [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;*/

    // Login Variables
    [Space]
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;

    // Registration Variables
    [Space]
    [Header("Registration")]
    public TMP_InputField nameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField confirmPasswordRegisterField;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        }
        // Check that all of the necessary dependencies for firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseManager.Instance.dependencyStatus = task.Result;
 
            if (FirebaseManager.Instance.dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
             
            }
            else
            {
                Debug.LogError("Could not resolve all firebase dependencies: " + FirebaseManager.Instance.dependencyStatus);
            }
        });
    }


    void InitializeFirebase()
    {
        //Set the default instance object
        FirebaseManager.Instance.auth = FirebaseAuth.DefaultInstance;

        FirebaseManager.Instance.auth.StateChanged += AuthStateChanged;
        //AuthStateChanged(this, null);
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (FirebaseManager.Instance.auth.CurrentUser != FirebaseManager.Instance.user)
        {
            bool signedIn = FirebaseManager.Instance.user != FirebaseManager.Instance.auth.CurrentUser && FirebaseManager.Instance.auth.CurrentUser != null;

            if (!signedIn && FirebaseManager.Instance.user != null)
            {
                Debug.Log("Signed out " + FirebaseManager.Instance.user.UserId);
            }

            FirebaseManager.Instance.user = FirebaseManager.Instance.auth.CurrentUser;
            //Debug.Log(user.UserId);
            if (signedIn)
            {
                Debug.Log("Signed in " + FirebaseManager.Instance.user.UserId);
                
            }
        }
    }

    void OnDestroy()
    {
        FirebaseManager.Instance.auth.StateChanged -= AuthStateChanged;
       // FirebaseManager.Instance.auth = null;
    }

    public void Login()
    {
        StartCoroutine(LoginAsync(emailLoginField.text, passwordLoginField.text));
    }

    private IEnumerator LoginAsync(string email, string password)
    {
        var loginTask = FirebaseManager.Instance.auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogError(loginTask.Exception);

            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;


            string failedMessage = "";

            switch (authError)
            {
                case AuthError.InvalidEmail:
                    failedMessage += "Email is invalid";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "Email is missing";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "Password is missing";
                    break;
                default:
                    failedMessage += "Wrong Password";
                    break;
            }

            UIManager.Instance.OpenNotiPanell(failedMessage);
            Debug.Log(failedMessage);
        }
        else
        {
            FirebaseManager.Instance.user = loginTask.Result.User;

            Debug.LogFormat("{0} You Are Successfully Logged In", FirebaseManager.Instance.user.DisplayName);

            UIManager.Instance.OpenPlayPanel();
        }
    }

    public void Register()
    {
        StartCoroutine(RegisterAsync(nameRegisterField.text, emailRegisterField.text, passwordRegisterField.text, confirmPasswordRegisterField.text));
    }

    private IEnumerator RegisterAsync(string name, string email, string password, string confirmPassword)
    {
        if (name == "")
        {
            UIManager.Instance.OpenNotiPanell("User Name is empty");
  
        }
        else if (email == "")
        {
            UIManager.Instance.OpenNotiPanell("Email field is empty");
         
        }
        else if (password.Length <6)
        {
            UIManager.Instance.OpenNotiPanell("Lenght password longer 6 chars");

        }
        else if (passwordRegisterField.text != confirmPasswordRegisterField.text)
        {
            UIManager.Instance.OpenNotiPanell("Password not match");
        }
        else
        {
            var registerTask = FirebaseManager.Instance.auth.CreateUserWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(() => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                //UIManager.Instance.OpenNotiPanell(registerTask.Exception.ToString());
                //Debug.LogError(registerTask.Exception);

                FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string failedMessage = "";
                switch (authError)
                {
                    case AuthError.InvalidEmail:
                        failedMessage += "Email is invalid";
                        break;
                    case AuthError.WrongPassword:
                        failedMessage += "Wrong Password";
                        break;
                    case AuthError.MissingEmail:
                        failedMessage += "Email is missing";
                        break;
                    case AuthError.MissingPassword:
                        failedMessage += "Password is missing";
                        break;
                    default:
                        failedMessage = "Email has been used";
                        break;
                }
                UIManager.Instance.OpenNotiPanell(failedMessage);
               // Debug.Log(failedMessage);
            }
            else
            {
                // Get The User After Registration Success
                FirebaseManager.Instance.user = registerTask.Result.User;

                UserProfile userProfile = new UserProfile { DisplayName = name };

                var updateProfileTask = FirebaseManager.Instance.user.UpdateUserProfileAsync(userProfile);

                yield return new WaitUntil(() => updateProfileTask.IsCompleted);

                if (updateProfileTask.Exception != null)
                {
                    // Delete the user if user update failed
                    FirebaseManager.Instance.user.DeleteAsync();

                    Debug.LogError(updateProfileTask.Exception);

                    FirebaseException firebaseException = updateProfileTask.Exception.GetBaseException() as FirebaseException;
                    AuthError authError = (AuthError)firebaseException.ErrorCode;


                    string failedMessage = "Profile update Failed! Becuase ";
                    switch (authError)
                    {
                        case AuthError.InvalidEmail:
                            failedMessage += "Email is invalid";
                            break;
                        case AuthError.WrongPassword:
                            failedMessage += "Wrong Password";
                            break;
                        case AuthError.MissingEmail:
                            failedMessage += "Email is missing";
                            break;
                        case AuthError.MissingPassword:
                            failedMessage += "Password is missing";
                            break;
                        default:
                            failedMessage = "Profile update Failed";
                            break;
                    }

                    Debug.Log(failedMessage);
                }
                else
                {
                    CreatUser(FirebaseManager.Instance.user.DisplayName, 0);
                    Debug.Log("Registration Sucessful Welcome " + FirebaseManager.Instance.user.DisplayName);
                    UIManager.Instance.OpenLoginPanel();

                }
            }
        }
    }


    public void LogOut()
    {
        if (FirebaseManager.Instance.auth != null && FirebaseManager.Instance.user != null)
        {
            Debug.Log("bhwrethwrt");
            FirebaseManager.Instance.auth.SignOut();
            GoScence("Login"); 
        }
    }

    IEnumerator GoHome()
    {
        yield return new WaitForSeconds(0.5f);
        GoScence("Login");
    }


    public void GoScence(string nameScence)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(nameScence);
    }
    public string GetNameUser()
    {
        return FirebaseManager.Instance.user.DisplayName;
    }
    public void CreatUser( string name, int score)
    {
        User newUser = new User(name, score);
        string json = JsonUtility.ToJson(newUser);
       
        databaseReference.Child("users").Child(FirebaseManager.Instance.user.UserId).SetRawJsonValueAsync(json);
      
    }

}
