using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    private GameObject loginPanel;

    [SerializeField]
    private GameObject registrationPanel;

    [SerializeField]
    private GameObject playGamePanel;

    private void Awake()
    {
        CreateInstance();
    }

    private void CreateInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void OpenLoginPanel()
    {
        OffAllPanel();
        loginPanel.SetActive(true);
  
    }

    public void OpenRegistrationPanel()
    {
        OffAllPanel();
        registrationPanel.SetActive(true);
     
    }

    public void OpenPlayPanel()
    {
        OffAllPanel();
        playGamePanel.SetActive(true);
        
    }

    private void OffAllPanel()
    {
        registrationPanel.SetActive(false);
        loginPanel.SetActive(false);
        playGamePanel.SetActive(false);
    }
}
