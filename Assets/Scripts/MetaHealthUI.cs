using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MetaHealthUI : MonoBehaviour
{
    static public MetaHealthUI Instance;

    //Initializing array of snowflake references
    private GameObject[] Snowflakes;
    private List<RawImage> SnowflakeRawImage = new();
    private int NumSnowflakes;

    //Panel Reference
    [SerializeField] GameObject Panel;
    private Image PanelImage;
    [SerializeField] GameObject TransitionPanel;
    private Image TransPanelImage;

    //HP logic vars
    private int MaxHP;
    private int HPDivValue;
    private float HalfHP;

    //Coroutine
    private Coroutine AnimationCoroutine;
    /*private WaitForSeconds AnimationWFS;
    [SerializeField] float AnimationInterval = 0.1f;*/

    //Transition Coroutines
    private Coroutine FadeOutCoroutine;
    private Coroutine FadeInCoroutine;
    private Coroutine TextFadeInCoroutine;

    //Text
    [SerializeField] TextMeshProUGUI WinText;
    [SerializeField] TextMeshProUGUI LoseText;

    private void Awake()
    {
        // Singleton instance
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        //Retrieve MaxHP from gamemanager
        MaxHP = GameManager.Instance.InitHealth;

        //Save half health float
        HalfHP = MaxHP / 2f;

        //Create array of references to all snowflakes
        Snowflakes = GameObject.FindGameObjectsWithTag("Flakes");
        NumSnowflakes = Snowflakes.Length;

        //Create list of references to RawImage component of flakes
        foreach (GameObject Flake in Snowflakes)
            SnowflakeRawImage.Add(Flake.GetComponent<RawImage>());

        //Get Image component of panel
        PanelImage = Panel.GetComponent<Image>();
        TransPanelImage = TransitionPanel.GetComponent<Image>();

        //Determine modulo value for animation (left in ints bc the animation doesn't need to be exact)
        HPDivValue = MaxHP / (Snowflakes.Length);

        //Initialize WFS for animation interval
        //AnimationWFS = new(AnimationInterval);

        AnimationCoroutine = StartCoroutine(AnimationTrigger());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator AnimationTrigger()
    {
        while (true)
        {
            yield return null;
            SnowflakeAnimation(GameManager.Instance.Health);
        }
    }

    private void SnowflakeAnimation(float Health)
    {
        //Subtracting total num of snowflakes from the division makes number go from 0-13 instead of 13-0, as timer counts down
        //
        //Evenly divides max hp 
        int FlakeIteration = (NumSnowflakes - 1) - Mathf.FloorToInt((float)Health / HPDivValue);
        //Debug.Log(FlakeIteration);

        //Run only the req. alpha change functions depending on the index
        if (FlakeIteration < 0)
        {
            SetTransparent(-1);
        }
        else if (FlakeIteration < 1)
        {
            SetAlphaCurrent(FlakeIteration, Health);
            SetTransparent(FlakeIteration);
        }
        else if (FlakeIteration < 2)
        {
            SetAlphaCurrent(FlakeIteration, Health);
            SetAlphaPrev(FlakeIteration, Health);
            SetTransparent(FlakeIteration);
        }
        else if (FlakeIteration < NumSnowflakes - 2)
        {
            SetAlphaCurrent(FlakeIteration, Health);
            SetAlphaPrev(FlakeIteration, Health);
            SetOpaque(FlakeIteration);
            SetTransparent(FlakeIteration);
        }
        else if (FlakeIteration >= NumSnowflakes + 2)
        {
            SetAlphaCurrent(FlakeIteration, Health);
            SetAlphaPrev(FlakeIteration, Health);
            SetOpaque(FlakeIteration);
        }

        SetPanelAlpha(Health);
    }

    /// <summary>
    /// Set all flakes that should already be opaque to opaque
    /// </summary>
    /// <param name="Iter"></param>
    private void SetOpaque(int Iter)
    {
        for (int i = Iter - 2; i >= 0; i--)
        {
            Color color = new(255, 255, 255, 1);
            SnowflakeRawImage[i].color = color;
        }

    }

    /// <summary>
    /// Set all flakes that should already be transparent to transparent
    /// </summary>
    /// <param name="Iter"></param>
    private void SetTransparent(int Iter)
    {
        for (int i = Iter + 1; i < NumSnowflakes; i++)
        {
            Color color = new(255, 255, 255, 0);
            SnowflakeRawImage[i].color = color;
        }
    }

    /// <summary>
    /// Set alpha of color of the current iteration snowflake to variable alpha
    /// </summary>
    /// <param name="Iter"></param>
    /// <param name="Health"></param>
    private void SetAlphaCurrent(int Iter, float Health)
    {
        //Current Index Snowflake (alpha = (0 - 0.5))
        Color color = new(255, 255, 255, ((MaxHP - Health) - Iter * HPDivValue) / (HPDivValue * 2));
        SnowflakeRawImage[Iter].color = color;
    }

    /// <summary>
    /// Set alpha of color of the previous iteration snowflake to variable alpha
    /// </summary>
    /// <param name="Iter"></param>
    /// <param name="Health"></param>
    private void SetAlphaPrev(int Iter, float Health)
    {
        //Previous Index Snowflake (alpha = (0 - 0.5))
        Color color = new(255, 255, 255, ((MaxHP - Health) - Iter * HPDivValue) / (HPDivValue * 2) + 0.5f);
        SnowflakeRawImage[Iter - 1].color = color;
    }

    /// <summary>
    /// Set alpha to scale from half health
    /// </summary>
    /// <param name="Health"></param>
    private void SetPanelAlpha(float Health)
    {
        //Default alpha to 0.2, until 20% hp is left
        //Then slowly increase alpha so alpha reaches 1 when hp reaches 0
        float alpha = Health < MaxHP * 0.2f ? ((MaxHP * 0.2f - Health) / (MaxHP * 0.2f)) * 0.8f + 0.2f : 0.2f;
        Color color = new(125, 255, 255, alpha);
        PanelImage.color = color;
    }

    public void TransFadeOut()
    {
        FadeOutCoroutine = StartCoroutine(TransFadeOutIEnum());
    }

    public void TransFadeIn()
    {
        FadeInCoroutine = StartCoroutine(TransFadeInIEnum());
    }

    IEnumerator TransFadeOutIEnum()
    {
        float LerpDuration = 3;
        float TimeElapsed = 0;

        while (TimeElapsed < LerpDuration)
        {
            //Debug.Log(Mathf.Lerp(255, 0, TimeElapsed));
            TransPanelImage.color = new Color(0.6f, 1, 1, Mathf.Lerp(1, 0, TimeElapsed / LerpDuration));
            TimeElapsed += Time.deltaTime;
            yield return null;
        }

        TransPanelImage.color = new Color32(155, 255, 255, 0);
        StateManager.Instance.GameState = StateManager.State.Play;
        StopCoroutine(FadeOutCoroutine);
    }

    IEnumerator TransFadeInIEnum()
    {
        float LerpDuration = 3;
        float TimeElapsed = 0;

        while (TimeElapsed < LerpDuration)
        {
            //Debug.Log(Mathf.Lerp(255, 0, TimeElapsed));
            TransPanelImage.color = new Color(0.6f, 1, 1, Mathf.Lerp(0, 1, TimeElapsed / LerpDuration));
            TimeElapsed += Time.deltaTime;
            yield return null;
        }

        TransPanelImage.color = new Color32(155, 255, 255, 255);

        TextFadeInCoroutine = StartCoroutine(TextFadeInIEnum(StateManager.Instance.GameState == StateManager.State.Win ? WinText : LoseText));

        StopCoroutine(FadeInCoroutine);
    }

    IEnumerator TextFadeInIEnum(TextMeshProUGUI text)
    {
        text.enabled = true;
        Debug.Log("Text Enabled!");

        float LerpDuration = 3;
        float TimeElapsed = 0;

        while (TimeElapsed < LerpDuration)
        {
            //Debug.Log(Mathf.Lerp(255, 0, TimeElapsed));
            text.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, TimeElapsed / LerpDuration));
            TimeElapsed += Time.deltaTime;
            yield return null;
        }
        Time.timeScale = 0;
        text.color = new Color32(255, 255, 255, 255);
        GameManager.Instance.TextFadedIn = true;
        StopCoroutine(TextFadeInCoroutine);
    }

    public IEnumerator TextFadeOutIEnum()
    {
        TextMeshProUGUI text = WinText.enabled ? WinText : LoseText;

        float LerpDuration = 3;
        float TimeElapsed = 0;

        Time.timeScale = 1;

        while (TimeElapsed < LerpDuration)
        {
            //Debug.Log(Mathf.Lerp(255, 0, TimeElapsed));
            text.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, TimeElapsed / LerpDuration));
            TimeElapsed += Time.deltaTime;
            yield return null;
        }
        Time.timeScale = 0;
        text.color = new Color32(255, 255, 255, 0);

        StartCoroutine(StateManager.Instance.WaitForSceneLoad("HomeScene"));
    }


}
        
