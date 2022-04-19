using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanManager : MonoBehaviour
{
    public GameObject human;
    public float value;
    public Vector3 sizeChange;

    public void ResetHuman()
    {
        human.transform.position = new Vector3(13f, 6f, 13f);
        human.transform.rotation = Quaternion.Euler(new Vector3(-70f, 90f, 0f));
        human.transform.localScale = new Vector3(1f, 1f, 1f);
    }
    public void Grow()
    {
        human.transform.localScale *= 1.2f;
    }
    public void Decrease()
    {
        human.transform.localScale *= 0.8f;
    }
    public void Rotate()
    {
        human.transform.Rotate(0f, 30f, 25f);
    }
    public void MoveLeft()
    {
        value = value - 1f;
        human.transform.position = new Vector3(13, 6, value + 13);
    }
    public void MoveRight()
    {
        value = value + 1f;
        human.transform.position = new Vector3(13, 6, value + 13);
    }
}
