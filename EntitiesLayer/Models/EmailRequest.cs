namespace EntitiesLayer.Models;

public class EmailRequest
    {
        public List<string> ToEmails { get; set; } 
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
    }

