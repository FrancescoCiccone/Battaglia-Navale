using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{    
    public GameObject menu;
    private bool menuActive;

    [SerializeField] private AudioSource source;
    [SerializeField] private Slider musicSlider;

    public GameObject OpzioniMenu;

    private float currentVolume;
    // Start is called before the first frame update
    void Start()
    {
        menuActive = false;

        if (PlayerPrefs.HasKey("Volume"))
        {
            currentVolume = PlayerPrefs.GetFloat("Volume");
            source.volume = currentVolume;
            musicSlider.value = source.volume;
        }
        else
        {
            // Se non ci sono dati salvati, salva il volume iniziale
            currentVolume = source.volume;
            musicSlider.value = source.volume;
            PlayerPrefs.SetFloat("Volume", currentVolume);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && menuActive == false)
        {
            menu.SetActive(true);
            menuActive = true;
            Time.timeScale = 0;
            DisableTileScripts();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && menuActive == true)
        {
            menu.SetActive(false);
            menuActive = false;
            Time.timeScale = 1;
            EnableTileScripts();
        }
    }
    public void exit()
    {
        SceneManager.LoadScene("Menu");
    }

    public void resume()
    {
        menu.SetActive(false);
        menuActive = false;
        Time.timeScale = 1;
        EnableTileScripts();
    }

    void DisableTileScripts()
    {
        TileScript[] tileScripts = FindObjectsOfType<TileScript>();
        foreach (TileScript tileScript in tileScripts)
        {
            tileScript.enabled = false;
        }
    }

    void EnableTileScripts()
    {
        TileScript[] tileScripts = FindObjectsOfType<TileScript>();
        foreach (TileScript tileScript in tileScripts)
        {
            tileScript.enabled = true;
        }
    }

    public void opzioni()
    {
        OpzioniMenu.SetActive(true);
    }

    public void chiudiOpzioni()
    {
        OpzioniMenu.SetActive(false);
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        source.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }
}
