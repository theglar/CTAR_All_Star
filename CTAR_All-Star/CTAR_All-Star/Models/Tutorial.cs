using System;
namespace CTAR_All_Star.Models
{
    public class Tutorial
    {
        public string Topic { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public bool isVisible { get; set; }
        public string URL { get; set; }

        public Tutorial()
        {
        }
    }
}
