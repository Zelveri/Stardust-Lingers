﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuBehaviour : MonoBehaviour
{

    public GameObject settingsUI;
    public GameObject saveUI;

    public UnityEvent OnLoad;

    public UnityEvent OnClose;

    private void Awake()
    {
        //OnLoad = new UnityEvent();
        if(!PlayerPrefs.HasKey("menu_last_open")) PlayerPrefs.SetInt("menu_last_open", 0);
        // load previous seen screen
        var prevMenu = PlayerPrefs.GetInt("menu_last_open");
        if(prevMenu == 0)
        {
            ShowSave();
        }
        else
        {
            ShowSettings();
        }

        // invoked from sceneController
        GameManager.OnMenuClose.AddListener(Close);
    }

    private void Start()
    {
        OnLoad.Invoke();
    }

    private void Close()
    {
        OnClose.Invoke();
    }

    public void OnFileClick()
    {
        ShowSave();
    }

    public void OnReturnClick()
    {
        gameObject.GetComponent<CanvasGroup>().interactable = false;
        GameManager.sceneController.ToggleMenu();
    }

    public void OnGearClick()
    {
        ShowSettings();
    }

    void ShowSettings()
    {
        saveUI.SetActive(false);
        settingsUI.SetActive(true);
        PlayerPrefs.SetInt("menu_last_open",1);
    }

    void ShowSave()
    {
        settingsUI.SetActive(false);
        saveUI.SetActive(true);
        PlayerPrefs.SetInt("menu_last_open", 0);
    }

    public void OnExitClick()
    {
        gameObject.GetComponent<CanvasGroup>().interactable = false;
        //GameManager.Quit();
        GameManager.sceneController.ToMainMenu();
    }
}
