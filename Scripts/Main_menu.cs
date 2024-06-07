using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1 );
    }

    public void Credits()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +3 );
    }

    public void Creators()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +2 );
    }

    public void HowHow()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +4 );
    }
    public void Unity()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +3 );
    }

    public void Back_credits()
    {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -3 );
    }
    public void Back_creators()
    {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -2 );
    }
}
