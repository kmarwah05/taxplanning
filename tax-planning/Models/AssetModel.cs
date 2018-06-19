namespace tax_planning.Models
{
    public class AssetModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Value { get; set; }

        private decimal _Match = 0.00M;
        public decimal Match
        {
            get
            {
                return _Match;
            }
            set
            {
                _Match = value / 100;
            }
        }

        private decimal _Cap = 0.00M;
        public decimal Cap
        {
            get
            {
                return _Cap;
            }
            set
            {
                _Cap = value / 100;
            }
        }
    }
}
