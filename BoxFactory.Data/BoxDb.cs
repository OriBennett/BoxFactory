using BoxFactory.Models;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using static BoxFactory.Data.BoxDb;
using static BoxFactory.Models.BoxBatch;

namespace BoxFactory.Data;

public class BoxDb
{
    private const int BIN_NUM = 10000;
    private readonly LList<BoxBatch>[]?[] grid;
    private readonly LList<LList<BoxBatch>> allLists;

    public BoxDb()
    {
        grid = new LList<BoxBatch>[BIN_NUM][];
        allLists = new LList<LList<BoxBatch>>();
    }

    private LList<BoxBatch> GetOrCreateBoxList(int x, int y)
    {
        LList<BoxBatch>[]? column = grid[x];
        if (column == null)
        {
            grid[x] = new LList<BoxBatch>[BIN_NUM];
            column = grid[x];
        }

        LList<BoxBatch>? boxList = column[y];
        if (boxList == null)
        {
            column[y] = new LList<BoxBatch>();
            boxList = column[y];

            allLists.Add(boxList);
        }

        return boxList;
    }

    private LList<BoxBatch>? GetBoxList(int x, int y)
    {
        LList<BoxBatch>[]? column = grid[x];
        if (column == null)
        {
            return null;
        }

        LList<BoxBatch>? boxList = column[y];
        if (boxList == null)
        {
            return null;
        }

        // lazy deletition
        if (boxList.Count == 0)
        {
            column[y] = null;
            return null;
        }

        return boxList;
    }

    public void Add(int x, int y, int count)
    { 
        LList<BoxBatch> boxList = GetOrCreateBoxList(x, y);
        DateTime now = DateTime.Now.AddYears(1);
        BoxBatch newBox = new BoxBatch(now, count);
        boxList.Add(newBox);
    }

    public LList<BoxBatch>? GetBoxes(int x, int y)
    {
        LList<BoxBatch>? boxList = GetBoxList(x, y);
        if (boxList != null)
        {
            return boxList;
        }

        double PercentageTolerance = 0.1;
        int xMax = Convert.ToInt32(x * (1.0 + PercentageTolerance));
        int xMin = Convert.ToInt32(x * (1.0 - PercentageTolerance));
        int yMax = Convert.ToInt32(y * (1.0 + PercentageTolerance));
        int yMin = Convert.ToInt32(y * (1.0 - PercentageTolerance));

        for (int xIter = xMin; xIter < xMax; xIter++)
        {
            boxList = GetBoxList(xIter, y);
            if (boxList != null)
            {
                return boxList;
            }
        }

        for (int yIter = yMin; yIter < yMax; yIter++)
        {
            boxList = GetBoxList(x, yIter);
            if (boxList != null)
            {
                return boxList;
            }

        }

        for (int xIter = xMin; xIter < xMax; xIter++)
        {
            for (int yIter = yMin; yIter < yMax; yIter++)
            {
                boxList = GetBoxList(xIter, yIter);
                if (boxList != null)
                {
                    return boxList;
                }
            }
        }

        return null;
    }

    public class ListsIterator : IEnumerator<BoxBatch>
    {
        private LList<BoxBatch>.BoxListEnumerator<BoxBatch> _listIterator;
        private LList<LList<BoxBatch>>.BoxListEnumerator<LList<BoxBatch>> _listsIterator;

        public ListsIterator(LList<LList<BoxBatch>> listsHead)
        {
            _listsIterator = listsHead.GetEnumerator();
            _listsIterator.MoveNext();
            LList<BoxBatch> firstList = _listsIterator.Current;
            _listIterator = firstList.GetEnumerator();            
        }
        public BoxBatch Current
        {
            get { return _listIterator.Current; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public void Dispose()
        { }

        public bool MoveNext()
        {
            ;
            while (_listIterator.MoveNext() == false)
            {
                bool isNExtListNotNull = _listsIterator.MoveNext();
                if (isNExtListNotNull == false)
                {
                    return false;
                }

                LList<BoxBatch> firstList = _listsIterator.Current;
                _listIterator = firstList.GetEnumerator();
            }

            return true;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public ListsIterator GetEnumerator()
        { return this; }

    }
    public ListsIterator GetAllLists()
    {
        return new ListsIterator(allLists);
    }
}