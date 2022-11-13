namespace SubClient1.Services
{
    public class DataWrapper
    {
        private bool _statusTopic1;
        public bool StatusTopic1
        {
            get { return Topic1.Count > 0 || _statusTopic1; }
            set { _statusTopic1 = value; }
        }

        private bool _statusTopic2;
        public bool StatusTopic2
        {
            get { return Topic2.Count > 0 || _statusTopic2; }
            set { _statusTopic2 = value; }
        }

        private bool _statusTopic3;
        public bool StatusTopic3
        {
            get { return Topic3.Count > 0 || _statusTopic3; }
            set { _statusTopic3 = value; }
        }

        public List<string> Topic1 { get; set; } = new List<string>();
        public List<string> Topic2 { get; set; } = new List<string>();
        public List<string> Topic3 { get; set; } = new List<string>();
    }
}
