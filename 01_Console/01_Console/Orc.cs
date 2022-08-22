using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Console
{
    public class Orc : Character
    {
        int rage = 0;
        int rageMax = 100;
        bool berserker = false;

        void BerserkerMode(bool on)
        {
            berserker = on;
            if (berserker)
            {
                strenth *= 2;
            }
            else
            {
                //strenth /= 2;
                strenth = strenth >> 1; // 절반으로 줄이기
            }
        }

        public override void TakeDamage(int damage)
        {
            // 맞을 때마다 최대 분노의 1/10 증가 + 데미지 10당 1씩 증가
            rage += (int)(rageMax * 0.1f + damage * 0.1f);
            if(rage >= rageMax)
            {
                BerserkerMode(true);
            }
            base.TakeDamage(damage);
        }
    }
}
