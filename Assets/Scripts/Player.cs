using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    [Header("Player Settings"), Space(5)]
    [SerializeField] float maxPlayerHealth;

    [Header("References"), Space(5)]
    [SerializeField] Image healthBarFiller;
    [SerializeField] GameObject restartCanvas;
    [SerializeField] AudioClip hurt, gameOver;

    AudioSource audioSource;

    float currentPlayerHealth;


    private void Awake()
    {
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        audioSource = gameObject.GetComponent<AudioSource>();

        currentPlayerHealth = maxPlayerHealth;
        healthBarFiller.fillAmount = currentPlayerHealth / maxPlayerHealth;
    }

    public void TakeDamage(float amount)
    {
        currentPlayerHealth -= amount;

        //If the player dies the Game Over screen is activated and the game stops else just updates the health bar
        if (currentPlayerHealth <= 0)
        {
            Time.timeScale = 0f;
            restartCanvas.SetActive(true);
            audioSource.PlayOneShot(gameOver);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            healthBarFiller.fillAmount = currentPlayerHealth / maxPlayerHealth;
        }
        else
        {
            audioSource.PlayOneShot(hurt);
            healthBarFiller.fillAmount = currentPlayerHealth / maxPlayerHealth;
        }
    }
}
