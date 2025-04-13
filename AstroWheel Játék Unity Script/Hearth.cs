using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hearth : MonoBehaviour
{
    public void BackToOverView()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
