namespace WebApplicationTestTask.Models
{
    public class MessageDto
    {
        private string? text;
        public string? Text
        {
            get
            {
                return text;
            }
            set
            {
                if (value?.Length > 128)
                    throw new ArgumentException("Message length is larger than 128!");
                text = value;
            }
        }
        public int Order { get; set; }
    }
}
