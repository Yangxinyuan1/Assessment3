using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int Score=0;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

    }

    // Update is called once per frame
    void Update()
    {
        Score_.text = Score.ToString();
    }

    public UnityEvent OnWin;


    public UnityEvent OnFail;

    public UnityEvent OnColider;

    public Text Score_;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "fantuan")
        {
            Destroy(other.gameObject);
            Time.timeScale = 0;
            OnFail?.Invoke();

        }
        if (other.tag=="tiantianquan")
        {
            Destroy(other.gameObject);
            Score += 10;
            OnColider?.Invoke();
            if (Score == 100)
                StartCoroutine(ToWin());

        }
    }

    IEnumerator ToWin()
    {
        yield return new WaitForSeconds(1f);
        OnWin?.Invoke();
    }

    public void ReGame()
    {
        SceneManager.LoadScene("YouYongChiGame");
    }

    public void ExistGame()
    {
        Application.Quit();
    }

   
}
