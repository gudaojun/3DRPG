using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCanvas : MonoBehaviour
{
    CanvasGroup canvasGroup;
    public float fadeInTime;
    public float fadeOutTime;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        DontDestroyOnLoad(this.gameObject);
    }

    public IEnumerator FadeCanva(float time)
    {
        yield return FadeIn(fadeInTime);
        yield return FadeOut(fadeOutTime);
    }

    /// <summary>
    /// ½¥Òþ
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator FadeOut(float time)
    {
        while (canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }
        Destroy(canvasGroup.gameObject);
    }
    /// <summary>
    /// ½¥ÏÔ
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator FadeIn(float time)
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }

    }
}
