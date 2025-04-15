
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hearth : MonoBehaviour
{
    public void BackToOverView()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
