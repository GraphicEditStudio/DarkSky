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
    [Header("Damage Flash")]
    public Image _damageFX;



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
            _currentHealth -= 5f;
            _healthBar.fillAmount = _currentHealth / _maxHealth;
            ShowDamage();

            //Debug.Log("Was hit");
        }

        if (_currentHealth <= 0)
        {
            _deadScreen.SetActive(true);// call dead screen
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //Time.timeScale = 0;

        }

    }

    void Update()
    {
        if (_damageFX.color.a != 0)
        {
            _damageFX.color = new Color(255f, 0f, 0f, Mathf.MoveTowards(_damageFX.color.a, 0f, 2f * Time.deltaTime));

        }
    }


    public void ShowDamage()
    {

        _damageFX.color = new Color(255f, 0f, 0f, 0.5f);


    }



}
