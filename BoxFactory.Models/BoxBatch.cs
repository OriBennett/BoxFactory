using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxFactory.Models
{
    public class BoxBatch
    {
        private int _count;
        private readonly DateTime _expiryDate;
        public int Count { get => _count;}
        public DateTime ExpiryDate { get => _expiryDate;}

        public BoxBatch(DateTime expiryDate, int count)
        {
            _expiryDate = expiryDate;
            _count = count;
        }
        public void ReduceCount(int reduceBy)
        {
            if (reduceBy > _count)
            {
                throw new Exception("Bug in Reduce count - reduce by ammount greater than count");
            }

            _count -= reduceBy;
        }

        public bool IsExpired()
        {
            return _expiryDate < DateTime.Now;
        }
    }
}


