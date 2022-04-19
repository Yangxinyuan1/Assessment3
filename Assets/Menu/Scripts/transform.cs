using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class transform : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void goToMenu()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void goToPlayGround()
    {
        SceneManager.LoadScene("Game");
    }
    public void goToSBT()
    {
        SceneManager.LoadScene("SBT");
    }
    public void goToNN()
    {
        SceneManager.LoadScene("nn");
    }
    public void goToYYC()
    {
        SceneManager.LoadScene("YouYongChiGame");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
