using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum LevelState { Playing, Complete, Reloading }
    [SerializeField] private LevelState _currentLevelState;

    private Coroutine _nextLevelLoadCoroutine;
    private float _nextLevelLoadDelay = 1.5f;
    private float _reloadLevelDelay = .6f;
    private SoundManager _soundManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        _currentLevelState = LevelState.Playing;
        _soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();

        if (_soundManager == null)
        {
            Debug.LogError("Sound Manager is NULL.");
        }
    }

    private void Update()
    {
        ProcessKeys();
    }

    private void ProcessKeys()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Quit();
        }
    }

    public void LoadNextLevel()
    {
        if (_currentLevelState != LevelState.Playing)
        {
            return;
        }

        _currentLevelState = LevelState.Complete;
        _soundManager.PlaySuccessSound();
        _nextLevelLoadCoroutine = StartCoroutine(LoadNextLevelRoutine());
    }

    IEnumerator LoadNextLevelRoutine()
    {
        yield return new WaitForSeconds(_nextLevelLoadDelay);

        NextLevel();
    }

    private void NextLevel()
    {
        ResetStates();
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings) nextSceneIndex = 0;
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void SkipToNextLevel()
    {
        NextLevel();
    }

    public void ReloadLevel()
    {
        _soundManager.PlayCrashSound();

        if (_nextLevelLoadCoroutine != null)
        {
            StopCoroutine(_nextLevelLoadCoroutine);
            _nextLevelLoadCoroutine = null;
        }

        if (_currentLevelState == LevelState.Reloading) return;

        _currentLevelState = LevelState.Reloading;
        GameObject.FindWithTag("Player").GetComponent<Movement>().enabled = false;

        StartCoroutine(ReloadLevelRoutine());
    }

    IEnumerator ReloadLevelRoutine()
    {
        yield return new WaitForSeconds(_reloadLevelDelay);

        ResetStates();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void ResetStates()
    {
        _currentLevelState = LevelState.Playing;
    }

    public LevelState GetLevelState()
    {
        return _currentLevelState;
    }

    private void Quit()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }

    
}
