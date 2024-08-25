using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PosterSceneHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SwitchToGameScene()
    {
        Invoke("WaitAndLoad", 0.2f);
    }
    public void WaitAndLoad()
    {
        SceneManager.LoadScene(1);
    }
}
