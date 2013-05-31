namespace ilf.pgn.Data
{
    public class GameInfo
    {
        public GameInfo(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("[{0} \"{1}\"]", Name, Value);
        }
    }

}
