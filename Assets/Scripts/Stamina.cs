using UnityEngine;
using System.Collections.Generic;

using UnityEngine.UI;
using System.Collections;
using StarterAssets;

public class Stamina : MonoBehaviour
{


    [Header("Stamina")]
    public GameObject _staminaUI;
    [Tooltip("Current stamina")]
    public float _stamina;
    [Tooltip("Maximum Stamina")]
    public float _maxStamina;
    private float _staminaRechargeRate = 5f;
    private Coroutine _recharge;
    public Image _staminaBar;
    private float _sprintSpeed;

    private FirstPersonController PlayerController;
    private StarterAssetsInputs AssetsInputs;

    void Start()
    {
        PlayerController = GetComponent<FirstPersonController>();
        AssetsInputs = GetComponent<StarterAssetsInputs>();
        _sprintSpeed = PlayerController.SprintSpeed;// get current sprintSpeed
    }


    void Update()
    {

        //Stamina drain
        if (AssetsInputs.sprint)
        {

            if (_stamina > 0)
            {
                _stamina -= 10 * Time.deltaTime * PlayerController._speed;
                _staminaBar.fillAmount = _stamina / _maxStamina;
            }
            else
            {
                PlayerController.SprintSpeed = PlayerController.MoveSpeed;
                _stamina = 0;
            }

            if (_recharge != null)
            {
                StopCoroutine(_recharge);

            }

            _recharge = StartCoroutine(StaminaRecharge());

        }

        // If the stamina is equals to max stamina, then the stamina bar will dissapear
        if (_stamina == _maxStamina)
        {
            _staminaUI.SetActive(false);
            PlayerController.SprintSpeed = _sprintSpeed;// reset SprintSpeed
        }
        else
        {
            _staminaUI.SetActive(true);
        }
    }

    // Coroutine for the stamina recharge
    private IEnumerator StaminaRecharge()
    {
        yield return new WaitForSeconds(1f);

        while (_stamina < _maxStamina)
        {
            _stamina += _staminaRechargeRate / 5;
            if (_stamina > _maxStamina)
                _stamina = _maxStamina;
            _staminaBar.fillAmount = _stamina / _maxStamina;
            yield return new WaitForSeconds(.1f);
        }



    }






}
