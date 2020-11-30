using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class Form1 : Form
    {
        static private string THIS_PATH = AppDomain.CurrentDomain.BaseDirectory;    // путь к текущей директории
        Reports AllReport;                                                          // все отчёты за день
        private string NameRadioCheked = "apparat";                                 // имя выбранного параметра (по умолчанию аппаратные хар-ки)

        public Form1()
        {
            InitializeComponent();
        }

        private void radio_Apparat_CheckedChanged(object sender, EventArgs e)
        {
            NameRadioCheked = "apparat";
            RefreshListReport();

            textTimeStart.Enabled = false;
            textTimeEnd.Enabled = false;

            textTimeStart.Text = "";
            textTimeEnd.Text = "";
        }

        private void radio_Programs_CheckedChanged(object sender, EventArgs e)
        {
            NameRadioCheked = "programs";
            RefreshListReport();

            textTimeStart.Enabled = false;
            textTimeEnd.Enabled = false;

            textTimeStart.Text = "";
            textTimeEnd.Text = "";
        }

        private void radio_usb_CheckedChanged(object sender, EventArgs e)
        {
            NameRadioCheked = "usb";
            RefreshListReport();

            textTimeStart.Enabled = true;
            textTimeEnd.Enabled = true;
        }

        private void radio_PrintDoc_CheckedChanged(object sender, EventArgs e)
        {
            NameRadioCheked = "print";
            RefreshListReport();

            textTimeStart.Enabled = true;
            textTimeEnd.Enabled = true;
        }

        private void radio_Process_CheckedChanged(object sender, EventArgs e)
        {
            NameRadioCheked = "process";
            RefreshListReport();

            textTimeStart.Enabled = true;
            textTimeEnd.Enabled = true;
        }

        private void radio_Web_CheckedChanged(object sender, EventArgs e)
        {
            NameRadioCheked = "web";
            RefreshListReport();

            textTimeStart.Enabled = true;
            textTimeEnd.Enabled = true;
        }
        /* Кнопка "Получить информацию" */
        private void ShowInfo_Click(object sender, EventArgs e)
        {
            try
            {
                int port = Convert.ToInt32(textPort.Text);      // порт сервера
                string address = textIP.Text;                   // адрес сервера

                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipPoint);        // открываем соединение

                string TimeStart = textTimeStart.Text;      // время "с"
                string TimeEnd = textTimeEnd.Text;         // время "до"

                string message;            // сообщение для отправки клиенту

                message = NameRadioCheked + "|" + TimeStart + "|" + TimeEnd;    // формируем запрос

                byte[] data = Encoding.Unicode.GetBytes(message);       // конвертируем сообщение в байты
                socket.Send(data);                                      // отправляем сообщение

                // получаем ответ
                data = new byte[256]; // буфер для ответа
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт

                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (socket.Available > 0);

                string result = builder.ToString();
                SaveNewReport(result);
                textBoxResult.Text = result;

                // закрываем сокет
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();

                AllReport.IP = textIP.Text;
                AllReport.Port = textPort.Text;
                SaveJson();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось установить cоединение!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        /* Метод для обновления списка отчетов */
        private void RefreshListReport()
        {
            comboBox_ListReport.Items.Clear();
            comboBox_ListReport.Items.Add("Выберите время");
            comboBox_ListReport.SelectedItem = comboBox_ListReport.Items[0];

            foreach (report rep in AllReport.reports)
            {
                if (rep.Type == NameRadioCheked)
                {
                    string date = rep.Date.ToString("HH:mm:ss");
                    comboBox_ListReport.Items.Add(date);
                }
            }

        }

        private void SaveJson()
        {
            string json = JsonConvert.SerializeObject(AllReport);   // конвертируем объект в JSON
            json = json.Replace("[", "[\n");                        //_________________________________//
            json = json.Replace("]", "\n]");                        //_____Добавляем оступы в JSON_____//
            json = json.Replace("},", "},\n");                      //_________________________________//

            File.WriteAllText(THIS_PATH + "reports.json", json);    // перезаписываем JSON
        }

        /* Метод для сохранения нового отчета */
        private void SaveNewReport(string data)
        {
            report NewReport = new report();                    // создаём новый отчет
            NewReport.Date = DateTime.Now;                      // добавляем дату его получения
            NewReport.Type = NameRadioCheked;                   // его тип

            AllReport.reports.Add(NewReport);                   // добавляем его в список
            RefreshListReport();                                // обновляем список отчетов

            string path = THIS_PATH + NewReport.Type;           // путь к папке, в которой будет храниться отчет
            Directory.CreateDirectory(path);                    // создаём папку, если её еще нет

            string date = NewReport.Date.ToString("HH_mm_ss");  // извлекаем дату получения запроса
            path = path + "\\" + date;                          // полный путь к файлу с отчетом
            File.WriteAllText(path, data);                      // сохраняем отчет по указанному пути
            SaveJson();
        }
        /* Метод для обновления файла JSON */
        private void RefreshJSON()
        {
            try
            {
                string text = File.ReadAllText(THIS_PATH + "reports.json");     // извлекаем инфу из json
                AllReport = JsonConvert.DeserializeObject<Reports>(text);       // конвертируем её в объект 
            }
            catch
            {
                AllReport = new Reports();                           // если json не найден создаём новый объект
            }

            List<report> NewListRep = new List<report>();
            DateTime today = DateTime.Now;

            foreach (report rep in AllReport.reports)
            {
                string NameFile = rep.Date.ToString("HH_mm_ss");
                string PathFile = THIS_PATH + rep.Type + "//" + NameFile;

                if (File.Exists(PathFile))
                {
                    if ((rep.Date.Year == today.Year) && (rep.Date.Month == today.Month) && (rep.Date.Day == today.Day))
                    {
                        NewListRep.Add(rep);
                    }
                    else
                    {
                        File.Delete(PathFile);
                    }
                }
            }
            AllReport.reports = NewListRep;
            SaveJson();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshJSON();          // обновляем json
            RefreshListReport();    // обновляем список отчетов в combobox

            textIP.Text = AllReport.IP;
            textPort.Text = AllReport.Port;
        }
        /* Событие при изменении выбранного отчета в comboBox */
        private void comboBox_ListReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_ListReport.SelectedIndex != 0)
            {
                string NameFile = comboBox_ListReport.SelectedItem.ToString().Replace(':', '_');
                string path = THIS_PATH + NameRadioCheked + "//" + NameFile;
                string result = File.ReadAllText(path);
                textBoxResult.Text = result;
            }
            else
            {
                textBoxResult.Text = "";
            }
        }


    }
}
