namespace SubClient1.Models
{
    public class HomeViewModel
    {
        public bool Status1 { get; set; } = false;
        public bool Status2 { get; set; } = false;
        public bool Status3 { get; set; } = false;

        public List<string> Topic1{ get; set; } = new List<string>();
        public List<string> Topic2 { get; set; } = new List<string>();
        public List<string> Topic3 { get; set; } = new List<string>();
    }
}
