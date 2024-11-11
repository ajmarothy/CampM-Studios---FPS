using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    private bool _fadeIn = false;
    private bool _fadeOut = false;

    public float timeToFade = 1; // influences the rate of the opacity change aka the speed of the "fade" effect

    // Update is called once per frame
    void Update()
    {
        if (_fadeIn)
        {
            if (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += timeToFade * Time.deltaTime;

                if (canvasGroup.alpha >= 1)
                {
                    _fadeIn = false;
                }
            }
        }

        if (_fadeOut)
        {
            if (canvasGroup.alpha >= 0)
            {
                canvasGroup.alpha -= timeToFade * Time.deltaTime;

                if (canvasGroup.alpha == 0)
                {
                    _fadeOut = false;
                }
            }
        }
    }

    public void FadeIn()
    {
        _fadeIn = true;

    }

    public void FadeOut() 
    { 
        _fadeOut = true;
    
    }
}
