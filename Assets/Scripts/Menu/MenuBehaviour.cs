using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnFileClick()
    {
        GameObject fileUI = gameObject.transform.Find("SaveUIPanel").gameObject;
        GameObject settingsUI = gameObject.transform.Find("SettingsUIPanel").gameObject;
        settingsUI.SetActive(false);
        fileUI.SetActive(true);
    }

    public void OnReturnClick()
    {
        SceneController controller = GameObject.Find("SceneController").GetComponent<SceneController>();
        controller.ReturnToStory();
    }

    public void OnGearClick()
    {
        GameObject fileUI = gameObject.transform.Find("SaveUIPanel").gameObject;
        GameObject settingsUI = gameObject.transform.Find("SettingsUIPanel").gameObject;
        fileUI.SetActive(false);
        settingsUI.SetActive(true);
    }

    public void OnExitClick()
    {
        GameManager.Quit();
    }
}
