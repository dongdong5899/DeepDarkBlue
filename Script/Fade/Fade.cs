using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public static float FADE_TIME_DEFAULT = 0.3f;

    // singleton
    private static Fade _Instance = null;

    private static readonly string Path = "Prefabs";
    private static readonly string Name = "Fade";

    /// <summary>
    /// Get or Create Fade GameObject instance
    /// </summary>
    /// <returns></returns>
    private static Fade GetOrCreate()
    {
        if (_Instance == null)
        {
            GameObject _prefab = Resources.Load($"{Path}/{Name}") as GameObject;
            GameObject _fade = Instantiate(_prefab/*, GameManager.Controller._ScriptParent.transform*/);
            _Instance = _fade.GetComponent<Fade>();
        }

        return _Instance;
    }
    // end of singleton

    // private static readonly variables
    private static readonly float Start = 0.0f;
    private static readonly float End = 1.0f;

    [Header("Background Fade In/Out Image")]
    public Image img_fade;

    /// <summary>
    /// Set fade color.
    /// </summary>
    /// <param name="color">Target color to fade.</param>
    public void SetColor(Color color)
    {
        img_fade.color = color;
    }

    /// <summary>
    /// Fade In.
    /// </summary>
    /// <param name="duration">Fade duration.</param>
    public static void In(float duration, Action callback = null)
    {
        Fade fade = GetOrCreate();
        fade.StopAllCoroutines();
        fade.StartCoroutine(fade.Co_In(duration, callback));
    }

    /// <summary>
    /// Coroutine method for In method.
    /// </summary>
    private IEnumerator Co_In(float duration, Action callback = null)
    {
        Color color = img_fade.color;
        color.a = 1;
        img_fade.raycastTarget = true;

        float time = Start;
        while (color.a > Start)
        {
            time += Time.deltaTime / duration;
            color.a = Mathf.Lerp(End, Start, time);
            img_fade.color = color;
            yield return null;
        }
        callback?.Invoke();
        color.a = 0;
        img_fade.color = color;
        img_fade.raycastTarget = false;
    }

    /// <summary>
    /// Fade Out.
    /// </summary>
    /// <param name="duration">Fade duration.</param>
    public static void Out(float duration, Action callback = null)
    {
        Fade fade = GetOrCreate();
        fade.StopAllCoroutines();
        fade.StartCoroutine(fade.Co_Out(duration, callback));
    }

    /// <summary>
    /// Coroutine method for Out method.
    /// </summary>
    private IEnumerator Co_Out(float duration, Action callback = null)
    {
        Color color = img_fade.color;
        color.a = 0;
        img_fade.raycastTarget = true;

        float time = Start;
        while (color.a < End)
        {
            time += Time.deltaTime / duration;
            color.a = Mathf.Lerp(Start, End, time);
            img_fade.color = color;
            yield return null;
        }
        callback?.Invoke();
        color.a = 1;
        img_fade.color = color;
        img_fade.raycastTarget = false;
    }
}
