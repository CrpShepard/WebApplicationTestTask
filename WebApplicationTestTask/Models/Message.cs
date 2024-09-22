namespace WebApplicationTestTask.Models
{
    public class Message
    {
        public int Id { get; set; }
        private string? text;
        public string? Text {
            get {
                return text;
            }
            set {
                if (value?.Length > 128)
                    throw new ArgumentException("Message length is larger than 128!");
                text = value;
            } 
        } 
        public DateTime Timestamp { get; set; }
        public int Order { get; set; }  
    }
}
