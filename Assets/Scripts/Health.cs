using UnityEngine;
using UnityEngine.UI;
using Utils;


public class Health : MonoBehaviour
{
    [Header("Health")]
    public GameObject _deadScreen;
    [Tooltip("Maximum Health")]
    public float _maxHealth;
    public Image _healthBar;
    private Slider slider;
    [Header("Damage Flash")]
    public Image _damageFX;

    private HealthManager healthManager;
    
    void Awake()
    {
        this.healthManager = new HealthManager();
        healthManager.Initialize(_maxHealth);
        this.healthManager.HealthUpdated += OnHealthUpdate;
    }

    private void OnHealthUpdate(float startHealth, float previousHealth, float newHealth)
    {
        if (newHealth < previousHealth)
        {
            _healthBar.fillAmount = healthManager.GetHealthPercentage();
            ShowDamage();
        }
    }


    public void SetMaxHealth(int health)
    {
        healthManager.Initialize(health);
        slider.maxValue = health;
        slider.value = health;

    }

    public void SetHealth(int health)
    {
        healthManager.ManuallySetHealth(health);
        slider.value = health;

    }

    //HealthSystem and dead screen
    private void OnTriggerEnter(Collider other)
    {
        //Check to see if the tag on the collider is equal to Enemy
        var isAlive = true;
        if (other.gameObject.tag == "Enemy")
        {
            isAlive = healthManager.TakeDamage(5f);
            //Debug.Log("Was hit");
        }

        if (!isAlive)
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
