using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace CourierService
{
    public partial class Form1 : Form
    {
        string S1="",S2="",S3="",S4="";
        


        public Form1()
        {
            InitializeComponent();
            label4.Text = "Промежуток времени между появлением\nзаявок в незагруженные часы:";
            label5.Text = "Промежуток времени между появлением\nзаявок в загруженные часы:";
            label15.Text = "          Получение посылок в филиале \nи выезд за посылками с другие филиалы:";
        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            int Idling = 0;
            int SlowPackage = 0;
            int TimeSlowPackage = 0;
            int PackageCount = 0;

            S1 = "";
            S2 = "";
            S3 = "";
            S4 = "";

            

            for (int i = 1; i < Convert.ToInt32(numberOfDays.Value) + 1; i++)
            {
                
                S1 += "День " + i.ToString() + "\r\n\r\n";
                S2 += "День " + i.ToString() + "\r\n\r\n";
                S3 += "День " + i.ToString() + "\r\n\r\n";
                string s1, s2, s3;
                try
                {
                    Modeling _Modeling = new Modeling(Convert.ToInt32(numericUpDown2.Value), Convert.ToInt32(numericUpDown1.Value), Convert.ToInt32(MidTimeMove.Value));
                    
                    _Modeling.WorkingDay(out s1, out s2, out s3, Convert.ToInt32(MinTimePackageN.Value), Convert.ToInt32(MaxTimePackageN.Value), Convert.ToInt32(MinTimePackageY.Value), Convert.ToInt32(MaxTimePackageY.Value), Convert.ToInt32(MinUrgency.Value), Convert.ToInt32(MaxUrgency.Value));
                   
                    S1 += s1;
                    S2 += s2;
                    S3 += s3;

                    Idling += _Modeling._Idling;
                    SlowPackage += _Modeling.SlowPackage;
                    TimeSlowPackage += _Modeling.TimeSlowPackage;
                    PackageCount += _Modeling.NumberOfPackages;

                    S1 += "\r\n\r\n";
                    S2 += "\r\n\r\n";
                    S3 += "\r\n\r\n";
                }
                catch (ArgumentOutOfRangeException)
                {
                    S1 = "";
                    S2 = "";
                    S3 = "";
                    S4 = "";
                    MessageBox.Show("Минимальное значение превышает максимальное", "Ошибка", MessageBoxButtons.OK);
                }
            }

            S4 += "Общее время холостых переездов: " + Idling.ToString() + " мин\n";
            S4 += "Количество заявок: " + PackageCount.ToString() + "\n";
            S4 += "Количество опоздавших посылок: " + SlowPackage.ToString() + "\n";
            S4 += "Общее время задержки посылок: " + TimeSlowPackage.ToString() + " мин\n";

            richTextBox1.Text = S1;
            richTextBox2.Text = S2;
            richTextBox3.Text = S3;
            richTextBox4.Text = S4;

        }
                
                

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.InitialDirectory = "c:\\";
                sfd.Filter = "Текстовый файл (*.txt)|*.txt";
                sfd.FilterIndex = 1;

                sfd.FileName = "Заявки";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter file = new StreamWriter(sfd.OpenFile());
                    file.Write(S1);
                    file.Close();
                }

                sfd.FileName = "Получение";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter file = new StreamWriter(sfd.OpenFile());
                    file.Write(S2);
                    file.Close();
                }

                sfd.FileName = "Доставка";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter file = new StreamWriter(sfd.OpenFile());
                    file.Write(S3);
                    file.Close();
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Ошибка сохранения", "Ошибка", MessageBoxButtons.OK);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("Справка.txt");
            }
            catch (System.ComponentModel.Win32Exception)
            {
                MessageBox.Show("Справка не найдена", "Ошибка", MessageBoxButtons.OK);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Справка не найдена", "Ошибка", MessageBoxButtons.OK);
            }
        }

    }
}
