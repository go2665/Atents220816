//#define PRINT_DEBUG_INFO

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 싱글톤
// 1. 디자인 패턴 중 하나
// 2. 클래스의 객체(Instance)를 무조건 하나만 생성하는 디자인 패턴
// 3. 데이터를 확신할 수 있다.
// 4. static 맴버를 이용해서 객체에 쉽게 접근 할 수 있도록 해준다.


// Singleton 클래스는 제네릭 타입의 클래스이다.(만들때 타입(T)을 하나 받아야 한다.)
// where 이하에 있는 조건을 만족시켜야 한다.(T는 컴포넌트를 상속받은 타입이어야 한다.)
public class Singleton<T> : MonoBehaviour where T : Component   
{
    private static bool isShutDown = false;
    private static T _instance = null;
    public static T Inst
    {
        get
        {
            if(isShutDown)
            {
#if PRINT_DEBUG_INFO
                Debug.LogWarning($"{typeof(T)} 싱글톤은 이미 삭제되었음.");
#endif
                return null;
            }

            if( _instance == null )
            {
                // 한번도 사용된 적이 없다.
                T obj = FindObjectOfType<T>();              // 같은 타입의 컴포넌트가 게임에 있는지 찾아보기
                if(obj == null)
                {
                    GameObject gameObj = new GameObject();  // 다른 객체가 없으면 새로 만든다.
                    gameObj.name = $"{typeof(T).Name}";
                    obj = gameObj.AddComponent<T>();
                }

                _instance = obj;                            // 찾거나 새로 만든 객체를 인스턴스로 설정한다.
                DontDestroyOnLoad(_instance.gameObject);    // 씬이 사라지더라도 게임 오브젝트를 삭제하지 않게 하는 코드
            }
#if PRINT_DEBUG_INFO
            Debug.Log($"Singleton({_instance.gameObject.name}) : Get");
#endif
            return _instance;   // 무조건 null이 아닌 값이 리턴된다.
        }
    }

    /// <summary>
    /// 오브젝트가 생성 완료된 직후에 호출(씬에 싱글톤 오브젝트가 여러개 배치된 상황일 때 처리를 위해 작성)
    /// </summary>
    private void Awake()
    {
#if PRINT_DEBUG_INFO
        Debug.Log($"Singleton({this.gameObject.name}) : Awake");
#endif
        if (_instance == null)
        {
            // 처음 생성 완료된 싱글톤 게임 오브젝트
            _instance = this as T;                      // _instance에 이 스크립트의 객체 저장
            DontDestroyOnLoad(_instance.gameObject);    // 씬이 사라지더라도 게임 오브젝트를 삭제하지 않게 하는 코드
        }
        else
        {
            // 첫번째 이후에 만들어진 싱글톤 게임 오브젝트
            if( _instance != this )
            {
                Destroy(this.gameObject);       // 내가 아닌 같은 종류의 오브젝트가 이미 있으면 자신을 바로 삭제
            }
        }
    }

    private void OnEnable()
    {
#if PRINT_DEBUG_INFO
        Debug.Log($"Singleton({this.gameObject.name}) : OnEnable");
#endif
        SceneManager.sceneLoaded += OnSceneLoaded;  // 씬 로드가 완료되면 Initialize 함수 실행
    }

    private void OnDisable()
    {
#if PRINT_DEBUG_INFO
        Debug.Log($"Singleton({this.gameObject.name}) : OnDisable");
#endif
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnApplicationQuit()
    {
#if PRINT_DEBUG_INFO
        Debug.Log($"Singleton({this.gameObject.name}) : Quit");
#endif
        isShutDown = true;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
#if PRINT_DEBUG_INFO
        Debug.Log($"Singleton({this.gameObject.name}) : SceneLoaded");
#endif
        Initialize();   // 씬이 로드 되면 초기화 함수 따로 실행
    }

    /// <summary>
    /// 게임 메니저가 새로 만들어지거나 씬이 로드 되었을 때 실행될 초기화 함수
    /// </summary>
    protected virtual void Initialize()
    {
#if PRINT_DEBUG_INFO
        Debug.Log($"Singleton({this.gameObject.name}) : Initialize");
#endif
    }
}

// static 키워드
// 실행 시점에서 이미 메모리에 위치가 고정되게 하는 한정자 키워드
// 타입이름을 통해서만 맴버에 접근이 가능하다.
// 모든 객체(instance)가 같은 값을 가진다.

// as 키워드
// 예시) a as b;  // a를 b타입으로 캐스팅을 시도한 후 실패하면 null 아니면 b타입으로 변경해서 처리 