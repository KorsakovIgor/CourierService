using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CourierService
{
    /// <summary>
    /// Посылка
    /// </summary>
    public class Package
    {
        //Номер посылки
        public int _id;

        //Пункт отправки
        public int _Departure;

        //Пункт назначения
        public int _Destination;

        //Срочность доставки
        public int _Urgency;

        //рандом
        private static Random rand = new Random();

        //Время поступления заявки
        public TIME time;

        //Крайнее время доставки
        public TIME DeadLine;

        public Package(int _ID, int _NumberOfBranch, int MinUrgency, int MaxUrgency)
        {
            time = new TIME();
            DeadLine = new TIME();
            _id = _ID;
            do
           {
                //генерация пункта отправки и пункта назначения
                _Departure = Package.rand.Next(_NumberOfBranch);
                _Destination = Package.rand.Next(_NumberOfBranch);
            }
            while (_Departure == _Destination);

            //генерация срочности заявки
            _Urgency = Package.rand.Next(MinUrgency, MaxUrgency);
        }
    }

}
