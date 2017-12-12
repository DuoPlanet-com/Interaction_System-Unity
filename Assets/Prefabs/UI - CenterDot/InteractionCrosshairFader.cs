using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionCrosshairFader : MonoBehaviour {

    public float fadeRate = 1.6f;

    [HideInInspector]
    public bool highlight = false;

    Image image;

    float opacity;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (highlight)
        {
            opacity += fadeRate * Time.deltaTime;
        } else
        {
            opacity -= fadeRate * Time.deltaTime;
        }
        opacity = Mathf.Clamp(opacity, 0, 1);

        image.color = new Color(image.color.r, image.color.g, image.color.b, opacity);
    }

}
