using UnityEngine;
using UnityEngine.SceneManagement;
public class Control : MonoBehaviour
{
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public void ResetTheGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
