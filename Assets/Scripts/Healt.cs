using UnityEngine;
using UnityEngine.UI;


public class Healt : MonoBehaviour
{
    [Header("Healt")]
    //public GameObject m_deadScreen;
    [Tooltip("Current Healt")]
    public float _currentHealt;
    [Tooltip("Maximum Healt")]
    public float _maxHealt;
    public Image _healtBar;

    private Slider slider;

    public void SetMaxHealt(int health)
    {

        slider.maxValue = health;
        slider.value = health;

    }

    public void SetHealt(int health)
    {

        slider.value = health;

    }

    //HealthSystem and dead screen
    private void OnTriggerEnter(Collider other)
    {
        //Check to see if the tag on the collider is equal to Enemy
        if (other.gameObject.tag == "Enemy")
        {
            _currentHealt -= 25f;
            _healtBar.fillAmount = _currentHealt / _maxHealt;

            Debug.Log("Was hit");
        }

        //if (_currentHealt <= 0) m_deadScreen.SetActive(true);

    }

}
