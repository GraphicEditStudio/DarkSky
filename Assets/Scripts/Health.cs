using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour
{
    [Header("Health")]
    public GameObject _deadScreen;
    [Tooltip("Current Health")]
    public float _currentHealth;
    [Tooltip("Maximum Health")]
    public float _maxHealth;
    public Image _healthBar;
    private Slider slider;

    public void SetMaxHealth(int health)
    {

        slider.maxValue = health;
        slider.value = health;

    }

    public void SetHealth(int health)
    {

        slider.value = health;

    }

    //HealthSystem and dead screen
    private void OnTriggerEnter(Collider other)
    {
        //Check to see if the tag on the collider is equal to Enemy
        if (other.gameObject.tag == "Enemy")
        {
            _currentHealth -= 25f;
            _healthBar.fillAmount = _currentHealth / _maxHealth;

            //Debug.Log("Was hit");
        }

        if (_currentHealth <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            _deadScreen.SetActive(true);
        }

    }

}
