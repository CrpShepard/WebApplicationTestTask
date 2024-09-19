namespace WebApplicationTestTask.Models
{
    public class Message
    {
        public int Id { get; set; }  
        public string? Text {
            get {
                return Text;
            }
            set {
                if (value?.Length > 128)
                    throw new ArgumentException("Message length is larger than 128!");
                Text = value;
            } 
        } 
        public DateTime Timestamp { get; set; }
        public int Order { get; set; }  
    }
}
