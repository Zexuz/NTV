using System;

namespace NTV.Scraper.Sites
{
    public class RateLimit
    {
        private readonly TimeSpan _timer;
        private DateTime _timeWhenStarted;

        public int Remaining => NumberOfPosibleRequest - RequestUsed;
        public int RequestUsed { get; set; }
        public DateTime DateWhenReset { get; set; }
        public int NumberOfPosibleRequest { get; }

        public RateLimit(int numberOfPosibleRequest, TimeSpan timer)
        {
            _timer = timer;
            NumberOfPosibleRequest = numberOfPosibleRequest;
            _timeWhenStarted = DateTime.Now;
        }

        public void RequestDone()
        {
            if (Remaining == 0) throw new Exception("We can't do this request now or we might get flaged.");
            if (RequestUsed == 0 && DateWhenReset == DateTime.MinValue)
            {
                DateWhenReset = DateTime.Now.AddSeconds(_timer.TotalSeconds);
            }
            RequestUsed++;
        }

        public void Reset()
        {
            RequestUsed = 0;
            DateWhenReset = DateTime.MinValue;
        }

        public TimeSpan GetTimeSpanUntillNextScrape()
        {
            var timeNow = DateTime.Now;
            var millisecUntillReset = _timer.TotalMilliseconds - (timeNow - _timeWhenStarted).TotalMilliseconds;
            var millisecUntillNextScrape = millisecUntillReset / Remaining;
            return TimeSpan.FromMilliseconds(millisecUntillNextScrape);
        }
    }
}