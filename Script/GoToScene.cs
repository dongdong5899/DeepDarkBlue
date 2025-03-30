using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour
{
    public int SceneIdx;
    void Start()
    {
        SceneManager.LoadScene(SceneIdx);
    }
    
}
