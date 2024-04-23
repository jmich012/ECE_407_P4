using UnityEngine;
using UnityEngine.SceneManagement;


public class Manager : MonoBehaviour
{
    public static Manager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            ResetGame();
        }
    }
    private void NewGame()
    {
        LoadScene("_Complete-Game");
    }

    public void ResetGame()
    {
        NewGame();
    }

    public void GameOver(float t)
    {
        Invoke(nameof(GameOver), t);
    }

    public void GameOver()
    {
        NewGame();
    }

    public void ResetGame(float t)
    { Invoke(nameof(ResetGame),t); }

    private void LoadScene(string scene) 
    {
        SceneManager.LoadScene(scene);
    }
}
