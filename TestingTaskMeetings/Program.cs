using System;

namespace Service
{
    public class Program
    {
        public ListService listMeet = new ListService();
        private enum Commads
        {
            AddMeeting = 1,
            DeleteMeeting = 2,
            ChangeMeeting = 3,
            PrintMeetings = 4,
            ExportMeetings = 5,
            CheckingMeeting = 6,
            Exit = 7
        }

        private static void ClientMenu()
        {
            Console.WriteLine("1 - Добавить встречу в список;");
            Console.WriteLine("2 - Удалить встречу из списка;");
            Console.WriteLine("3 - Изменить данные о встрече;");
            Console.WriteLine("4 - Вывести встречи на конкертную дату;");
            Console.WriteLine("5 - Эспорт встреч в txt файл;");
            Console.WriteLine("6 - Проверить ближайшие встречи;");
            Console.WriteLine("7 - Закрыть.");
        }

        static void Main(string[] args)
        {
            var listMeeting = new ListService();
            listMeeting.IsFile();
            listMeeting.Logger += OutputActionValue.WriteLog;
            listMeeting.OnAction += (message) => Console.WriteLine(message);
            Commads command = 0;
            while (true)
            {
                ClientMenu();
                try
                {
                    command = (Commads)int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Неизвестная команда.");
                }

                switch (command)
                {
                    case Commads.AddMeeting:
                        {
                            Console.Write("Введите название встречи: ");
                            var nameMeeting = Console.ReadLine();
                            Console.Write("Введите дату встречи в формате dd.mm.yyyy hh:mm ");
                            var dateStartMeeting = Console.ReadLine();
                            if (listMeeting.CheckActuallDate(dateStartMeeting))
                            {
                                Console.Write("Введите продолжительность встречи в минутах: ");
                                double durationOfTheMeeting = 0.0;
                                try
                                {
                                    durationOfTheMeeting = double.Parse(Console.ReadLine());
                                }
                                catch
                                {
                                    Console.WriteLine("Продолжительность времени должна быть числом!");
                                    break;
                                }
                                Console.Write("За сколько минут до начала встречи вас необходимо уведомить: ");
                                int timer = -1;
                                try
                                {
                                    timer = int.Parse(Console.ReadLine());
                                }
                                catch
                                {
                                    Console.WriteLine("Время до уведомления должно быть числом!");
                                }
                                finally
                                {
                                    listMeeting.AddToList(new Meeting(nameMeeting, dateStartMeeting, DateTime.Parse(dateStartMeeting).AddMinutes(durationOfTheMeeting).ToString("g"), timer));
                                }
                            }
                            else
                            {
                                Console.WriteLine($"{nameMeeting} нельзя добавить т.к. дата должна быть актуальной.");
                            }
                            break;
                        }
                    case Commads.DeleteMeeting:
                        {
                            Console.WriteLine("Введите название встречи, которую бы хотели удалить: ");
                            var deleteName = Console.ReadLine();
                            listMeeting.DeleteFromList(deleteName);
                            break;
                        }
                    case Commads.ChangeMeeting:
                        {
                            Console.Write("Введите название встречи, которую хотели бы изменить: ");
                            var changeName = Console.ReadLine();
                            if (listMeeting.CheckMeeting(changeName))
                            {
                                Console.WriteLine("Введите обновленную инофрмацию о встрече:");
                                Console.Write("Введите новое название встречи: ");
                                var nameMeeting = Console.ReadLine();
                                Console.Write("Введите новую дату встречи (в формате dd.MM.yyyy hh:mm): ");
                                var dateStartMeeting = Console.ReadLine();
                                Console.Write("Введите продолжительность встречи (мин.): ");
                                double durationOfTheMeeting = 0.0;
                                try
                                {
                                    durationOfTheMeeting = Double.Parse(Console.ReadLine());
                                }
                                catch
                                {
                                    Console.WriteLine("Продолжительность времени должна быть числом!");
                                    break;
                                }
                                var changeMeeting = new Meeting(nameMeeting, dateStartMeeting, DateTime.Parse(dateStartMeeting).AddMinutes(durationOfTheMeeting).ToString("g"), -1);
                                listMeeting.ChangeMeet(changeMeeting, changeName);
                                break;
                            }
                            Console.WriteLine("Встреча не найдена.");
                            break;
                        }
                    case Commads.PrintMeetings:
                        {
                            Console.WriteLine("Введите дату, за которую хотели бы вывести ваши встречи (dd.mm.yyyy): ");
                            var dateToPrintMeetings = Console.ReadLine();
                            listMeeting.PrintMeet(dateToPrintMeetings);
                            break;
                        }
                    case Commads.ExportMeetings:
                        {
                            Console.WriteLine("Введите дату, за которую хотели бы экспортировать ваши встречи (dd.mm.yyyy): ");
                            var dateToExportMeetings = Console.ReadLine();
                            listMeeting.Export(dateToExportMeetings);
                            break;
                        }
                    case Commads.CheckingMeeting:
                        {
                            foreach (Meeting meeting in listMeeting)
                                if (listMeeting.CheckTimeToNonification(meeting))
                                    Console.WriteLine($"У вас назначена встреча в {meeting.DateStart} {meeting.Name}");
                            break;
                        }
                    case Commads.Exit:
                        {
                            return;
                        }

                }
            }
        }
    }
}