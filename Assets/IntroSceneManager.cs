using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroSceneManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer player;
    [SerializeField] private string nextSceneName = "MainMenu";

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("VideoPlayer is not assigned!");
            return;
        }

        player.loopPointReached += OnVideoFinished;
        player.Play();
    }

    void Update()
    {
        // Enter key on PC is KeyCode.Return
        if (Input.GetKeyDown(KeyCode.Return)) // no KeyCode.Enter, use Return instead [web:5][web:11][web:13]
        {
            LoadNextScene();
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        player.loopPointReached -= OnVideoFinished;
        SceneManager.LoadScene(nextSceneName);
    }
}
