using System;

namespace JobApi.Models
{
    public class JobItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime DateEnd { get; set; }
        public string Place { get; set; }
        public string Region { get; set; }
    }
}
