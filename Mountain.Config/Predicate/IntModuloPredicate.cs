namespace Mountain.Config.Predicate
{
    public class IntModuloPredicate : BaseDataPredicate
    {
        public int Divisor { get; }
        public int ExpectedQuotient { get; }
        public IntModuloPredicate(int divisor, int expectedQuotient)
        {
            Divisor = divisor;
            ExpectedQuotient = expectedQuotient;
        }

        public override bool Validate(object obj) => (obj is int i) && i % Divisor == ExpectedQuotient;
    }
}
