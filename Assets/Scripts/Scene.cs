using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private Slider musicSlider;

    private float currentVolume;

    public GameObject menu, manuale;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            currentVolume = PlayerPrefs.GetFloat("Volume");
            source.volume = currentVolume;
            musicSlider.value =source.volume ;
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
        
    }

    public void loadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
    public void QuitGame()
    {
        // Chiude l'applicazione
        Application.Quit();

        // Nota: L'applicazione verrà chiusa solo in build standalone. In editor o in WebGL, questa funzione potrebbe non avere effetto.
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        source.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }

    public void opzioni()
    {
        menu.SetActive(true);
    }

    public void chiudiOpzioni()
    {
        menu.SetActive(false);
    }

    public void apriManuale()
    {
        manuale.SetActive(true);
    }

    public void chiudiManuale()
    {
        manuale.SetActive(false);
    }
}
