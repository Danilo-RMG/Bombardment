using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  // Singleton
   public static GameManager Instance {get; private set;}

  // Constants
   private static readonly string KEY_HIGHEST_SCORE = "HighestScore";

  // API
   public bool isGameOver {get; private set;}

   [Header("Audio")]
   [SerializeField] private AudioSource musicPlayer;
   [SerializeField] private AudioSource GameOverSfx;
   [SerializeField] private AudioSource ocean;

   [Header("Score")]
   [SerializeField] private float score;
   [SerializeField] private int highestScore;
   public float skySpeed = 1f;

   [Header("Pause")]
   public GameObject pauseMenu;
   [HideInInspector] static public bool isPaused {get; private set;}

   void Awake()
    {
     // Singleton
      if(Instance != null && Instance != this)
       {
        Destroy(this);
       }
        else
         {
          Instance = this;
         }

     // Set score
      score = 0;
      highestScore = PlayerPrefs.GetInt(KEY_HIGHEST_SCORE);
    }

   void Start()
    {
     pauseMenu.SetActive(false);
    }

   void Update()
    {
     // Increment score
      if(!isGameOver && !isPaused)
       {
        score += Time.deltaTime;
       }
     // Update highest score
      if(GetScore() > GetHighestScore())
       {
        highestScore = GetScore();
       }
     // Pause game
      if(Input.GetKeyDown(KeyCode.Escape))
       {
        if(isPaused)
         {
          ResumeGame();
         }
          else
           {
            PauseGame();
           }
       }
    }
   public int GetScore()
    {
     return (int) Mathf.Floor(score);
    }
   public int GetHighestScore()
    {
     return highestScore;
    }
   public void EndGame()
    {
     if(isGameOver) return;
      // Set flag
       isGameOver = true;
      // Stop music
       musicPlayer.Stop();
      // Play SFX
       GameOverSfx.Play();
      // Save highest score
       PlayerPrefs.SetInt(KEY_HIGHEST_SCORE, GetHighestScore());
      // Reload scene
       StartCoroutine(ReloadScene(6));
    }
   private IEnumerator ReloadScene(float delay)
    {
     // Wait
      yield return new WaitForSeconds(delay);
     // Reload scene
      string sceneName = SceneManager.GetActiveScene().name;
       SceneManager.LoadScene(sceneName);
    }
   public void PauseGame()
    {
     // Activate pause panel
      pauseMenu.SetActive(true);
     // Stop music
      musicPlayer.Pause();
      ocean.Pause();
     // Stop time
      Time.timeScale = 0f;
     // Set var
      isPaused = true;
    }
   public void ResumeGame()
    {
     // Desable pause panel
      pauseMenu.SetActive(false);
     // Play music
      musicPlayer.Play();
      ocean.Play();
     // Start time
      Time.timeScale = 1f;
     // Set var
      isPaused = false;
    }
  public void GoToScene(string sceneName)
   {
    SceneManager.LoadSceneAsync(sceneName);
   }
  public void QuitApp()
   {
    Application.Quit();
    Debug.Log("Application has quit.");
   }
}
