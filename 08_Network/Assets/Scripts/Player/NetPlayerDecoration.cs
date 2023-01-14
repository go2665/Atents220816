using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetPlayerDecoration : NetworkBehaviour
{
    NetworkVariable<Color> color = new NetworkVariable<Color>();

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
    }
}
