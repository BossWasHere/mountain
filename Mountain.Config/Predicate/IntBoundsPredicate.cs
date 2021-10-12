namespace Mountain.Config.Predicate
{
    public class IntBoundsPredicate : BaseDataPredicate
    {
        public int Min { get; }
        public int Max { get; }
        public IntBoundsPredicate(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public override bool Validate(object obj) => (obj is int i) && i >= Min && i <= Max;
    }
}
