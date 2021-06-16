using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class PlayerSpawner : NetworkBehaviour
{
    MainPlayer mainPlayer;
    MovementPlus movePlus;
    Shooting shooting;
    public Behaviour[] scripts;
    Renderer[] renderers;

    void Start()
    {
        mainPlayer = gameObject.GetComponent<MainPlayer>();
        movePlus = gameObject.GetComponent<MovementPlus>();
        shooting = gameObject.GetComponent<Shooting>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    void SetPlayerState(bool state)
    {
        foreach (var script in scripts)
        {
            script.enabled = state;
        }
        foreach (var renderer in renderers)
        {
            renderer.enabled = state;
        }
    }

    private Vector3 GetRandPos()
    {
        return new Vector3(Random.Range(-13f, 13f), 1f, Random.Range(-13, 13f));
    }

    [ServerRpc]
    void RespawnServerRpc()
    {
        RespawnClientRpc(GetRandPos());
    }

    [ClientRpc]
    void RespawnClientRpc(Vector3 spawnPos)
    {
        StartCoroutine(RespawnDelay(spawnPos));
        //mainPlayer.enabled = false;
        //transform.position = spawnPos;
        //mainPlayer.enabled = true;
    }

    IEnumerator RespawnDelay(Vector3 spanwPos)
    {
        mainPlayer.enabled = false;
        movePlus.enabled = false;
        shooting.enabled = false;
        SetPlayerState(false);
        yield return new WaitForSeconds(3f);
        transform.position = spanwPos;
        mainPlayer.enabled = true;
        movePlus.enabled = true;
        shooting.enabled = true;
        SetPlayerState(true);
    }

    public void Respawn()
    {
        RespawnServerRpc();
    }
}
