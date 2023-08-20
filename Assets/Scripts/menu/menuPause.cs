using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuPause : MonoBehaviour
{
    [SerializeField] private GameObject buttonResume;
    [SerializeField] private GameObject menuResume;
   // [SerializeField] private GameObject buttonRestard;

    private bool pause =false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (pause)
            {
                Resume();
            }
            else
            {
                Pausa();
            }
        }
    }
    public void Pausa()
    {
        pause = true;
        Time.timeScale = 0f;
       // buttonResume.SetActive(false);  
        menuResume.SetActive(true);
    }

    public void Resume()
    {
        pause=false;
        Time.timeScale = 1f;
       // buttonResume.SetActive(true);
        menuResume.SetActive(false);
    }

    public void Restart()
    {
        //RoomController.instance.loadedRooms.Clear();
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void EXIT()
    {
        Debug.Log("Salir");
        Application.Quit();
    }
}
