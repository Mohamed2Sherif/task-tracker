

namespace Task_Tracker
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Status{ get; set; }

        public override string ToString()
        {
            return $"Title: {Description}\nStatus: {Status}\nId: {Id}\n";
        }
    }

    public class Status
    {
        public static readonly  Status InProgress = new Status("InProgress");
        public  static  readonly Status Done = new Status("Done");
        public  static readonly Status ToDo = new Status("ToDo");

        public string Name { get; set; }
        private  Status(string Name)
        {
            this.Name = Name;
        }

        public static Status TryParse(string status)
        {
            // Get the type of the Status class
            var statusType = typeof(Status);
        
            // Use reflection to get all public static fields of the Status class
            var fields = statusType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.Instance);
        
            foreach (var field in fields)
            {
                // Check if the field is of type Status and compare the Name property
                if (field.GetValue(null) is Status statusInstance && statusInstance.ToString().Equals(status, StringComparison.OrdinalIgnoreCase))
                {
                    return statusInstance;
                }
            }
            
            return null; // or throw new ArgumentException($"Status '{status}' not found.");
        }
        public override string ToString()
        {
            return this.Name;
        }
    }
}
