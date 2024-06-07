using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    public Movement movement;
    private bool isPaused = false;
    private AudioSource mapMusicSource;
    private AudioSource mapCompleteSFX;
    private bool MCSFXCheck;
    
    
    

    private void Start()
    {
        mapMusicSource = GameObject.Find("MapMusic").GetComponent<AudioSource>();
        mapCompleteSFX = GameObject.Find("MapCompleteSFX").GetComponent<AudioSource>();
    }

    private void Update()
    {
        GameObject playerObject = GameObject.Find("Player");
        Movement movementScript = playerObject.GetComponentInChildren<Movement>();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        else if(Input.GetKeyDown(KeyCode.R) && isPaused)
        {
            movementScript.Respawn();
            TogglePause();
        }
        else if(Input.GetKeyDown(KeyCode.M) && isPaused)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void TogglePause()
    {
        isPaused = !isPaused; 
        Time.timeScale = isPaused ? 0f : 1f; // Pause/unpause the game

        if (mapMusicSource != null)
        {
            if (isPaused)
            {
                if(mapCompleteSFX.isPlaying)
                {
                    mapCompleteSFX.Pause();
                    MCSFXCheck = true;
                }
                

                mapMusicSource.Pause();
                Debug.Log("Game paused");
            }
            else
            {
                if(MCSFXCheck)
                {
                    mapCompleteSFX.UnPause();
                    MCSFXCheck = false;
                }
                mapMusicSource.UnPause();
                Debug.Log("Game unpaused");
            }
        }
    }
}
