using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToLastScene : MonoBehaviour
{
    
    void Start()
    {
        SceneManager.LoadScene("Ending");
    }
    
}
