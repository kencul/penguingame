using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance;

    [SerializeField] GameObject Player;

    //Health timer
    private Coroutine HealthCoroutine;
    //Health property
    private float _health;
    public float Health
    {
        get => _health;
        set
        {
            _health = value;
            HealthDebug.text = _health.ToString("00");
        }
    }
    public int InitHealth = 60;
    [SerializeField] TextMeshProUGUI HealthDebug;

    //Food Pickup
    private Coroutine FoodCoroutine;
    [SerializeField] float FoodHealSpeed = 1f;

    //Emergency state to make sure end text is faded in
    public bool TextFadedIn = false;

    //Reference to Pause UI
    [SerializeField] GameObject PauseUI;

    private void Awake()
    {
        //Singleton instance
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        Health = InitHealth;
        Time.timeScale = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void GameLoad()
    {
        Time.timeScale = 1;
        MetaHealthUI.Instance.TransFadeOut();
    }

    public void GameInit()
    {
        HealthCoroutine = StartCoroutine(Timer());
        PauseUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void GamePause()
    {
        StopCoroutine(HealthCoroutine);
        PauseUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void LoadEnd()
    {
        Destroy(Player);
        StopCoroutine(HealthCoroutine);
        MetaHealthUI.Instance.TransFadeIn();
    }

    public void LoadHome()
    {
        StartCoroutine(MetaHealthUI.Instance.TextFadeOutIEnum());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            StateManager.Instance.GameState = StateManager.Instance.GameState == StateManager.State.Play ? StateManager.State.Pause : StateManager.State.Play;

        if (TextFadedIn)
        {
            if (StateManager.Instance.GameState == StateManager.State.Win || StateManager.Instance.GameState == StateManager.State.GameOver)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                    StateManager.Instance.GameState = StateManager.State.Home;
            }
        }
    }

    public void FoodPickup()
    {
        StopCoroutine(HealthCoroutine);
        FoodCoroutine = StartCoroutine(FoodIEnum());
    }

    //https://gamedevbeginner.com/the-right-way-to-lerp-in-unity-with-examples/#:~:text=What%20is%20Lerp%20in%20Unity,over%20a%20period%20of%20time.
    /// <summary>
    /// Lerps between health and +10 seconds over 1 second, then restarts natural health decrease
    /// </summary>
    /// <returns></returns>
    IEnumerator FoodIEnum()
    {
        float StartValue = Health;
        float EndValue = StartValue + 10;
        float LerpDuration = FoodHealSpeed;
        float TimeElapsed = 0;

        while(TimeElapsed < LerpDuration)
        {
            Health = Mathf.Lerp(Health, EndValue, TimeElapsed / LerpDuration);
            TimeElapsed += Time.deltaTime;
            yield return null;
        }
        //Ensures Health ends at proper end value no matter what
        Health = EndValue;
        HealthCoroutine = StartCoroutine(Timer());
    }

    /// <summary>
    /// Decrements health, where 1 hp = one second
    /// </summary>
    /// <returns></returns>
    IEnumerator Timer()
    {
        while (Health > 0)
        {
            yield return null;
            Health -= Time.deltaTime;
        }
        StateManager.Instance.GameState = StateManager.State.GameOver;
        StopCoroutine(HealthCoroutine);
    }

    
}
