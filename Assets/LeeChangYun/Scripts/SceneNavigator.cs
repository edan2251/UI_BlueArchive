using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    public void LoadMainScene()
    {

        SceneManager.LoadScene("Main");
    }
}