using Task_Tracker;

class Program
{
    static void Main(string[] args)
    {
        var taskManager = new TaskManager();
        
        if (args.Length == 0)
        {
            Console.WriteLine("No command provided.");
            return;
        }

        try
        {
            taskManager.LoadTasks();
            var command = args[0].ToLower();
            switch (command)
            {
                case "add":
                    if (args.Length > 1)
                    {
                        TaskModel task = new TaskModel
                        {
                            Id = taskManager.GetId(),
                            Description = string.Join(" ", args[1..]),
                            Status = Status.ToDo.ToString()
                        };
                        taskManager.AddTask(task);
                        taskManager.SaveTasks();
                        Console.WriteLine("Task added successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Please provide a task description.");
                    }
                    break;

                case "update":
                    if (args.Length > 1 && int.TryParse(args[1], out int id))
                    {
                        taskManager.UpdateTask(id, string.Join(" ", args[2..])); // Use args[2..] for description
                        taskManager.SaveTasks();
                        Console.WriteLine("Task updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Please provide a valid task ID and description.");
                    }
                    break;

                case "list":
                    if (args.Length == 1)
                    {
                        taskManager.ListTasks();
                    }
                    else if (args.Length > 1 && Status.TryParse(args[1]) is Status status)
                    {
                        taskManager.ListTasks(status);
                    }
                    else
                    {
                        Console.WriteLine("Please provide a valid status (ToDo, InProgress, Done).");
                    }
                    break;

                case "mark-in-progress":
                    if (args.Length > 1 && int.TryParse(args[1], out int inProgressId))
                    {
                        taskManager.ChangeStatus(inProgressId, Status.InProgress);
                        taskManager.SaveTasks();
                        Console.WriteLine("Task marked as in progress.");
                    }
                    else
                    {
                        Console.WriteLine("Please provide a valid task ID.");
                    }
                    break;

                case "mark-done":
                    if (args.Length > 1 && int.TryParse(args[1], out int doneId))
                    {
                        taskManager.ChangeStatus(doneId, Status.Done);
                        taskManager.SaveTasks();
                        Console.WriteLine("Task marked as done.");
                    }
                    else
                    {
                        Console.WriteLine("Please provide a valid task ID.");
                    }
                    break;

                case "delete":
                    if (args.Length > 1)
                    {
                        if (int.TryParse(args[1], out int taskId))
                        {
                            taskManager.DeleteTask(taskId);
                            taskManager.SaveTasks();
                            Console.WriteLine("Task deleted successfully.");
                        }
                        else if (args[1].Contains(","))
                        {
                            string[] ids = args[1].Split(",");
                            List<int> taskIds = ids.Select(id => 
                            {
                                int.TryParse(id, out int parsedId);
                                return parsedId;
                            }).Where(id => id > 0).ToList();

                            if (taskIds.Count > 0)
                            {
                                taskManager.DeleteTasks(taskIds);
                                taskManager.SaveTasks();
                                Console.WriteLine("Tasks deleted successfully.");
                            }
                            else
                            {
                                Console.WriteLine("No valid task IDs provided for deletion.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Please provide a valid task ID or a comma-separated list of IDs.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please provide a task ID.");
                    }
                    break;
                case "clear":
                    if (args.Length == 1)
                    {
                        taskManager.ClearList();
                        taskManager.SaveTasks();
                        Console.WriteLine("All Tasks cleared successfully.");
                    }
                    else
                    {
                        Console.WriteLine("This Command Gets no Arguments");
                    }
                    break;
                case "help":
                {
                    Console.WriteLine(taskManager.HelpMessage());
                    break;
                }
                default:
                    Console.WriteLine("Unknown command. use help for the list of commands.");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
