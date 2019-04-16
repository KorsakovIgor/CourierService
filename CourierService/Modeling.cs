using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService
{
    /// <summary>
    /// Моделирование курьерской службы
    /// </summary>
    public class Modeling
    {
        //Количество курьеров и филиалов
        private int NumberOfCourier, NumberOfBranch;

        //рандом
        private static Random rand = new Random();

        //Список Филиалов
        List<Branch> Branches;

        //Список Курьеров
        List<Courier> Couriers;

        //Список заявок
        public List<Package> Packages;

        //Время 
        TIME Time;

        //Счетчик посылок
        public int NumberOfPackages;

        //Общее время холостых пееездов
        public int _Idling;

        //Количество опоздавших посылок
        public int SlowPackage;

        //Общее время опоздания посылок
        public int TimeSlowPackage;

        //Среднее время поездки курьера между филиалами
        private int MidCourier;

        public Modeling(int NOC, int NOB, int MidC)
        {
            _Idling = 0;
            SlowPackage = 0;
            TimeSlowPackage = 0;
            MidCourier = MidC;
            NumberOfCourier = NOC;
            NumberOfBranch = NOB;
            Branches = new List<Branch>();
            Couriers = new List<Courier>();
            Time=new TIME();
            Packages = new List<Package>();
            NumberOfPackages = 0;

            //Инициализация Филиалов
            for (int i=0;i<NOB;i++)
            {
                Branches.Add(new Branch(i));
            }

            //Инициализация Курьеров
            for(int i=0;i<NOC;i++)
            {
                Couriers.Add(new Courier(i, Modeling.rand.Next(NOB)));
            }
        }

        /// <summary>
        /// генерация заявок в течение дня
        /// </summary>
        public void GeneratePackage (out string PACK, int MinPackageN, int MaxPackageN, int MinPackageY,int  MaxPackageY, int MinUrgency, int MaxUrgency)
        {
            PACK = "";
            do
            {
                //Генерация в менее загруженные часы
                if (Time.hour < 12 || Time.hour > 15)
                {
                    Time.AddTime(Modeling.rand.Next(MinPackageN, MaxPackageN));
                    Package pack = new Package(NumberOfPackages++, NumberOfBranch, MinUrgency, MaxUrgency);
                    pack.time.hour = Time.hour;
                    pack.time.minute = Time.minute;
                    pack.DeadLine = pack.time + pack._Urgency;
                    Packages.Add(pack);
                    Branches[pack._Departure].Packages.Add(pack);
                }

                //Генерация в более загруженные часы
                else
                {
                    Time.AddTime(Modeling.rand.Next(MinPackageY, MaxPackageY));
                    Package pack = new Package(NumberOfPackages++, NumberOfBranch, MinUrgency, MaxUrgency);
                    pack.time.hour = Time.hour;
                    pack.time.minute = Time.minute;
                    pack.DeadLine = pack.time + pack._Urgency;
                    Packages.Add(pack);
                    Branches[pack._Departure].Packages.Add(pack);
                }
            }
            while (Time.hour < 17);

            for (int i = 0; i < Packages.Count; i++)
            {
                PACK +=Packages[i].time.strTime() + "  Посылка " + Packages[i]._id.ToString() + "  от филиала " + Packages[i]._Departure.ToString() + "  в филиал " + Packages[i]._Destination.ToString() + "\r\nСрочность доставки: "+Packages[i]._Urgency.ToString()+" мин\r\n\r\n";
                
            }
        }

        /// <summary>
        /// Движение курьеров в течение дня
        /// </summary>
        public void MovingCourier(out string TakePackage, out string OutPackage)
        {
            Time.hour = 9; Time.minute = 0;
            TakePackage = "";
            OutPackage = "";

            //Рабочий день
            while (Packages.Count != 0)
            {
                //Взятие посылок из текущего филиала
                for (int i = 0; i < NumberOfCourier; i++)
                {
                    if (Couriers[i].Freedom)
                    {
                        for (int j = 0; j < Branches[Couriers[i]._Branch].Packages.Count; j++)
                        {
                            if (Branches[Couriers[i]._Branch].Packages.Count == 0) break;
                            if (Time>Branches[Couriers[i]._Branch].Packages[j].time)
                            {
                                Couriers[i].Roster.Add(Branches[Couriers[i]._Branch].Packages[j]);
                                TakePackage += Time.strTime() + " Курьер " + i.ToString() + " взял посылку " + Branches[Couriers[i]._Branch].Packages[j]._id.ToString() + " из филиала " + Branches[Couriers[i]._Branch].Packages[j]._Departure.ToString() + "\r\n\r\n";
                                Branches[Couriers[i]._Branch].Packages.RemoveAt(j);
                                Couriers[i].Freedom = false;
                                j = j - 1;
                            }
                        }

                        Couriers[i].Sort();
                        if (Couriers[i].Freedom == false)
                        {
                            Couriers[i]._Branch = Couriers[i].Roster[0]._Destination;
                            Couriers[i].time.hour = Time.hour; Couriers[i].time.minute = Time.minute;
                            Couriers[i].time.AddTime(MidCourier - 5 + Modeling.rand.Next(16));
                        }

                    }
                }


                //Поиск посылок в других филиалах
                for(int i=0; i<NumberOfCourier; i++)
                {
                    if(Couriers[i].Freedom)
                    {
                        for(int j=0; (j<NumberOfBranch)&&(Couriers[i].Freedom); j++)
                        {
                            if (Branches[j].Packages.Count != 0)
                            {
                                if (Time>Branches[j].Packages[0].time)
                                {
                                    Couriers[i]._Branch = j;
                                    Couriers[i].Freedom = false;
                                    Couriers[i].time.hour = Time.hour; Couriers[i].time.minute = Time.minute;
                                    TakePackage += Time.strTime() + " Курьер " + i.ToString() + " выехал в филиал " + Couriers[i]._Branch.ToString()+"\r\n\r\n";
                                    int Idling = MidCourier - 5 + Modeling.rand.Next(16);
                                    _Idling += Idling;
                                    Couriers[i].time.AddTime(Idling);
                                }
                            }
                        }
                    }
                }

                //Привёз посылку
               for(int i=0;i<NumberOfCourier;i++)
                {
                    if (Couriers[i].Freedom == false)
                    {
                        if (Couriers[i].Roster.Count == 0)
                        {
                            if (Time>Couriers[i].time)
                                Couriers[i].Freedom = true;
                        }
                        else
                        {
                            //если курьер с посылкой
                            if (Time>Couriers[i].time)
                            {
                                
                                for (int j = 0; j < Couriers[i].Roster.Count; j++)
                                {
                                    if (Couriers[i]._Branch ==Couriers[i].Roster[j]._Destination)
                                    {
                                        OutPackage += Time.strTime() + " Курьер " + i.ToString() + " доставил посылку " + Couriers[i].Roster[j]._id.ToString() + " в филиал " + Couriers[i].Roster[j]._Destination.ToString();
                                        
                                        if (Time>(Couriers[i].Roster[j].time+Couriers[i].Roster[j]._Urgency))
                                        {
                                            OutPackage += " c опозданием " + (Time - Couriers[i].Roster[j].DeadLine)+" мин";
                                            SlowPackage++;
                                            TimeSlowPackage+= Time - Couriers[i].Roster[j].DeadLine;
                                        }
                                        OutPackage += "\r\n\r\n";
                                        Packages.Remove(Couriers[i].Roster[j]);
                                        Couriers[i].Roster.RemoveAt(j);
                                        j = j - 1;
                                    }
                                    if (Couriers[i].Roster.Count == 0) break;
                                }


                                if (Couriers[i].Roster.Count == 0)
                                {
                                    Couriers[i].Freedom = true;
                                }
                                else
                                {                   
                                    for (int j = 0; j < Branches[Couriers[i]._Branch].Packages.Count; j++)
                                    {
                                        if (Branches[Couriers[i]._Branch].Packages.Count == 0) break;
                                        if (Time>Branches[Couriers[i]._Branch].Packages[j].time)
                                        {
                                            Couriers[i].Roster.Add(Branches[Couriers[i]._Branch].Packages[j]);
                                            TakePackage += Time.strTime() + " Курьер " + i.ToString() + " взял посылку " + Branches[Couriers[i]._Branch].Packages[j]._id.ToString() + " из филиала " + Branches[Couriers[i]._Branch].Packages[j]._Departure.ToString() + "\r\n\r\n";
                                            Branches[Couriers[i]._Branch].Packages.RemoveAt(j);
                                            Couriers[i].Freedom = false;
                                            j = j - 1;
                                        }
                                    }

                                    Couriers[i].Sort();
                                    Couriers[i]._Branch = Couriers[i].Roster[0]._Destination;
                                    Couriers[i].time.AddTime(MidCourier - 5 + Modeling.rand.Next(16));
                                }
                            }
                        }
                    }
                }

                Time.AddTime(1);      
            }
        }

        /// <summary>
        /// Моделирование одного рабочего дня
        /// </summary>
        public void WorkingDay(out string Str1, out string Str2, out string Str3, int MinPackageN, int MaxPackageN, int MinPackageY, int MaxPackageY, int MinUrgency, int MaxUrgency)
        {
            this.GeneratePackage(out Str1, MinPackageN, MaxPackageN, MinPackageY, MaxPackageY, MinUrgency, MaxUrgency);
            this.MovingCourier(out Str2, out Str3);
        }

    }
}
