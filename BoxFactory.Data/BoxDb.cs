using BoxFactory.Models;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

namespace BoxFactory.Data;

public class BoxDb
{
    private const int BIN_NUM = 10000;
    private readonly BoxList[]?[] grid; 
    public BoxDb()
    {
        grid = new BoxList[BIN_NUM][];
    }

    private BoxList GetOrCreateBoxList(int x, int y)
    {
        BoxList[]? column = grid[x];
        if (column == null)
        {
            grid[x] = new BoxList[BIN_NUM];
            column = grid[x];
        }

        BoxList? boxList = column[y];
        if (boxList == null)
        {
            column[y] = new BoxList();
            boxList = column[y];
        }

        return boxList;
    }

    private BoxList? GetBoxList(int x, int y)
    {
        BoxList[]? column = grid[x];
        if (column == null)
        {
            return null;
        }

        BoxList? boxList = column[y];
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
        BoxList boxList = GetOrCreateBoxList(x, y);
        DateTime now = DateTime.Now.AddYears(1);
        BoxBatch newBox = new BoxBatch(now, count);
        boxList.Add(newBox);
    }

    public BoxList? GetBoxes(int x, int y)
    {
        BoxList? boxList = GetBoxList(x, y);
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
}