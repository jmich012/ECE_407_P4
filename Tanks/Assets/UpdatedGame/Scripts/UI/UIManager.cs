using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public Text HealthDisplay;
    public Health playerHealth;

    // Update is called once per frame
    void Update()
    {
        HealthDisplay.text = "Player Health \n" + $"                   {playerHealth.health}";
    }
}
