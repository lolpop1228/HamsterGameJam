using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlashLightWithBattery : MonoBehaviour
{
    public GameObject flashLight;
    public TextMeshProUGUI batteryText;

    public float maxBattery = 100f;
    public float batteryDrainRate = 10f;     // Battery drain per second when on
    public float batteryRegenRate = 5f;      // Battery regen per second when off
    private AudioSource audioSource;
    public AudioClip openSound;

    private float currentBattery;
    private bool isOn = false;

    void Start()
    {
        currentBattery = maxBattery;
        audioSource = GetComponent<AudioSource>();
        flashLight.SetActive(false);
        UpdateBatteryUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && currentBattery > 0)
        {
            isOn = !isOn;
            flashLight.SetActive(isOn);
            audioSource.PlayOneShot(openSound);
        }

        if (isOn)
        {
            currentBattery -= batteryDrainRate * Time.deltaTime;
            currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);
            
            if (currentBattery <= 0f)
            {
                isOn = false;
                flashLight.SetActive(false);
                audioSource.PlayOneShot(openSound);
            }
        }
        else if (currentBattery < maxBattery)
        {
            currentBattery += batteryRegenRate * Time.deltaTime;
            currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);
        }

        UpdateBatteryUI();
    }

    void UpdateBatteryUI()
    {
        if (batteryText != null)
        {
            batteryText.text = "Battery: " + Mathf.CeilToInt(currentBattery) + "%";
        }
    }
}
