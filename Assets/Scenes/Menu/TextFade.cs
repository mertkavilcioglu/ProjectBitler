using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFade : MonoBehaviour
{
    public float fadeTime;
    private TextMeshProUGUI fadeText;
    void Start()
    {
        fadeText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeTime>0)
        {
            fadeTime -= Time.deltaTime;
            //devam edilcek
        }
        
    }
}
