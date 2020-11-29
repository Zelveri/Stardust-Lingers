using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LogUpdate : MonoBehaviour
{
    TMPro.TextMeshProUGUI textMesh;
    Scrollbar scrollbar;

    public UnityEvent OnLoad;
    public UnityEvent OnClose;
    private void Awake()
    {
        textMesh = gameObject.transform.Find("TextContainer").gameObject.transform.Find("Text").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        scrollbar = FindObjectOfType<Scrollbar>();
    }
    // Start is called before the first frame update
    void Start()
    {
        textMesh.text = string.Join("\n\n", DataController.Lines);
        scrollbar.value = 0;
        OnLoad.Invoke();
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        OnClose.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
