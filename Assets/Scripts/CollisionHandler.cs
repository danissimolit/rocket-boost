using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private ParticleSystem _successParticle;
    [SerializeField] private ParticleSystem _crashParticle;
    private bool _isCollisionActive = true;


    private void OnCollisionEnter(Collision collision)
    {
        if (!_isCollisionActive)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Its Start!");
                break;
            case "Fuel":
                Debug.Log("Its Fuel!");
                break;
            case "Finish":
                Debug.Log("You sooo good!");
                if (GameManager.Instance.GetLevelState() == GameManager.LevelState.Playing)
                    _successParticle.Play();

                GameManager.Instance.LoadNextLevel();
                break;
            default:
                Debug.Log("IDK what that is 0_0");
                _crashParticle.Play();
                GameManager.Instance.ReloadLevel();
                break;
        }
    }

    private void Update()
    {
        ProccessDebugKeys();
    }

    private void ProccessDebugKeys()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            GameManager.Instance.SkipToNextLevel();
        }

        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            _isCollisionActive = !_isCollisionActive;
        }
    }

    //IEnumerator LoadNextLevelRoutine()
    //{
    //    if (GameManager.Instance.LevelIsComplete == false)
    //    {
    //        _soundManager.PlaySuccessSound();
    //    }
    //    GameManager.Instance.CompleteLevel();

    //    yield return new WaitForSeconds(_nextLevelLoadDelay);

    //    GameManager.Instance.NextLevel();
    //}

    //IEnumerator ReloadLevelRoutine()
    //{
    //    _soundManager.PlayCrashSound();
    //    GetComponent<Movement>().enabled = false;

    //    yield return new WaitForSeconds(_reloadLevelDelay);

    //    GameManager.Instance.ReloadLevel();
    //}
}
