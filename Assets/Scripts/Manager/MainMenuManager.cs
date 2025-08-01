using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;
    // Start is called before the first frame update

    [SerializeField] private string _sceneName;

    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    public AudioClip musicBackground;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        musicSource.clip = musicBackground;
        musicSource.Play();
    }

    public void StartGame()
    {
        musicSource.Stop();
        SceneManager.LoadScene(_sceneName);
    }
}
