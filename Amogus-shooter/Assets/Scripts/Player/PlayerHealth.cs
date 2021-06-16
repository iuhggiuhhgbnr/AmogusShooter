using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.NetworkVariable;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField]
    NetworkVariableFloat health = new NetworkVariableFloat(new NetworkVariableSettings {WritePermission = NetworkVariablePermission.OwnerOnly },100f);
    public AudioSource deathSound;

    //health text setup
    public Text healthTextPrefab;
    public Transform healthTextPos;
    private Text healthText;

    //health slider
    public Slider healthSliderPrefab;
    public Transform healthSliderPos;
    Slider healthSlider;

    Respawn respawnPlayer;

    private void Start()
    {
        //healthTextPrefab.enabled = false;
        respawnPlayer = GetComponent<Respawn>();

        //health text
        GameObject canvas = GameObject.FindWithTag("MainCanvas");
        healthText = Instantiate(healthTextPrefab, Vector3.zero, Quaternion.identity) as Text;         
        healthText.transform.SetParent(canvas.transform);

        //health slider
        healthSlider = Instantiate(healthSliderPrefab, Vector3.zero, Quaternion.identity);
        healthSlider.transform.SetParent(canvas.transform);
    }
    private void Update()
    {
        if(health.Value <= 0 && IsLocalPlayer)
        {
            health.Value = 100;
            deathSound.Play();
            gameObject.GetComponent<ScoreScript>().PlayerDeathAddPointServerRpc();
            //StartCoroutine(ResetHealth());
            respawnPlayer.RespawnPlayer();
        }

        //health text 
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(this.transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        healthText.text = health.Value.ToString();

        //health slider
        healthSlider.value = health.Value;

        if (onScreen)
        {
            Vector3 hpPos = Camera.main.WorldToScreenPoint(healthTextPos.position);
            healthText.transform.position = hpPos;

            //hp slider
            Vector3 hpSliderPos = Camera.main.WorldToScreenPoint(healthSliderPos.position);
            healthSlider.transform.position = hpSliderPos;
        }
        else
        {
            healthText.transform.position = new Vector3(100000, 100000, 100000);

            healthSlider.transform.position = new Vector3(100000, 100000, 100000);
        }
    }
    public void TakeDamage(float damage)
    {
        health.Value -= damage;
    }

    private void OnDestroy()
    {
        if (healthText != null)
        {
            Destroy(healthText.gameObject);
        }

        if(healthSlider != null)
        {
            Destroy(healthSlider.gameObject);
        }
    }
}
