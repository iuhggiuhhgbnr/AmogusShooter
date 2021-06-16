using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class BuildingHealth : NetworkBehaviour
{
    [SerializeField]
    NetworkVariableFloat healthBuilding = new NetworkVariableFloat(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly }, 100f);


    private void Update()
    {
        if (healthBuilding.Value <= 0 )
        {
            DestroyBuilding();
        }
    }

    public void BuildingTakeDamage(float damage)
    {
        healthBuilding.Value -= damage;
    }

    void DestroyBuilding()
    {
        DestroyBuildingServerRpc();
    }

    [ServerRpc]
    void DestroyBuildingServerRpc()
    {
        DestroyBuildingClientRpc();
    }

    [ClientRpc]
    void DestroyBuildingClientRpc()
    {
        gameObject.SetActive(false);
    }
}
