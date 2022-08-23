using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Console
{
    public class Human : Character  // Human 클래스는 Character 클래스를 상속 받았다.
    {
        int mp = 100;               // Human은 추가로 MP를 가지고 있음.
        int maxMp = 100;
        
        const int DefenseCount = 3;     // 메뉴에서 한번 방어를 선택할 때 몇번까지 데미지가 감소하는지
        int remainsDefenseCount = 0;    // 남아있는 방어 회수

        /// <summary>
        /// 생성자
        /// </summary>
        public Human()
        {
            // 이게 없으면 Human(string newName)만 존재하게 되기 때문에
            // 자동으로 상속받은 부모의 생성자도 실행됨
        }

        /// <summary>
        /// 이름을 입력받는 생성자
        /// </summary>
        /// <param name="newName">Human의 이름</param>
        public Human(string newName) : base(newName)    // base(newName) == Character(string newName) 실행
        {

        }

        /// <summary>
        /// 스테이터스 생성용(MP도 생성)
        /// </summary>
        public override void GenerateStatus()
        {
            base.GenerateStatus();      // Character의 GenerateStatus 함수 실행
            maxMp = rand.Next() % 100;  // 추가한 변수만 추가로 처리
            mp = maxMp;
        }

        /// <summary>
        /// 스테이터스 창 출력
        /// </summary>
        public override void PrintStatus()
        {
            Console.WriteLine("┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
            Console.WriteLine($"┃ 이름\t:{name,10}             ┃ "); 
            Console.WriteLine($"┃ HP\t:{hp,4} / {maxHP,4}               ┃ ");
            Console.WriteLine($"┃ MP\t:{mp,4} / {maxMp,4}               ┃ ");
            Console.WriteLine($"┃ 힘\t:{strenth,3}                       ┃ ");
            Console.WriteLine($"┃ 민첩\t:{dexterity,3}                       ┃ ");
            Console.WriteLine($"┃ 지능\t:{intellegence,3}                       ┃ ");
            Console.WriteLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
        }

        /// <summary>
        /// 공격 함수
        /// </summary>
        /// <param name="target">공격 대상</param>
        public override void Attack(Character target)
        {
            int damage = strenth;   // 힘을 기반으로 데미지 계산

            // 30% 확률로 크리티컬 터짐
            //rand.NextDouble();  // 0.0 ~ 1.0
            if( rand.NextDouble() < 0.3f)   // 이 조건이 참이면 30% 안쪽으로 들어왔다.
            {
                damage *= 2;    // damage = damage * 2; 크리티컬이 터지면 데미지 두배
                Console.WriteLine("크리티컬 히트!");
            }

            Console.WriteLine($"{name}이 {target.Name}에게 공격을 합니다.(공격력 : {damage})");
            target.TakeDamage(damage);  // 최종 데미지를 대상에게 전달
        }

        /// <summary>
        /// 스킬(지능 기반으로 공격)
        /// </summary>
        /// <param name="target">공격 대상</param>
        public void Skill(Character target)
        {
            // rand.NextDouble() : 0 ~ 1
            // rand.NextDouble() * 1.5f : 0 ~ 1.5
            // ((rand.NextDouble() * 1.5f) + 1) : 1 ~ 2.5

            int requireMP = 10;         // 스킬 사용할 때 필요한 마나량

            if( mp - requireMP > 0 )    // 스킬을 사용할 수 있을 만큼 마나가 있다면
            {
                int damage = (int)(((rand.NextDouble() * 1.5f) + 1) * intellegence);    // 지능을 1 ~ 2.5배 한 결과에서 소수점 제거한 수
                Console.WriteLine($"{name}이 {target.Name}에게 스킬을 사용 합니다.(공격력 : {damage})");
                target.TakeDamage(damage);  // 데미지 전달
                mp -= requireMP;            // mp 감소
            }
            else
            {
                Console.WriteLine("마나가 부족합니다.");
            }
        }

        /// <summary>
        /// 방어함수
        /// </summary>
        public void Defense()
        {
            Console.WriteLine($"3턴간 받는 데미지 반감");
            remainsDefenseCount += DefenseCount;    // 데미지 반감되는 회수 증가
        }

        /// <summary>
        /// 받은 피해 처리 함수
        /// </summary>
        /// <param name="damage">받은 데미지</param>
        public override void TakeDamage(int damage)
        {
            // 방어 회수가 남아있으면
            if( remainsDefenseCount > 0 )
            {
                Console.WriteLine("방어 발동! 받는 데미지가 절반 감소합니다.");
                remainsDefenseCount--;  // 회수 1차감하고
                damage = damage >> 1;   // 데미지 절반으로 줄이기
            }
            base.TakeDamage(damage);    // 나에게 데미지 전달
        }
    }
}
