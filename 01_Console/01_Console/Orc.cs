using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Console
{
    public class Orc : Character
    {
        int rage = 0;               // 분노 변수 추가
        const int MaxRage = 100;    // 최대 분노. 상수로 설정. 시작부터 끝까지 값이 변하지 않는다.
        bool berserker = false;     // 버서커 상태 표시용

        /// <summary>
        /// 이름을 입력받는 생성자(생성자는 상속이 안되기 때문에 항상 새로 만들어 줘야 한다.)
        /// </summary>
        /// <param name="newName">새 이름</param>
        public Orc(string newName) : base(newName)  // Character(string newName) 실행됨
        {            
        }

        /// <summary>
        /// 스테이터스 생성
        /// </summary>
        public override void GenerateStatus()
        {
            base.GenerateStatus();
            strenth += rand.Next(10) + 1;    // 오크라 힘을 추가로 더함
            rage = 0;   // 시작 분노는 0
        }
        
        /// <summary>
        /// 데미지 처리 함수
        /// </summary>
        /// <param name="damage">받은 데미지</param>
        public override void TakeDamage(int damage)
        {
            // 맞을 때마다 최대 분노의 1/10 증가 + 데미지 10당 1씩 증가
            rage += (int)(MaxRage * 0.1f + damage * 0.1f);
            if(rage >= MaxRage) // 분노가 최대분노를 넘어셔면
            {
                BerserkerMode(true);    // 버서커 모드로 설정
            }
            base.TakeDamage(damage);    // 나에게 데미지 전달
        }

        /// <summary>
        /// 스테이터스 창 출력
        /// </summary>
        public override void PrintStatus()
        {
            Console.WriteLine("┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
            Console.WriteLine($"┃ 이름\t:\t{name}");
            Console.WriteLine($"┃ HP\t:\t{hp,4} / {maxHP,4}");
            Console.WriteLine($"┃ Rage\t:\t{rage,4} / {MaxRage,4}");
            Console.WriteLine($"┃ 힘\t:\t{strenth,2}");
            Console.WriteLine($"┃ 민첩\t:\t{dexterity,2}");
            Console.WriteLine($"┃ 지능\t:\t{intellegence,2}");
            Console.WriteLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
        }

        /// <summary>
        /// 버서커 모드 설정
        /// </summary>
        /// <param name="on">true면 버서커 모드. false 일반 모드</param>
        void BerserkerMode(bool on)
        {
            berserker = on;     // 변수에 세팅해서 표시
            if (berserker)
            {
                strenth *= 2;   // 버서커 모드면 힘이 두배
            }
            else
            {
                //strenth /= 2;
                strenth = strenth >> 1; // 일반 모드로 돌아올 때 힘을 절반으로 줄이기
            }
        }
    }
}
