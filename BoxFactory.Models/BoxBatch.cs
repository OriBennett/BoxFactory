using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxFactory.Models
{
    // Class holding a batch of boxes
    public class BoxBatch
    {
        // Number of boxes in the batch.
        private int _count;
        // Expiry date
        private readonly DateTime _expiryDate;
        // Properties for getting count and expiry date.
        public int Count { get => _count;}
        public DateTime ExpiryDate { get => _expiryDate;}

        // Constructore
        public BoxBatch(DateTime expiryDate, int count)
        {
            _expiryDate = expiryDate;
            _count = count;
        }
        // reducing ammount of boxes from the batch.
        // Throws exception of the reduction amount is grater than the number of boxes.
        public void ReduceCount(int reduceBy)
        {
            if (reduceBy > _count)
            {
                throw new Exception("Bug in Reduce count - reduce by ammount greater than count");
            }

            _count -= reduceBy;
        }

        // Check if the batch is expired
        public bool IsExpired()
        {
            return _expiryDate < DateTime.Now;
        }
    }
}


