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
//boxDb.Get(90, 60, 10);
BoxList? boxList = boxDb.GetBoxes(85, 62);
Console.ReadLine();

