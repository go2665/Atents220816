using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Logger logger;
    public Logger Logger => logger;

    CinemachineVirtualCamera virtualCamera;
    public CinemachineVirtualCamera VCamera => virtualCamera;

    NetPlayer player;
    public NetPlayer Player => player;

    VirtualPad virtualPad;
        

    protected override void Initialize()
    {
        base.Initialize();
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnect;
    }

    protected override void ManagerDataReset()
    {
        base.ManagerDataReset();
        logger = FindObjectOfType<Logger>();

        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        virtualPad = FindObjectOfType<VirtualPad>();
    }

    private void OnClientConnect(ulong id)
    {
        Logger.Log($"{id} is connected.");

        NetworkObject netObj = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject( id );
        if( netObj.IsOwner )                            // 내 NetPlayer가 접속했으면
        {
            player = netObj.GetComponent<NetPlayer>();  // 게임메니저에 기록해 놓기
            virtualPad.onMoveInput += player.SetInputDir;
        }

        TMP_InputField inputField = FindObjectOfType<TMP_InputField>();
        string name = $"{id} - {inputField.text}";

        NetPlayerDecoration decoration = netObj.GetComponent<NetPlayerDecoration>();
        decoration.SetPlayerNameServerRpc(name);        // 이름판에 이름 쓰기

        netObj.gameObject.name = $"NetPlayer - {name}"; // NetPlayer 게임 오브젝트 이름 변경
    }

}
