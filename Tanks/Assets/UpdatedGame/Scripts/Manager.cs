using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * @brief Manages game states and scene loading.
 */
public class Manager : MonoBehaviour
{
    /**
     * @brief Singleton instance of the Manager class.
     */
    public static Manager Instance { get; private set; }

    /**
     * @brief Initializes the singleton instance and ensures it persists between scenes.
     */
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

    /**
     * @brief Starts a new game on scene start.
     */
    private void Start()
    {
        NewGame();
    }

    /**
     * @brief Checks for reset input during gameplay.
     */
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }

    /**
     * @brief Starts a new game by loading the main game scene.
     */
    private void NewGame()
    {
        LoadScene("_Complete-Game");
    }

    /**
     * @brief Resets the game to its initial state.
     */
    public void ResetGame()
    {
        NewGame();
    }

    /**
     * @brief Delays the game over action by a specified time.
     * @param t The delay time in seconds.
     */
    public void GameOver(float t)
    {
        Invoke(nameof(GameOver), t);
    }

    /**
     * @brief Restarts the game after a game over.
     */
    public void GameOver()
    {
        NewGame();
    }

    /**
     * @brief Delays the game reset action by a specified time.
     * @param t The delay time in seconds.
     */
    public void ResetGame(float t)
    {
        Invoke(nameof(ResetGame), t);
    }

    /**
     * @brief Loads the specified scene.
     * @param scene The name of the scene to load.
     */
    private void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
