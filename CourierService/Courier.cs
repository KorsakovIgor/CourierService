using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService
{
    /// <summary>
    /// Курьер
    /// </summary>
    public class Courier
    {
        //Имя курьера
        public int _Name;

        //Филиал, в котором находится курьер
        public int _Branch;

        // Список посылок у курьера
        public List<Package> Roster;

        //Занятость курьера
        public bool Freedom;

        //Время прибытия курьера
        public TIME time;

        public Courier(int N, int Br)
        {
            _Name = N;
            _Branch = Br;
            Roster = new List<Package>();
            Freedom = true;
            time = new TIME();
        }

        //сортировка посылок по важности
        public void Sort()
        {
            for(int i=0;i<(this.Roster.Count-1);i++)
                for(int j=(i+1);j<this.Roster.Count;j++)
                {
                    if(this.Roster[j].DeadLine<this.Roster[i].DeadLine)
                    {
                        Package z = this.Roster[i];
                        this.Roster[i] = this.Roster[j];
                        this.Roster[j] = z;
                    }
                }
        }

    }
}
