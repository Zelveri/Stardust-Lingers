using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using System;

public class SaveSlotBehaviour : MonoBehaviour
{
    public int slotNumber;

    TMPro.TextMeshProUGUI slotText;
    string dateFormat;
    string timeFormat;
    private void Awake()
    {
        // get text field object
        slotText = gameObject.transform.Find("SaveName").GetComponent<TMPro.TextMeshProUGUI>();
        //get ddatetime formatting for region of current user
        dateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToString();
        timeFormat = CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern.ToString();

        // check if slot has savefile and display its date and time
        if (PlayerPrefs.HasKey("Slot" + slotNumber.ToString() + "_Save_Date"))
        {
            // get stored DateTime ticks and convert from string to long (cannot save longs to PlayerPrefs)
            long ticks = long.Parse(PlayerPrefs.GetString("Slot" + slotNumber.ToString() + "_Save_Date"));
            // create new DateTime from ticks and format to string
            slotText.text += slotNumber.ToString() + new DateTime(ticks).ToString("\n" + dateFormat + "\n" + timeFormat);
        } 
        else
        {
            // if slot is empty
            slotText.text += slotNumber.ToString() + "\nEmpty";
        }

        // register button events with functions, dont have to do it in inspector for every prefab instance
        var saveBtn = gameObject.transform.Find("SaveBtn").GetComponent<Button>();
        var loadBtn = gameObject.transform.Find("LoadBtn").GetComponent<Button>();
        saveBtn.onClick.AddListener(OnSave);
        loadBtn.onClick.AddListener(OnLoad);

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // on save button press
    public void OnSave()
    {
        // save data
        SaveFileHandler.Save(slotNumber);
        var now = DateTime.Now;
        // display time of save on slot
        slotText.text = "Slot" + slotNumber.ToString() + now.ToString("\n" + dateFormat + "\n" + timeFormat);
        PlayerPrefs.SetString("Slot" + slotNumber.ToString() + "_Save_Date", now.Ticks.ToString());
        PlayerPrefs.Save();
    }

    // on load button press
    public void OnLoad()
    {
        SaveFileHandler.Load(slotNumber);
    }
}
