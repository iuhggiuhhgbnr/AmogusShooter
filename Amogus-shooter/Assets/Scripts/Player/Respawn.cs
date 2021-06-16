using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class Respawn : NetworkBehaviour
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

        //Random position from start
        transform.position = new Vector3(Random.Range(-20f,20f), 80, Random.Range(-20f, 20f)) ;
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

    // Update is called once per frame
    void Update()
    {
        if(IsLocalPlayer && Input.GetKeyDown(KeyCode.Y))
        {
            RespawnPlayer();
        }
    }

    public void RespawnPlayer()
    {
        RespawnServerRpc();
    }

    [ServerRpc]
    void RespawnServerRpc()
    {
        RespawnClientRpc(GetRandomSpawn());
    }

    [ClientRpc]
    void RespawnClientRpc(Vector3 spawnPos)
    {
        //transform.position = spawnPos;
        StartCoroutine(RespawnCoroutine(spawnPos));
    }
    Vector3 GetRandomSpawn()
    {
        float x = Random.Range(-30f, 30f);
        float y = 80f;
        float z = Random.Range(-30f, 30f);
        return new Vector3(x, y, z);
    }

    IEnumerator RespawnCoroutine(Vector3 spawnPos)
    {
        mainPlayer.enabled = false;
        movePlus.enabled = false;
        shooting.enabled = false;
        SetPlayerState(false);
        transform.position = new Vector3(1,125,1);
        transform.localEulerAngles = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(5.0f);
        transform.position = spawnPos;
        mainPlayer.enabled = true;
        movePlus.enabled = true;
        shooting.enabled = true;
        SetPlayerState(true);
    }
}
