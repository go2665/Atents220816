using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class NetPlayerDecoration : NetworkBehaviour
{
    /// <summary>
    /// 플레이어의 색상
    /// </summary>
    NetworkVariable<Color> color = new NetworkVariable<Color>();

    /// <summary>
    /// 플레이어의 이름
    /// </summary>
    NetworkVariable<FixedString32Bytes> playerName = new NetworkVariable<FixedString32Bytes>();

    /// <summary>
    /// 플레이어의 이름판
    /// </summary>
    TextMeshPro namePlate;

    private void Awake()
    {
        namePlate = GetComponentInChildren<TextMeshPro>();

        // 이름이 변경될 때 실행되는 델리게이트에 함수 등록
        playerName.OnValueChanged += OnNameChange;
    }

    /// <summary>
    /// 이름이 변경되면 실행되는 함수
    /// </summary>
    /// <param name="previousValue">네트워크 변수가 이전에 가지고 있던 값</param>
    /// <param name="newValue">네트워크 변수에 이번에 설정되는 값</param>
    private void OnNameChange(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
        namePlate.text = newValue.ToString();   // 새 값을 이름판에 설정
    }

    /// <summary>
    /// 네트워크 상에서 이 네트워크 오브젝트가 스폰되었을 때 실행
    /// </summary>
    public override void OnNetworkSpawn()
    {
        //IsClient;   // 이 NetworkBehaviour 오브젝트는 클라이언트가 가지고 있다.
        //IsOwner;    // 이 NetworkBehaviour를 컨트롤 하고 있다.
        //IsServer;   // 이 NetworkBehaviour 오브젝트는 서버가 가지고 있다.
        //IsHost;     // 이 NetworkBehaviour 오브젝트는 서버이면서 클라이언트인 것이 가지고 있다.

        // 랜더러에 있는 첫번째 머티리얼의 색상을 랜덤으로 변경하기
        if( IsServer )  // 서버일 때만 실행
        {
            // 서버에서 스폰이 되면 color값을 랜덤으로 결정
            color.Value = Random.ColorHSV(0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f);
        }

        Renderer renderer = GetComponentInChildren<Renderer>(); // 랜더러 가져와서
        renderer.material.color = color.Value;                  // color 값으로 머티리얼 컬러 설정
    }

    /// <summary>
    /// 서버에 플레이어 이름 변경을 요청하는 서버 Rpc
    /// </summary>
    /// <param name="text">새 플레이어 이름</param>
    [ServerRpc]
    public void SetPlayerNameServerRpc(string text)
    {
        playerName.Value = text;
    }
}
