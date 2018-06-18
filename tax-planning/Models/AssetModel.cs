namespace tax_planning.Models
{
    public class AssetModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Value { get; set; }

        private decimal _EmployerMatchProportion = 0.00M;
        public decimal EmployerMatchProportion
        {
            get
            {
                return _EmployerMatchProportion;
            }
            set
            {
                _EmployerMatchProportion = value / 100;
            }
        }

        private decimal _EmployerMatchCap = 0.00M;
        public decimal EmployerMatchCap
        {
            get
            {
                return _EmployerMatchCap;
            }
            set
            {
                _EmployerMatchCap = value / 100;
            }
        }
    }
}
