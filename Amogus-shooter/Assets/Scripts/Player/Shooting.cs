using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public class Shooting : NetworkBehaviour
{
    public ParticleSystem projectilePrefab;
    private ParticleSystem.EmissionModule em;
    float fireRate = 10f;
    float shootTimer = 0f;

    public AudioSource hitMaker;
    public AudioSource hitBuilding;
    public AudioSource gunFire;

    NetworkVariableBool shooting = new NetworkVariableBool(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly }, false);

    void Start()
    {
        em = projectilePrefab.emission;
    }
    void Update()
    {
        if (IsLocalPlayer)
        {
            shooting.Value = Input.GetMouseButton(0);
            shootTimer += Time.deltaTime;

            if (shooting.Value && shootTimer >= 1f / fireRate)
            {
                shootTimer = 0;
                ShootProjectileServerRpc();
                gunFire.Play();
            }
        }
        em.rateOverTime = shooting.Value ? fireRate : 0f;
    }

   [ServerRpc]
    void ShootProjectileServerRpc()
    {
        //Debug.Log("Tset");
        Ray ray = new Ray(projectilePrefab.transform.position, projectilePrefab.transform.forward);
        if(Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            var player = hit.collider.GetComponent<PlayerHealth>();
            if(player != null)
            {
                player.TakeDamage(5f);
                hitMaker.Play();
            }

            //damage building
            var building = hit.collider.GetComponent<BuildingHealth>();
            if(building != null)
            {
                building.BuildingTakeDamage(1f);
                hitBuilding.Play();
            }
        }
    }
}
