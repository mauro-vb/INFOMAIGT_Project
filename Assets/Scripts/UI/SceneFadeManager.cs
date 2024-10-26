using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeManager : MonoBehaviour
{
    public Image fadeInImage;
    public Animator fadeInAnimator;

    public Image fadeOutImage;
    public Animator fadeOutAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        if (fadeInImage != null)
        {
            fadeInImage.gameObject.SetActive(true);
        }
        
        if (fadeOutImage != null)
        {
            fadeOutImage.gameObject.SetActive(false);
        }
        
        if (fadeInAnimator)
        {
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
        if (fadeInAnimator != null && fadeInImage != null)
        {
            fadeInAnimator.SetBool("Fade", true);
            yield return new WaitUntil(() => fadeInImage.color.a == 0.0f); 
            fadeInImage.gameObject.SetActive(false);
        }
    }

    /* Called by LevelManager whenever the scene is done */
    public IEnumerator FadeOut()
    {
        if (fadeOutAnimator != null && fadeOutImage != null)
        {
            fadeOutImage.gameObject.SetActive(true);
            fadeOutAnimator.SetBool("Fade", true);
            yield return new WaitUntil(() => fadeOutImage.color.a == 1f); 
        }
    }
}
