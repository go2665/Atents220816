using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class NetworkTestController : MonoBehaviour
{
    public Button startHost;
    public Button startClient;

    public TextMeshProUGUI playersInGame;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        startHost = child.GetComponent<Button>();       // 호스트 시작용 버튼 찾기
        child = transform.GetChild(1);
        startClient = child.GetComponent<Button>();     // 클라이언트 시작용 버튼 찾기

        startHost.onClick.AddListener( () =>            // 호스트 시작 버튼이 클릭 되었을 때
        {
            if(NetworkManager.Singleton.StartHost())    // 호스트로 시작하기(시도)
            {
                GameManager.Inst.Logger.Log("<#00ff00>호스트</color>가 시작되었습니다.");       // 시작에 성공
            }
            else
            {
                GameManager.Inst.Logger.Log("호스트가 시작에 실패했습니다.");    // 시작에 실패
            }
        });

        startClient.onClick.AddListener(() =>           // 클라이언트 시작 버튼이 클릭 되었을 때
        {
            if (NetworkManager.Singleton.StartClient()) // 클라이언트로 시작하기(시도)
            {
                GameManager.Inst.Logger.Log("클라이언트가 연결을 시작되었습니다."); // 시작에 성공
            }
            else
            {
                GameManager.Inst.Logger.Log("클라이언트가 연결에 실패했습니다.");  // 시작에 실패
            }
        });

        child = transform.GetChild(4);
        playersInGame = child.GetComponent<TextMeshProUGUI>();

        GameManager.Inst.onPlayersChange += PlayersInGameUpdate;
    }

    void PlayersInGameUpdate(int newPlayerInGame)
    {
        playersInGame.text = $"Players : {newPlayerInGame}";
    }

}
