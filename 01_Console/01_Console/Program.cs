using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");  // "Hello World!"를 출력하는 코드            
            Console.WriteLine("고병조");         // 출력
            //string str = Console.ReadLine();    // 키보드 입력을 받아서 str이라는 string 변수에 저장한다.

            // 변수 : 변하는 숫자. 컴퓨터에서 사용할 데이터를 저장할 수 있는 곳.
            // 변수의 종류 : 데이터 타입(Data type)
            // int : 인티저. 정수. 소수점 없는 숫자. 32bit
            // float : 플로트. 실수. 소수점 있는 숫자. 32bit
            // string : 스트링. 문자열. 글자들을 저장.
            // bool : 불 또는 불리언. true/false를 저장.

            int a = 10; // a라는 인티저 변수에 10이라는 데이터를 넣는다.
            long b = 5000000000;    // 50억은 int에 넣을 수 없다. => int는 32비트이고 32비트로 표현가능한 숫자의 갯수는 2^32(4,294,967,296)개 이기 때문이다.
            int c = -100;
            int d = 2000000000;
            int e = 2000000000;
            int f = d + e;
            Console.WriteLine(f);


            // float의 단점 : 태생적으로 오차가 있을 수 밖에 없다.
            float aa = 0.000123f;
            float ab = 0.999999999999f;
            float ac = 0.000000000001f;
            float ad = ab + ac; // 결과가 1이 아닐 수도 있다.
            Console.WriteLine(ad);

            string str1 = "Hello";
            string str2 = "안녕!";
            string str3 = $"Hello {a}"; //"Hello 10"
            Console.WriteLine(str3);
            string str4 = str1 + str2;  //"Hello안녕!"
            Console.WriteLine(str4);

            bool b1 = true;
            bool b2 = false;

            int level = 1;
            int hp = 10;
            float exp = 0.9f;
            string name = "너굴맨";
            // 실습: 아래 문장을 출력하는 코드를 작성하라(10시20분까지)
            // 너굴맨의 레벨은 1이고 HP는 10이고 exp는 0.9다.
            string result = $"{name}의 레벨은 {level}이고 HP는 {hp}이고 exp는 {exp}다.\n";
            Console.WriteLine(result);
            
            Console.WriteLine($"이름 : {name}\n레벨 : {level}\nHP : {hp}\nexp : {exp}\n\n");

            Console.Write("이름을 입력하세요 : ");
            name = Console.ReadLine();
            Console.Write($"{name}의 레벨을 입력하세요 : ");
            string temp = Console.ReadLine();
            //level = int.Parse(temp);    // string을 int로 변경해주는 코드(숫자로 바꿀 수 있을 때만 가능). 간단하지만 위험함
            //level = Convert.ToInt32(temp);  // string을 int로 변경해주는 코드(숫자로 바꿀 수 있을 때만 가능). 더 세세하게 변경할 수 있다. 그래도 위험.
            int.TryParse(temp, out level);  // string을 int로 변경해주는 코드(숫자로 바꿀 수 없으면 0으로 만든다.)
            result = $"{name}의 레벨은 {level}이고 HP는 {hp}이고 exp는 {exp}다.\n";
            Console.WriteLine(result);


            exp = 0.959595f;
            // 너굴맨의 레벨은 1이고 HP는 10이고 exp는 90%다.
            result = $"{name}의 레벨은 {level}이고 HP는 {hp}이고 exp는 {exp*100:F3}%다.\n";    // exp*100을 소수점 3자리까지 찍는 코드
            Console.WriteLine(result);


            // 이름, 레벨, hp, 경험치를 각각 입력 받고 출력하는 코드 만들기

            Console.ReadKey();                  // 키 입력 대기하는 코드
        }
    }
}
