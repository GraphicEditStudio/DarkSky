using UnityEngine;
using UnityEngine.SceneManagement;

public class NextChapter : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
        }

    }
}
