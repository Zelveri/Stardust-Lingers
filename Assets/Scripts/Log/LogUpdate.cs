using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogUpdate : MonoBehaviour
{
    TMPro.TextMeshProUGUI textMesh;
    private void Awake()
    {
        textMesh = gameObject.transform.Find("TextContainer").gameObject.transform.Find("Text").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        textMesh.text = string.Join("\n\n", DataController.GetLines());
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
