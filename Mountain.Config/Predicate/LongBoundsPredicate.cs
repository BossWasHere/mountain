namespace Mountain.Config.Predicate
{
    public class LongBoundsPredicate : BaseDataPredicate
    {
        public long Min { get; }
        public long Max { get; }
        public LongBoundsPredicate(long min, long max)
        {
            Min = min;
            Max = max;
        }

        public override bool Validate(object obj) => (obj is long i) && i >= Min && i <= Max;
    }
}
