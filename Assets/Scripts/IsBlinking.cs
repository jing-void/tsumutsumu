using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsBlinking : MonoBehaviour
{
    private float speed = 1.0f;
    [SerializeField] Text text;
    private float time;
    
    void Start()
    {
        text = this.gameObject.GetComponent<Text>();
    }

    void Update()
    {
         text.color = AlphaColor(text.color);

    }

    Color AlphaColor(Color color)
    {
        time += Time.deltaTime * 5.0f * speed;
        color.a = Mathf.Sin(time);

        return color;
    }
}
