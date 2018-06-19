namespace tax_planning.Models
{
    public class AssetModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Value { get; set; }

        private decimal _EmployerMatchPercentage = 0.00M;
        public decimal EmployerMatchPercentage
        {
            get
            {
                return _EmployerMatchPercentage;
            }
            set
            {
                _EmployerMatchPercentage = value / 100;
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
