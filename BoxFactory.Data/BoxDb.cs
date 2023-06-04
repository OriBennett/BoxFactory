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
 // Variables:
    // Hard coded max size of each dimention.
    private const int BIN_NUM = 10000;

    // Grid of lists of box batches.
    private readonly LList<BoxBatch>[]?[] grid;

    // List of all of the created lists of box batches in the grid.
    private readonly LList<LList<BoxBatch>> allLists;

 // Private methods:
    // Check if input parameters are legal.
    private static bool isInputLegal(int x, int y)
    {
        if (x <= 0 || x >= BIN_NUM)
        {
            Console.WriteLine("Input value of x in ilegal, must be between 1 and " + (BIN_NUM -1));
            return false;
        }

        if (y <= 0 || y >= BIN_NUM)
        {
            Console.WriteLine("Input value of y in ilegal, must be between 1 and " + (BIN_NUM - 1));
            return false;
        }

        return true;
    }

    // Check if input parameters are legal.
    private static bool isInputLegal(int x, int y, int count)
    {
        if (isInputLegal(x,y) == false)
        {
            return false;
        }

        if(count <= 0)
        {
            Console.WriteLine("Input value of count in ilegal, must be grater than 0");
            return false;
        }

        return true;
    }

    // Getting a list in the specified indexes, or creating one if it not exists.
    // On creation, updating both grid and allLists. 
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

    // Getting a list from the grid in the specified indexes.
    // If the list is empty, returns null
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

        // Get function not returning empty list, it is done only in GetOrCreateBoxList
        if (boxList.Count == 0)
        {
            return null;
        }

        return boxList;
    }

 // Public methods:
    // Constuctor.
    public BoxDb()
    {
        grid = new LList<BoxBatch>[BIN_NUM][];
        allLists = new LList<LList<BoxBatch>>();
    }

    // Adding new box batch.
    public void Add(int x, int y, int count)
    { 
        if (isInputLegal(x, y, count) == false)
        {
            return;
        }

        LList<BoxBatch> boxList = GetOrCreateBoxList(x, y);
        DateTime now = DateTime.Now.AddYears(1);
        BoxBatch newBox = new BoxBatch(now, count);
        boxList.Add(newBox);
    }

    // Returning all boxes with the provided dimentions.
    public LList<BoxBatch>? GetBoxes(int x, int y)
    {
        if (isInputLegal(x, y) == false)
        {
            return null;
        }  

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

    // Iterator class for iterating box batches over a list of lists of box batches.
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

    // Returning all boxes in system.
    public ListsIterator GetAllLists()
    {
        return new ListsIterator(allLists);
    }
}