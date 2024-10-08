using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{





    public void GameOver()
    {


        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
