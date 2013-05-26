namespace ilf.pgn.Data
{
    public class Square
    {
        public Square(File file, int rank)
        {
            File = file;
            Rank = rank;
        }

        public File File { get; set; }
        public int Rank { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Square;
            if (other == null) return false;

            return
                this.File == other.File &&
                this.Rank == other.Rank;
        }

        public override int GetHashCode()
        {
            return ((int) this.File)*this.Rank;
        }

        public override string ToString()
        {
            return this.File.ToString() + this.Rank;
        }

        public static bool operator ==(Square a, Square b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Square a, Square b)
        {
            return !(a == b);
        }
    }
}

