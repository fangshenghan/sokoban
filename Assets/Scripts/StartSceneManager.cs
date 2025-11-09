using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }
}
