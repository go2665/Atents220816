using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
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

    /// <summary>
    /// 플레이어의 랜더러 컴포넌트
    /// </summary>
    Renderer playerRenderer;

    /// <summary>
    /// 플레이어의 이름을 돌려주는 프로퍼티
    /// </summary>
    public string PlayerName => playerName.Value.ToString();

    private void Awake()
    {
        namePlate = GetComponentInChildren<TextMeshPro>();
        playerRenderer = GetComponentInChildren<Renderer>();

        // 이름이 변경될 때 실행되는 델리게이트에 함수 등록
        playerName.OnValueChanged += OnNameChange;
        color.OnValueChanged += OnColorChange;
    }

    private void OnColorChange(Color previousValue, Color newValue)
    {
        //playerRenderer.material.color = newValue;   // 새 color 값으로 머티리얼 컬러 설정(Standard 셰이더를 사용했을때)
        playerRenderer.material.SetColor("_BaseColor", color.Value);    // 셰이더가 변경되면서 직접 설정하는 것으로 수정(셰이더 그래프로 만든 커스텀 셰이더여서)
    }

    /// <summary>
    /// 이름이 변경되면 실행되는 함수
    /// </summary>
    /// <param name="previousValue">네트워크 변수가 이전에 가지고 있던 값</param>
    /// <param name="newValue">네트워크 변수에 이번에 설정되는 값</param>
    private void OnNameChange(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
        namePlate.text = newValue.ToString();           // 새 값을 이름판에 설정
        gameObject.name = $"NetPlayer - {newValue}";    // 게임 오브젝트 이름도 변경
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

        playerRenderer.material.SetColor("_BaseColor", color.Value);    // 스폰할 때 저장되어 있던 색상으로 설정
        namePlate.text = playerName.Value.ToString();                   // 스폰할 때 저장되어 있던 이름으로 설정
    }

    /// <summary>
    /// 서버에 플레이어 이름 변경을 요청하는 서버 Rpc
    /// </summary>
    /// <param name="text">새 플레이어 이름</param>
    [ServerRpc]
    public void SetPlayerNameServerRpc(string text)
    {
        playerName.Value = text;
        GameManager.Inst.ConnectName = text;
    }

    [ServerRpc]
    public void SetPlayerColorServerRpc(Color newColor)
    {
        color.Value = newColor;
    }
}
