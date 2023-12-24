using System.Collections;
using System.Collections.Generic;
using TMPro;
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


    [SerializeField]
    private GameObject notifiPanel;
    [SerializeField]
    private TMP_Text text_Notifi;

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



    public void OpenNotiPanell(string message)
    {
        text_Notifi.text = message;

        StartCoroutine(ShowMessageAsync());
    }

    private IEnumerator ShowMessageAsync()
    {
        StatusNotiPanel(true);
        yield return new WaitForSeconds(3f);
        StatusNotiPanel(false);
    }
    public void StatusNotiPanel(bool status)
    {
        notifiPanel.gameObject.SetActive(status);
    }

    private void OffAllPanel()
    {
        registrationPanel.SetActive(false);
        loginPanel.SetActive(false);
        playGamePanel.SetActive(false);
    }



}
