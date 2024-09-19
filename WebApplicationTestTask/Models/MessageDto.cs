namespace WebApplicationTestTask.Models
{
    public class MessageDto
    {
        public string? Text
        {
            get
            {
                return Text;
            }
            set
            {
                if (value?.Length > 128)
                    throw new ArgumentException("Message length is larger than 128!");
                Text = value;
            }
        }
        public int Order { get; set; }
    }
}
