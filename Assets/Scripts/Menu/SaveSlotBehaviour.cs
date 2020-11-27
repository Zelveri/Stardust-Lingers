using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using System;

public class SaveSlotBehaviour : MonoBehaviour
{
    public int slotNumber;

    const string slotName = "Slot";

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

        // remove stored save date if file was not found
        if (!SaveFileHandler.CheckSavefileExists(slotNumber))
        {
            PlayerPrefs.DeleteKey(BuildPrefsKey());
        }

        slotText.text = BuildDisplayString();

        // register button events with functions, dont have to do it in inspector for every prefab instance
        var saveBtn = gameObject.transform.Find("SaveBtn").GetComponent<Button>();
        var loadBtn = gameObject.transform.Find("LoadBtn").GetComponent<Button>();
        saveBtn.onClick.AddListener(OnSave);
        loadBtn.onClick.AddListener(OnLoad);

    }

    // on save button press
    public void OnSave()
    {
        // save data
        SaveFileHandler.Save(slotNumber);
        var now = DateTime.Now;
        // display time of save on slot
        //slotText.text = slotName + " " + slotNumber.ToString() + now.ToString("\n" + dateFormat + "\n" + timeFormat);
        PlayerPrefs.SetString(BuildPrefsKey(), now.Ticks.ToString());
        slotText.text = BuildDisplayString();
        PlayerPrefs.Save();
    }

    string BuildPrefsKey()
    {
        return "Slot" + slotNumber.ToString() + "_Save_Date";
    }

    string BuildDisplayString()
    {
        // check if slot has savefile and display its date and time
        if (PlayerPrefs.HasKey(BuildPrefsKey()))
        {
            // get stored DateTime ticks and convert from string to long (cannot save longs to PlayerPrefs)
            long ticks = long.Parse(PlayerPrefs.GetString(BuildPrefsKey()));
            // create new DateTime from ticks and format to string
            return slotName + " " + slotNumber.ToString() + new DateTime(ticks).ToString("\n" + dateFormat + "\n" + timeFormat);
        }
        else
        {
            // if slot is empty
            return slotName + " " + slotNumber.ToString() + "\nEmpty";
        }
    }

    // on load button press
    public void OnLoad()
    {
        SaveFileHandler.Load(slotNumber);
    }
}
