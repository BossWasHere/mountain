using Mountain.Core.Enums;
using Mountain.Core.Exceptions;

namespace Mountain.Core.Statistics
{
    public class Statistic
    {
        private const int MAX_CUSTOM_STAT = 49;

        public StatisticCategory StatisticCategory { get; set; }

        private CustomStatistic cs = CustomStatistic.Unset;
        public CustomStatistic CustomStatistic
        {
            get => cs;
            set
            {
                cs = value;
                if (value != CustomStatistic.Unset)
                {
                    StatisticCategory = StatisticCategory.Custom;
                }
            }
        }

        private int sid = 0;
        public int StatisticId
        {
            get
            {
                return HasCustomStatistic ? (int)(object)cs : sid;
            }
            set
            {
                if (HasCustomStatistic)
                {
                    if (value < 0 || value > MAX_CUSTOM_STAT) throw new PropertyException("Cannot set statistic ID outside of range for custom statistic category");
                    cs = (CustomStatistic)(object)value;
                }
                else
                {
                    sid = value;
                }
            }
        }

        public bool HasCustomStatistic => StatisticCategory == StatisticCategory.Custom;
        public int Value { get; set; }
    }
}
