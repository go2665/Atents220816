using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System;

public class Test_GameOver : Test_Base
{

    protected override void TestInput1(InputAction.CallbackContext obj)
    {
        //// Serializable로 되어 있는 클래스 만들기        
        //SaveData saveData = new();      // 해당 클래스의 인스턴스 만들기
        //saveData.bestScore = 100;       // 인스턴스에 데이터 기록
        //saveData.name = "TestPlayer";

        //string json = JsonUtility.ToJson(saveData);     // 해당 클래스를 json형식의 문자열로 변경

        //string path = $"{Application.dataPath}/Save/";  // 파일을 저장할 폴더를 지정
        //if(!Directory.Exists(path))     // 해당 폴더가 없으면
        //{
        //    Directory.CreateDirectory(path);    // 해당 폴더를 새로 만든다.
        //}

        //string fullPath = $"{path}Save.json";   // 폴더이름과 파일이름을 합쳐서
        //File.WriteAllText(fullPath, json);      // 파일에 json형식의 문자열로 변경한 내용을 저장

        //Debug.Log("세이브 완료");
    }

    protected override void TestInput2(InputAction.CallbackContext obj)
    {
        //string path = $"{Application.dataPath}/Save/";      // 경로 확인용
        //string fullPath = $"{path}Save.json";               // 전체 경로 확인용
        
        //if(Directory.Exists(path) && File.Exists(fullPath)) // 해당 폴더가 있고 파일도 있으면
        //{
        //    string json = File.ReadAllText(fullPath);       // json형식의 데이터 읽기
        //    SaveData loadData = JsonUtility.FromJson<SaveData>(json);   //SaveData 타입에 맞게 파싱
        //    Debug.Log($"Load : {loadData.name}, {loadData.bestScore}"); 
        //}
    }

    protected override void TestInput3(InputAction.CallbackContext obj)
    {
        //GameManager.Inst.TestSetScore(10);
        //GameManager.Inst.RankUpdate();
        //GameManager.Inst.TestSetScore(20);
        //GameManager.Inst.RankUpdate();
        //GameManager.Inst.TestSetScore(30);
        //GameManager.Inst.RankUpdate();
        //GameManager.Inst.TestSetScore(40);
        //GameManager.Inst.RankUpdate();
        //GameManager.Inst.TestSetScore(50);
        //GameManager.Inst.RankUpdate();

        //GameManager.Inst.TestSetScore(35);
        //GameManager.Inst.RankUpdate();

        //int i = 0;
    }
}
