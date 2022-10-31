using System.Collections;
using System.IO;
using Newtonsoft.Json;

namespace Service
{
    public class ListService : IEnumerable
    {
        public List<Meeting> Meetings = new List<Meeting>();
        public delegate void ListServiceHandler(string message);
        public ListServiceHandler OnAction;
        public ListServiceHandler Logger;
        string message = string.Empty;
        string errorMessage = string.Empty;

        /// <summary>
        /// Получить путь к файлу со встречами.
        /// </summary>
        /// <returns>Путь к файлу, в котором хранятся все встречи.</returns>
        private string FileNameDataSet() => Path.GetFullPath($"MeetsDataSet.json");

        /// <summary>
        /// Получить путь к файлу со встречами за дату.
        /// </summary>
        /// <returns>Путь к файлу, в котором хранятся встречи за какую-то дату.</returns>
        private string FileNameToExport() => Path.GetFullPath($"Meets.txt");

        /// <summary>
        /// Отправить сообщение на экран и лог файл.
        /// </summary>
        /// <param name="message">Сообщение, которое отправляется.</param>
        private void SendMessageInLogsAndToUser(string message)
        {
            OnAction?.Invoke(message);
            Logger?.Invoke(message);
        }

        /// <summary>
        /// Добавить встречу в список.
        /// </summary>
        /// <param name="meeting">Встреча.</param>
        public virtual void AddToList(Meeting meeting)
        {
            if (CheckDateMeeting(meeting.DateStart))
            {
                errorMessage = $"{meeting.Name} нельзя добавить т.к. занято время ";
                SendMessageInLogsAndToUser(errorMessage);
            }
            else
            {
                Meetings.Add(meeting);
                SaveMeet();
                message = $"Встреча: {meeting.Name} добавлена.";
                SendMessageInLogsAndToUser(message);
            }

        }

        public bool CheckActuallDate(string date)
        {
            return DateTime.Parse(date) >= DateTime.Now.AddMinutes(-2);
        }

        /// <summary>
        /// Удалить встречу из списка.
        /// </summary>
        /// <param name="name">Название встречи.</param>
        public virtual void DeleteFromList(string name)
        {
            var deleteMeet = this.Meetings.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
            if (deleteMeet != null)
            {
                this.Meetings.Remove(deleteMeet);
                message = $"{name} удалена из списка.";
                SendMessageInLogsAndToUser(message);
                this.SaveMeet();
                return;
            }

            errorMessage = $"{name} в нет в списке.";
            OnAction?.Invoke(errorMessage);
        }

        /// <summary>
        /// Изменить данные о встрече.
        /// </summary>
        /// <param name="meet">Встреча.</param>
        public void ChangeMeet(Meeting meeting, string oldMeeting)
        {
            var changeMeeting = Meetings.Where(x => x.Name == oldMeeting).FirstOrDefault();
            Console.WriteLine(meeting);
            changeMeeting.Name = meeting.Name;
            changeMeeting.DateStart = meeting.DateStart;
            changeMeeting.DateEnd = meeting.DateEnd;
            SendMessageInLogsAndToUser(message);
            SaveMeet();
        }

        /// <summary>
        /// Проверить наличие встречи в списке.
        /// </summary>
        /// <param name="name">Название встречи.</param>
        /// <returns>True - дело в списке есть, иначе false.</returns>
        public bool CheckMeeting(string name)
        {
            return Meetings.Any(x => x.Name == name);
        }

        /// <summary>
        /// Проверить дату встречи.
        /// </summary>
        /// <param name="dateTimeStart">Дата встречи.</param>
        /// <returns>True - если на эту дату нет встречи, иначе false</returns>
        public bool CheckDateMeeting (string dateTimeStart)
        {
            foreach (var meeting in Meetings)
            {
                var dateStart = DateTime.Parse(meeting.DateStart);
                var dateEnd = DateTime.Parse(meeting.DateEnd);
                if (dateStart < DateTime.Parse(dateTimeStart) && dateEnd > DateTime.Parse(dateTimeStart))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Вывести встречи за конкретный период на экран.
        /// </summary>
        /// <param name="dateTimeStart">Дата встречи.</param>
        public void PrintMeet(string dateTimeStart)
        {
            var meetings = Meetings.Where(x => DateTime.Parse(DateTime.Parse(x.DateStart).ToString("d")) == DateTime.Parse(dateTimeStart));
            foreach (var meeting in meetings)
            {
                Console.WriteLine(meeting);
            }
        }


        /// <summary>
        /// Сохранить встречу в файл.
        /// </summary>
        private void SaveMeet()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.TypeNameHandling = TypeNameHandling.Auto;
            serializer.Formatting = Formatting.Indented;
            using (StreamWriter sw = new StreamWriter(FileNameDataSet()))
            using (JsonWriter writer = new JsonTextWriter(sw))
                serializer.Serialize(writer, Meetings, typeof(Meeting));
        }

        /// <summary>
        /// Загрузить файл со встречами.
        /// </summary>
        private void Load()
        {
            var json = File.ReadAllText(FileNameDataSet());
            var loeadedList = JsonConvert.DeserializeObject<List<Meeting>>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
            });
            if (loeadedList != null)
                this.Meetings.AddRange(loeadedList);
        }

        /// <summary>
        /// Экспортировать файл в txt файл за конкретную дату.
        /// </summary>
        /// <param name="date">Дата, за которую необходимо экспортировать встречи вфайл.</param>
        public void Export(string date)
        {
            if (File.Exists(FileNameDataSet()))
            {
                using (StreamWriter writer = new StreamWriter(FileNameToExport(), true))
                {
                    var meetings = Meetings.Where(x => DateTime.Parse(DateTime.Parse(x.DateStart).ToString("d")) == DateTime.Parse(date));                   
                    foreach (var meet in meetings)
                        writer.WriteLine(meet);
                }
            }
            else Console.WriteLine("Файл не найден");
        }

       
        /// <summary>
        /// Проверить времени до начала встречи.
        /// </summary>
        /// <param name="meeting">Встреча</param>
        /// <returns>True - Время до начала равно времени сейчас + время до уведомления, иначе false.</returns>
        public bool CheckTimeToNonification(Meeting meeting)
        {
            return (DateTime.Parse(meeting.DateStart) <= DateTime.Now.AddMinutes(meeting.Timer));
        }
        /// <summary>
        /// Проверить наличие файла по пути.
        /// </summary>
        public void IsFile()
        {
            if (File.Exists(FileNameDataSet())) this.Load();
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var meeting in Meetings)
                yield return meeting;
        }
    }
}
