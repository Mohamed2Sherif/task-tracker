using System.Text.Json;

namespace Task_Tracker
{
    public class TaskManager
    {
        private List<TaskModel> _tasks = new List<TaskModel>();
        private int LastAddedId => _tasks.Count > 0 ? _tasks.Last().Id + 1 : 1;
        private readonly string filePath = "Tasks.json";

        public void LoadTasks()
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "[]");
            }

            using (StreamReader fileReader = new StreamReader(filePath))
            {
                var json = fileReader.ReadToEnd();
                _tasks = JsonSerializer.Deserialize<List<TaskModel>>(json) ?? new List<TaskModel>();
            }
        }

        public int GetId()
        {
            return this.LastAddedId;
        }

        public void SaveTasks()
        {
            var tasks_string = JsonSerializer.Serialize(_tasks);
            File.WriteAllText(filePath, $"{tasks_string}");
        }

        public void AddTask(TaskModel task)
        {
            _tasks.Add(task);
        }

        public void AddTask(List<TaskModel> tasks)
        {
            foreach (TaskModel task in tasks)
            {
                _tasks.Add(task);
            }
        }

        public void DeleteTask(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            _tasks.Remove(task);
        }

        public void DeleteTasks(List<int> ids)
        {
            _tasks.RemoveAll(t => ids.Contains(t.Id));
        }

        public void UpdateTask(int id, string description)
        {
            _tasks.FirstOrDefault(t => t.Id == id).Description = description;
        }

        public void ChangeStatus(int id, Status status)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id).Status = status.ToString();
        }

        public void ListTasks()
        {
            foreach (var task in _tasks)
            {
                ChangeConsoleColor(task.Status);
                Console.WriteLine(task);
                Console.ResetColor();
            }
        }

        public void ListTasks(Status status)
        {
            var tasks = _tasks.Where(t => t.Status == status.ToString()).ToList();
            ChangeConsoleColor(status.ToString());
            foreach (var task in tasks)
            {
                Console.WriteLine(task);
            }

            Console.ResetColor();
        }

        public void ClearList()
        {
            _tasks.Clear();
        }

        public string HelpMessage()
        {
            return @"
Usage: myapp [command] [options]
Commands:
  add ""Task description""
  update <ID> ""New task description""
  delete <ID>
  mark-in-progress <ID>
  mark-done <ID>
  list [status]
";
        }

        private void ChangeConsoleColor(string status)
        {
            if (status == Status.Done.ToString())
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }

            else if (status == Status.ToDo.ToString())
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else if (status == Status.InProgress.ToString())
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            }
        }
    }
}