using UnityEngine;
using UnityEngine.UI;

/**
 * @brief Manages the User Interface elements.
 */
public class UIManager : MonoBehaviour
{
    /**
     * @brief Reference to the text component displaying player health.
     */
    public Text HealthDisplay;

    /**
     * @brief Reference to the Health component of the player.
     */
    public Health playerHealth;

    /**
     * @brief Updates the health display text.
     */
    void Update()
    {
        HealthDisplay.text = "Player Health \n" + $"                   {playerHealth.health}";
    }
}

