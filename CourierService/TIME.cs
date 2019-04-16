using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService
{
    /// <summary>
    /// Время в часах и минутах
    /// </summary>
    public class TIME
    {
        //Часы
        public int hour;

        //Минуты
        public int minute;

        public TIME()
        {
            hour = 9;
            minute = 0;
        }

        public void AddTime(int Min)
        {
            minute += Min;
            while(minute>59)
            {
                hour++;
                minute -= 60;
            }
        }

        public void NewDay()
        {
            hour = 9;
            minute = 0;
        }

        public string strTime()
        {
            if (this.minute > 9)
            {
                return this.hour.ToString() + ":" + this.minute.ToString();
            }
            else
            {
                return this.hour.ToString() + ":0" + this.minute.ToString();
            }


        }

        public static int operator -(TIME T1, TIME T2)
        {

            return ((T1.hour * 60 + T1.minute) - (T2.hour * 60 + T2.minute));
        }

        public static TIME operator +(TIME T1, int T2)
        {
            TIME T3 = new TIME();
            T3.minute = T1.minute + T2;
            T3.hour = T1.hour;
            while (T3.minute > 59)
            {
                T3.hour++;
                T3.minute -= 60;
            }
            return T3;
        }

        public static bool operator <(TIME T1, TIME T2)
        {
            if (T1.hour < T2.hour) return true;
            if (T1.hour > T2.hour) return false;
            if (T1.hour == T2.hour)
            {
                if (T1.minute < T2.minute) return true;
                else return false;
            }

            return false;
        }

        public static bool operator >(TIME T1, TIME T2)
        {
            if (T1.hour > T2.hour) return true;
            if (T1.hour < T2.hour) return false;
            if (T1.hour == T2.hour)
            {
                if (T1.minute > T2.minute) return true;
                else return false;
            }

            return false;
        }
    }
}
