using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HomeScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TitleText;
    [SerializeField] TextMeshProUGUI InstructText;

    private Color _color = new(1, 1, 1, 0);

    private Coroutine FadeInCoroutine;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        FadeInCoroutine = StartCoroutine(FadeInIEnum());
        Debug.Log("Coroutine Started");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopCoroutine(FadeInCoroutine);
            StartCoroutine(FadeOutIEnum());
        }
    }

    IEnumerator FadeInIEnum()
    {
        float LerpDuration = 5;
        float TimeElapsed = 0;

        while (TimeElapsed < LerpDuration)
        {
            SetColor(new Color(1, 1, 1, Mathf.Lerp(0, 1, TimeElapsed/LerpDuration)));
            TimeElapsed += Time.deltaTime;
            yield return null;
        }
        //Ensures color ends at proper end value no matter what
        SetColor(new Color(1, 1, 1, 1));
        Debug.Log("End of Coroutine Reached");
    }

    void SetColor (Color color)
    {
        _color = color;
        TitleText.color = _color;
        InstructText.color = _color;
    }

    IEnumerator FadeOutIEnum()
    {
        float LerpDuration = 1;
        float TimeElapsed = 0;
        float StartFloat = _color.a;

        while (TimeElapsed < LerpDuration)
        {
            SetColor(new Color(1, 1, 1, Mathf.Lerp(StartFloat, 0, TimeElapsed)/LerpDuration));
            TimeElapsed += Time.deltaTime;
            yield return null;
        }

        StateManager.Instance.GameState = StateManager.State.Load;
    }
}
