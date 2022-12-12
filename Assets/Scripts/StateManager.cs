using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance;

    public enum State
    {
        Home,
        Load,
        Play,
        Pause,
        GameOver,
        Win
    }

    private State _gameState = State.Home;
    public State GameState
    {
        get => _gameState;
        set
        {
            _gameState = value;
            StateChange();
            Debug.Log("RunningSwitch");
        }
    }

    private void Awake()
    {
        //Singleton instance
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Runs a switch statement, like a start function for all states
    /// </summary>
    void StateChange()
    {
        switch (GameState)
        {
            case State.Home:
                GameManager.Instance.LoadHome();
                break;
            case State.Load:
                StartCoroutine(WaitForSceneLoad("PlayScene"));
                break;
            case State.Play:
                GameManager.Instance.GameInit();
                break;
            case State.Pause:
                GameManager.Instance.GamePause();
                break;
            case State.Win:
            case State.GameOver:
                GameManager.Instance.LoadEnd();
                break;
            default:
                Debug.Log("State Switch Defaulted");
                break;
        }
    }


    //https://forum.unity.com/threads/stop-a-function-till-scene-is-loaded.546646/
    //Waits for scene to load so GameManager can be accessed
    public IEnumerator WaitForSceneLoad(string SceneName)
    {
        SceneManager.LoadScene(SceneName);

        if (SceneManager.GetActiveScene().name != SceneName)
            yield return null;

        switch (SceneName)
        {
            case "PlayScene":
                GameManager.Instance.GameLoad();
                LevelGenerator.Instance.GenerateLevel();
                break;
        }
    }
}
