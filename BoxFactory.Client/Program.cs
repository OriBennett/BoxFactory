// See https://aka.ms/new-console-template for more information
using BoxFactory.Data;
using BoxFactory.Models;
using BoxFactory.Services;


Console.WriteLine("what size base would you like?");
double x;
double.TryParse(Console.ReadLine(), out x);
Console.WriteLine("what hight would you like the box?");
double y;
double.TryParse(Console.ReadLine(), out y);

BoxDb boxDb = new BoxDb();


boxDb.Add(90, 60, 10);
boxDb.Add(90, 60, 9);

boxDb.Add(5, 5, 8);

BoxDb.ListsIterator iteratorr = boxDb.GetAllLists();

foreach (BoxBatch bb in iteratorr)
{
    Console.WriteLine(bb.Count);
}

LList<BoxBatch>? boxList = boxDb.GetBoxes(85, 62);
if (boxList != null)
{
    foreach (BoxBatch bb in boxList)
    {
        Console.WriteLine(bb.Count);
    }

    LList<BoxBatch>.BoxListEnumerator<BoxBatch> iterator = boxList.GetEnumerator();
    while (iterator.MoveNext())
    {
        if (iterator.Current.Count == 10)
        {
            iterator.Erase();
        }
    }

    foreach (BoxBatch bb in boxList)
    {
        Console.WriteLine(bb.Count);
    }

    iterator = boxList.GetEnumerator();
    while (iterator.MoveNext())
    {
        if (iterator.Current.Count == 9)
        {
            iterator.Erase();
        }
    }

    foreach (BoxBatch bb in boxList)
    {
        Console.WriteLine(bb.Count);
    }

}

Console.ReadLine();

