using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Agent
{
    class Program
    {
        static private string THIS_PATH = AppDomain.CurrentDomain.BaseDirectory;   // путь к текущей директории
        static private Info information;                                           // вся инфа
        static private Thread mythread;                                     // поток для мониторинга
        static private Thread WebThread;                                    // клиент-серверный поток

        static int port = 8005;
        static IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
        static Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        static private List<string> ListUSB = new List<string>();       // Список флешек

        static void Main(string[] args)
        {
            mythread = new Thread(ChekInWhile);     // связываем поток мониторинга с функцией мониторинга
            mythread.Start();                       // запускаем его

            WebThread = new Thread(ClientServer);   // связываем поток сетевого взаимодействия с функцией
            WebThread.Start();                      // запускаем его
        }

        /* Метод для удаления старых процессов */
        static private List<process> DeleteOldProcesses(List<process> AllProc)
        {
            List<process> NewProc = new List<process>();

            foreach (process p in AllProc)
            {
                string today = DateTime.Now.ToString("dd.MM.yyyy");
                if (p.Date == today)
                {
                    NewProc.Add(p);
                }
            }
            return NewProc;
        }
        /* Метод обновления списка процессов */
        static private List<process> UpdateListProcess(List<process> ListProcInJson, Process[] ListCurrentProc)
        {
            //__________Помечаем завершившиеся процессы__________\\

            foreach (process ProcInJson in ListProcInJson)                      // пробегаем каждый процесс из JSON
            {
                bool repeat = false;
                if (ProcInJson.TimeEnd == null)
                {
                    foreach (Process CurrentProc in ListCurrentProc)                // пробегаем каждый запущенный процесс
                    {
                        if (ProcInJson.Name == CurrentProc.ProcessName)             // если процесс из JSON есть среди запущенных
                        {
                            repeat = true;                                          // всё ок
                            break;
                        }
                    }
                    if (!repeat)                                                    // если процесса из JSON нет среди запущенных
                    {
                        ProcInJson.TimeEnd = DateTime.Now.ToString("HH:mm");     // записываем время отключения процесса
                    }
                }
            }
            //__________Добавляем новые процессы в список__________\\

            foreach (Process CurrentProc in ListCurrentProc)    // пробегаем каждый запущенный процесс 
            {
                bool repeat = false;                            // флаг повторения
                foreach (process p in ListProcInJson)           // пробегаем все процессы из списка
                {
                    if ((CurrentProc.ProcessName == p.Name) && (p.TimeEnd == null))         // если данный процесс есть в списке и он активен
                    {
                        repeat = true;
                        break;
                    }
                }
                if (!repeat)                                // если такого процесса не было в списке
                {
                    process newProc = new process();
                    newProc.Name = CurrentProc.ProcessName;                     // записываем имя процесса
                    newProc.TimeStart = DateTime.Now.ToString("HH:mm");      // время запуска
                    newProc.Date = DateTime.Now.ToString("dd.MM.yyyy");         // дату запуска
                    ListProcInJson.Add(newProc);                                // добавляем процесс в список
                }
            }
            return ListProcInJson;
        }

        /* Метод для добавления документа в список  */
        static private List<PrintDoc> GetPrintedDocumentName(List<PrintDoc> documents)
        {
            List<PrintDoc> result = new List<PrintDoc>();
            DateTime today = DateTime.Now;

            foreach (PrintDoc doc in documents)
            {
                DateTime time = doc.Date_Time;      // время из JSON

                if ((time.Year == today.Year) && (time.Month == today.Month) && (time.Day == today.Day))        // если cегодня
                {
                    result.Add(doc);
                }
            }

            using (ManagementObjectSearcher mos = new ManagementObjectSearcher("Select * from Win32_PrintJob"))
            {
                string NameDoc = "";
                try
                {
                    foreach (ManagementObject mo in mos.Get())
                    {
                        NameDoc = mo["Document"].ToString();
                        break;
                    }
                }
                catch (InvalidCastException e)
                {

                }
                if (NameDoc != "")
                {
                    PrintDoc NewDocument = new PrintDoc();

                    NewDocument.Date_Time = today;
                    NewDocument.Program = NameDoc.Split('-')[0].Trim();
                    NewDocument.Name = NameDoc.Split('-')[1].Trim();

                    result.Add(NewDocument);
                }
            }

            return result;
        }
        /* Метод для определения подключения флешки */
        static private UsbDriver ChekUSB()
        {
            List<string> Namelist = new List<string>();         // список имён флешек 
            DriveInfo[] D = DriveInfo.GetDrives();              // объект для работы с usb
            UsbDriver result = null;

            foreach (DriveInfo DI in D)                         // перебераем все носители
            {
                if (DI.DriveType == DriveType.Removable)        // если носитель является флешкой
                {
                    Namelist.Add(DI.Name + DI.VolumeLabel);     // добавляем его в список
                }
            }

            if (ListUSB.Count < Namelist.Count)
            {
                for (int i = 0; i < Namelist.Count; i++)
                {
                    bool flag = false;
                    for (int j = 0; j < ListUSB.Count; j++)
                    {
                        if (Namelist[i] == ListUSB[j])
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        result = new UsbDriver();
                        result.Date_Time = DateTime.Now;
                        result.Name = Namelist[i];
                    }
                }
            }
            ListUSB = Namelist;
            return result;
        }

        /* Метод для удаления прошлодневных записей о USB */
        static private List<UsbDriver> DeleteOldUSB(List<UsbDriver> AllUSB)
        {
            List<UsbDriver> result = new List<UsbDriver>();
            DateTime today = DateTime.Now;

            foreach (UsbDriver usb in AllUSB)
            {
                DateTime date = usb.Date_Time;
                if ((date.Year == today.Year) && (date.Month == today.Month) && (date.Day == today.Day))
                {
                    result.Add(usb);
                }
            }
            return result;
        }


        /* МЕТОД ПОСТОЯННОГО МОНИТОРИНГА */
        static private void ChekInWhile()
        {
            List<process> processes;       // список процессов

            while (true)
            {
                try
                {
                    string text = File.ReadAllText(THIS_PATH + "info.json");    // извлекаем инфу из json
                    information = JsonConvert.DeserializeObject<Info>(text);    // конвертируем её в объект 
                }
                catch
                {
                    information = new Info();                           // если json не найден создаём новый объект
                }

                //____________ПРОЦЕССЫ_____________\\

                processes = information.processes;                      // извлекаем из объекта список процессов 
                processes = DeleteOldProcesses(processes);              // удаляем все процессы, кроме сегодняшних  

                Process[] procList = Process.GetProcesses();            // получаем текущие запущенные процессы
                processes = UpdateListProcess(processes, procList);     // Обновляем список процессов 
                information.processes = processes;                      // обновляем записи о процессах в объекте

                //____________ДОКУМЕНТЫ_____________\\

                List<PrintDoc> documents = information.PrintDoc;        // получаем список распечатанных документов
                documents = GetPrintedDocumentName(documents);          // добавляем распечатанный документ в список
                information.PrintDoc = documents;

                //____________ФЛЕШКИ_____________\\

                UsbDriver NewUSB = ChekUSB();                           // проверяем есть ли новая вставленная флешка
                if (NewUSB != null)                                     // если есть
                {
                    information.usb.Add(NewUSB);                        // добавляем её в список
                }
                information.usb = DeleteOldUSB(information.usb);        // удаляем все прошлодневные записи

                //___________ЗАПИСЬ JSON___________\\

                string json = JsonConvert.SerializeObject(information); // конвертируем объект в JSON
                json = json.Replace("[", "[\n");                        //_________________________________//
                json = json.Replace("]", "\n]");                        //_____Добавляем оступы в JSON_____//
                json = json.Replace("},", "},\n");                      //_________________________________//

                File.WriteAllText(THIS_PATH + "info.json", json);       // перезаписываем JSON
                Thread.Sleep(500);
            }
        }

        /* Метод для получения списка процессов в указанный временной промежуток */
        static private string GetProcesses(string TimeStart, string TimeEnd)
        {
            List<process> ListProc = new List<process>();

            foreach (process p in information.processes)
            {
                DateTime StartProc = StringToTime(p.TimeStart);     // время старта процесса
                DateTime EndProc;                                   // время завершения процесса
                if (p.TimeEnd != null)          // если время завершения процесса не нуль                      
                {
                    EndProc = StringToTime(p.TimeEnd);
                }
                else
                {
                    EndProc = StringToTime("23:59:59");
                }
                DateTime time_start = StringToTime(TimeStart);      // время старта из запроса
                DateTime time_end = StringToTime(TimeEnd);          // время конца из запроса

                if (ChekTime(StartProc, "<", time_end) && (ChekTime(EndProc, ">", time_start)))        // если процесс попадает во временной промежуток
                {
                    ListProc.Add(p);                            // добавляем его в список
                }
            }

            //_________ Формирование строки вывода ____________\\
            string str_Processes = "Количество процессов: " + ListProc.Count() + "\r\n\r\n";

            foreach (process p in ListProc)
            {
                str_Processes += p.TimeStart + " | ";
                if (p.TimeEnd == null)
                {
                    str_Processes += "........." + " | " + p.Name + "\r\n";
                }
                else
                {
                    str_Processes += p.TimeEnd + " | " + p.Name + "\r\n";
                }
            }

            return str_Processes;
        }

        /* Меотд для получения информации о конкретном параметре аппаратного обеспечения */
        static private string GetHardwareInfo(string WIN32_Class, string ClassItemField)
        {
            string result = "";

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM " + WIN32_Class);

            try
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    result += obj[ClassItemField].ToString().Trim() + '\r' + '\n';
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }
        /* Метод для получения полной инфы об аппаратном обеспечении */
        static private string GetAllHardwareInfo()
        {
            string AllHardwareInfo = "";
            // Всё о ЦП
            string WIN32_Class = "Win32_Processor";

            AllHardwareInfo += "Процессор:\r\n" + GetHardwareInfo(WIN32_Class, "Name");
            AllHardwareInfo += "Производитель:\r\n" + GetHardwareInfo(WIN32_Class, "Manufacturer");
            AllHardwareInfo += "Описание:\r\n" + GetHardwareInfo(WIN32_Class, "Description") + "\r\n";

            // Всё о Видеокарте
            WIN32_Class = "Win32_VideoController";

            AllHardwareInfo += "Видеокарта:\r\n" + GetHardwareInfo(WIN32_Class, "Name");
            AllHardwareInfo += "Видеопроцессор:\r\n" + GetHardwareInfo(WIN32_Class, "VideoProcessor");
            AllHardwareInfo += "Версия драйвера:\r\n" + GetHardwareInfo(WIN32_Class, "DriverVersion");
            AllHardwareInfo += "Объем памяти (в байтах):\r\n" + GetHardwareInfo(WIN32_Class, "AdapterRAM") + "\r\n";

            // Всё о жёстких дисках
            WIN32_Class = "Win32_DiskDrive";

            AllHardwareInfo += "Жесткие диски:\r\n";
            string[] NameDrive = GetHardwareInfo(WIN32_Class, "Caption").Trim().Split('\n');
            string[] SizeDrive = GetHardwareInfo(WIN32_Class, "Size").Trim().Split('\n');
            for (int i = 0; i < NameDrive.Length; i++)
            {
                AllHardwareInfo += "\r\n" + NameDrive[i] + "\r\n" + "Объём (в байтах): " + SizeDrive[i] + "\r\n";
            }

            return AllHardwareInfo;
        }
        /* Метод для получения списка установленного ПО */
        static private string GetInstallPrograms()
        {
            string Programs = "";

            string SoftwareKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(SoftwareKey))
            {
                foreach (string keyName in key.GetSubKeyNames())
                {
                    RegistryKey subkey = key.OpenSubKey(keyName);

                    if (subkey.GetValue("DisplayName") != null)
                    {
                        Programs += subkey.GetValue("DisplayName") + "\r\n";
                    }
                }

            }
            return Programs;
        }

        static private string SearshDataBase(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);

            foreach (var item in dir.GetFiles())    // перебираем все файлы
            {
                if (item.Name == "places.sqlite")
                {
                    return path + "\\places.sqlite";
                }
                //MessageBox.Show(item.Name);
            }
            string NewPath = "";
            foreach (var item in dir.GetDirectories())
            {
                NewPath = SearshDataBase(path + "\\" + item);
            }
            return NewPath;
        }
        /* Метод для конвертации строки формата 'yyyy-MM-dd HH:mm:ss' в DateTime */
        static private DateTime StringToDateTime(string StrTime)
        {
            if (StrTime != "")
            {
                string[] Date_and_Time = StrTime.Split(' ');
                string[] Date = Date_and_Time[0].Split('-');
                string[] Time = Date_and_Time[1].Split(':');

                List<int> IntTime = new List<int>();
                foreach (string d in Date)
                {
                    IntTime.Add(Convert.ToInt32(d));
                }
                foreach (string t in Time)
                {
                    IntTime.Add(Convert.ToInt32(t));
                }
                DateTime result = new DateTime(IntTime[0], IntTime[1], IntTime[2], IntTime[3], IntTime[4], IntTime[5]);
                return result;
            }
            return new DateTime();
        }
        /* Метод для конвертации строки формата 'HH:MM:SS' в DateTime */
        static private DateTime StringToTime(string StrTime)
        {
            string[] Time;
            if (StrTime == "")
            {
                Time = new string[3] { "0", "0", "0" };
            }
            else
            {
                Time = StrTime.Trim().Split(':');
            }
            List<int> intTime = new List<int>();

            for (int i = 0; i < 3; i++)
            {
                if (i < Time.Length)
                {
                    intTime.Add(Convert.ToInt32(Time[i]));
                }
                else
                {
                    intTime.Add(0);
                }
            }

            int[] today = new int[3] { DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day };

            return new DateTime(today[0], today[1], today[2], intTime[0], intTime[1], intTime[2]);
        }
        /* Метод для проверки времени */
        static private bool ChekTime(DateTime TimeDB, string sign, DateTime TimeRequest)
        {
            if (sign == ">=" || sign == ">")
            {
                if (TimeDB.Hour > TimeRequest.Hour)
                {
                    return true;
                }
                else if (TimeDB.Hour == TimeRequest.Hour)
                {
                    if (TimeDB.Minute > TimeRequest.Minute)
                    {
                        return true;
                    }
                    else if (TimeDB.Minute == TimeRequest.Minute)
                    {
                        if (sign == ">=")
                        {
                            if (TimeDB.Second >= TimeRequest.Second)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (TimeDB.Second > TimeRequest.Second)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            else if (sign == "<=" || sign == "<")
            {
                if (TimeDB.Hour < TimeRequest.Hour)
                {
                    return true;
                }
                else if (TimeDB.Hour == TimeRequest.Hour)
                {
                    if (TimeDB.Minute < TimeRequest.Minute)
                    {
                        return true;
                    }
                    else if (TimeDB.Minute == TimeRequest.Minute)
                    {
                        if (sign == "<=")
                        {
                            if (TimeDB.Second <= TimeRequest.Second)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (TimeDB.Second < TimeRequest.Second)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        /* Метод для получения истории браузера */
        static private string GetHistoryWeb(string TimeStart, string TimeEnd)
        {
            string ThisUser = Environment.UserName;
            string Path = "C:\\Users\\" + ThisUser + "\\AppData\\Roaming\\Mozilla\\Firefox\\Profiles";
            Path = SearshDataBase(Path);        // получаем путь к базе

            SQLiteConnection m_dbConn = new SQLiteConnection("Data Source=" + Path + ";Version=3;");
            m_dbConn.Open();
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = m_dbConn;
            cmd.CommandText = "select url,last_visit_date as raw_visit_date,datetime(last_visit_date/1000000,'unixepoch','localtime') as last_visit_date from moz_places";
            SQLiteDataReader r = cmd.ExecuteReader();
            string result = "";

            while (r.Read())
            {
                DateTime time = StringToDateTime(r["last_visit_date"].ToString());      // дата из базы
                DateTime today = DateTime.Now;                                          // сегодняшний день

                if ((time.Day == today.Day) && (time.Month == today.Month) && (time.Year == today.Year))
                {
                    DateTime time_start = StringToTime(TimeStart);
                    DateTime time_end = StringToTime(TimeEnd);

                    if (ChekTime(time, ">=", time_start) && ChekTime(time, "<", time_end))
                    {
                        result += time.ToString("HH:mm:ss") + " | ";
                        result += r["url"] + "\r\n";
                    }
                }
            }
            return result;
        }

        static private string GetPrintDoc(string TimeStart, string TimeEnd)
        {
            string result = "";

            DateTime time_start = StringToTime(TimeStart);
            DateTime time_end = StringToTime(TimeEnd);

            List<PrintDoc> ListDoc = information.PrintDoc;
            foreach (PrintDoc doc in ListDoc)
            {
                DateTime time = doc.Date_Time;
                if (ChekTime(time, ">=", time_start) && ChekTime(time, "<", time_end))
                {
                    result += doc.Program + " | ";
                    result += doc.Name + " | ";
                    result += time.ToString("HH:mm:ss") + "\r\n";
                }
            }
            return result;
        }
        /* Метод для получения списка флешек за указанный период */
        static private string GetUSB(string TimeStart, string TimeEnd)
        {
            string result = "";

            DateTime time_start = StringToTime(TimeStart);
            DateTime time_end = StringToTime(TimeEnd);

            List<UsbDriver> ListUSB = information.usb;
            foreach (UsbDriver usb in ListUSB)
            {
                DateTime time = usb.Date_Time;
                if (ChekTime(time, ">=", time_start) && ChekTime(time, "<", time_end))
                {
                    result += time.ToString("HH:mm:ss") + "  ";
                    result += usb.Name + "\r\n";
                }
            }
            return result;
        }

        /* метод для сетевого взаимодействия */
        static private void ClientServer()
        {
            // связываем сокет с локальной точкой, по которой будем принимать данные
            listenSocket.Bind(ipPoint);

            // начинаем прослушивание
            listenSocket.Listen(10);

            Console.WriteLine("Служба мониторинга запущена. Ожидаются запросы от сервера...\n");

            while (true)
            {
                Socket handler = listenSocket.Accept();
                StringBuilder builder = new StringBuilder();

                byte[] data = new byte[256];        // буфер для получаемых данных

                do
                {
                    int bytes = handler.Receive(data);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (handler.Available > 0);

                string request = builder.ToString();        // конвертируем запрос от сервера в строку
                string[] list_param = request.Split('|');

                string TimeStart = list_param[1];       // время "с" из запроса
                string TimeEnd = list_param[2];         // время "до" из запроса

                string TypeInfo = list_param[0];        // информация, которая требуется в запросе
                string message;                         // сообщение для ответа серверу

                string OutputCMD;                       // сообщение для вывода в консоль

                if (TypeInfo == "process")
                {
                    message = GetProcesses(TimeStart, TimeEnd);
                    OutputCMD = "Была запрошена информация о запущенных процессах в период с " + TimeStart + " до " + TimeEnd;
                }
                else if (TypeInfo == "apparat")
                {
                    message = GetAllHardwareInfo();
                    OutputCMD = "Была запрошена информация об аппаратных характеристиках ПК";
                }
                else if (TypeInfo == "programs")
                {
                    message = GetInstallPrograms();
                    OutputCMD = "Была запрошена информация об установленном ПО";
                }
                else if (TypeInfo == "web")
                {
                    message = GetHistoryWeb(TimeStart, TimeEnd);
                    OutputCMD = "Была запрошена информация об открываемых сайтах в период с " + TimeStart + " до " + TimeEnd;
                }
                else if (TypeInfo == "print")
                {
                    message = GetPrintDoc(TimeStart, TimeEnd);
                    OutputCMD = "Была запрошена информация о распечатанных документах в период с " + TimeStart + " до " + TimeEnd;
                }
                else if (TypeInfo == "usb")
                {
                    message = GetUSB(TimeStart, TimeEnd);
                    OutputCMD = "Была запрошена информация о подключенных usb ноителях в период с " + TimeStart + " до " + TimeEnd;
                }
                else
                {
                    message = "Был отправлен некорректный запрос. Повторите попытку...";
                    OutputCMD = "Был получен некорректный запрос";
                }

                data = Encoding.Unicode.GetBytes(message);
                handler.Send(data);
                // закрываем сокет
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

                Console.WriteLine(OutputCMD);
            }
        }
    }
}
