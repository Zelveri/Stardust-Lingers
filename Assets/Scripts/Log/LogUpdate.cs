using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LogUpdate : MonoBehaviour
{
    TMPro.TextMeshProUGUI textMesh;

    public UnityEvent OnLoad;
    public UnityEvent OnClose;
    private void Awake()
    {
        textMesh = gameObject.transform.Find("TextContainer").gameObject.transform.Find("Text").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        textMesh.text = string.Join("\n\n", DataController.GetLines());
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
