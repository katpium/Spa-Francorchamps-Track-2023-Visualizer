
var pt = (X: 1, Y: 2);

var slope = (double)pt.Y / (double)pt.X;
Console.WriteLine($"A line from the origin to the point {pt} has a slope of {slope}.");

pt.X = pt.X + 10;
Console.WriteLine($"The point is now {pt}.");

var pt2 = pt with { X = 100 };
Console.WriteLine($"The point is now {pt2}.");

var subscript = (A: 0, B: 0);
subscript = pt;
Console.WriteLine(subscript);

var namedData = (Name: "Morning observation", Temp: 17, Wind: 4);
var person = (FirstName: "", LastName: "");
var order = (Product: "guitar picks", style: "triangle", quantity: 500, UnitPrice: 0.10m);


void WorkWithRecords()
{
    var pt1 = new Point(1, 2);
    Console.WriteLine($"The point is {pt1}.");

    var pt2 = pt1 with { X = 100 };
    Console.WriteLine($"The point is now {pt2}.");
}

//USE RECORDS TO HELP WITH IMMUTABLE DATA

Point pt3 = new Point(1, 1);
var pt4 = pt3 with { Y = 10 };
Console.WriteLine($"The two points are {pt3} and {pt4}");

double slopeResult = pt4.Slope();
Console.WriteLine($"The slope of {pt4} is {slopeResult}");
public record struct Point(int X, int Y) {
    public double Slope() => (double)Y / (double)X; //YOU CAN CALL ON THE METHOD
}