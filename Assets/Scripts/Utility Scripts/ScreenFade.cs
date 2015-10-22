using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFade : MonoBehaviour
{
    /// <summary>Speed that the screen fades to and from black</summary>
    public float fadeSpeed = 0.1f;
    /// <summary>Whether or not the scene is still fading in</summary>
    private bool sceneStarting = true;

    public bool m_FadeClearComplete = false;
    public bool m_FadeBlackComplete = false;
    
    void Update ()
    {
        // If the scene is starting...
        if(sceneStarting)
            // ... call the StartScene function.
            StartScene();
    }
    
    void FadeToClear ()
    {
        // Lerp the colour of the texture between itself and transparent.
        GetComponent<Image>().color = Color.Lerp(GetComponent<Image>().color, Color.clear, fadeSpeed * Time.deltaTime);
    }
    
    void FadeToBlack ()
    {
        // Lerp the colour of the texture between itself and black.
        GetComponent<Image>().color = Color.Lerp(GetComponent<Image>().color, Color.black, fadeSpeed * Time.deltaTime);
    }
    
    public void StartScene ()
    {
        // Fade the texture to clear.
        FadeToClear();
        
        // If the texture is almost clear...
        if (GetComponent<Image>().color.a <= 0.1f)
        {
            // ... set the colour to clear and disable the GUITexture.
            GetComponent<Image>().color = Color.clear;
            //GetComponent<Image>().enabled = false;
            
            // The scene is no longer starting.
            sceneStarting = false;
            m_FadeClearComplete = true;
            m_FadeBlackComplete = false;
        }
    }
    
    public void EndScene ()
    {
        // Make sure the texture is enabled.
        //GetComponent<Image>().enabled = true;
        
        // Start fading towards black.
        FadeToBlack();
        
        // If the screen is almost black...
        if (GetComponent<Image>().color.a >= 0.99f)
        {
            m_FadeBlackComplete = true;
            m_FadeClearComplete = false;
        }
    }

    public void FadeClear()
    {
        // Fade the texture to clear.
        FadeToClear();

        // If the texture is almost clear...
        if (GetComponent<Image>().color.a <= 0.05f)
        {
            // ... set the colour to clear and disable the GUITexture.
            GetComponent<Image>().color = Color.clear;
            //GetComponent<Image>().enabled = false;

            // The scene is no longer starting.
            sceneStarting = false;
            m_FadeClearComplete = true;
        }
    }

    public void ResetValues()
    {
        m_FadeBlackComplete = false;
        m_FadeClearComplete = false;
    }
}