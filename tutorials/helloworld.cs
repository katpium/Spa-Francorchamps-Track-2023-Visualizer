void WorkWithStrings() {
    Console.WriteLine("Hello, World!");
    string AFriend = "Alice";
    string AnotherFriend = "Bob";
    Console.WriteLine($"Hello, {AFriend} and {AnotherFriend}!");

    string intro = "        Hello, World!        "; // This string has leading and trailing whitespace.
    string trimmedIntro = intro.Trim();
    Console.WriteLine(trimmedIntro);

    string replacedIntro = trimmedIntro.Replace("World", "C#"); // This replaces "World" with "C#" in the trimmedIntro string.
    Console.WriteLine(replacedIntro);

    string upperIntro = replacedIntro.ToUpper();
    Console.WriteLine(upperIntro);

    string lowerIntro = replacedIntro.ToLower();
    Console.WriteLine(lowerIntro);

    string songIntro = "Twinkle, twinkle, little star"; // This string contains the word "twinkle" twice.
    Console.WriteLine(songIntro.Contains("twinkle"));
    Console.WriteLine(songIntro.StartsWith("Dog"));

    Console.WriteLine(songIntro.EndsWith("star"));
    Console.WriteLine(songIntro.StartsWith("twinkle"));
}

void WorkWithIntegers() {
    int a = 293584;
    int b = 383;
    int c = a / b;
    Console.WriteLine(c); // This will perform integer division and then convert the result to a double, which may not give the expected result.
    Console.WriteLine((double)a / b); // This will perform floating-point division and give the expected result.
    Console.WriteLine($"remainder: {(double)a % b}"); // This will calculate the remainder of a divided by b, but since a is cast to double, it will give a floating-point result, which may not be what you expect for a modulus operation.

    int max = int.MaxValue;
    int min = int.MinValue;
    Console.WriteLine($"The range of integers is {min} to {max}");
}

void WorkWithDoubles() {
    double a = 1.0;
    double b = 3.0;
    double result = a / b;
    Console.WriteLine(result); // This will print the value of one-third, which is a repeating decimal and may not be represented exactly in binary, leading to a long decimal representation.

    double maxDouble = double.MaxValue;
    double minDouble = double.MinValue;
    Console.WriteLine($"The range of double is {minDouble} to {maxDouble}");

    double third = 1.0 / 3.0;
    Console.WriteLine(third); // This will print the value of one-third, which is a repeating decimal and may not be represented exactly in binary, leading to a long decimal representation.
}

void WorkWithDecimals() {
    decimal max = decimal.MaxValue;
    decimal min = decimal.MinValue;
    Console.WriteLine($"The range of decimal is {min} to {max}");

    double a = 1.0;
    double b = 3.0;
    double result = a / b;
    Console.WriteLine(result); // This will print the value of one-third, which is a repeating decimal and may not be represented exactly in binary, leading to a long decimal representation. However, since

    decimal c = 1.0m / 3.0m; // WAY more PERCISE that double
    Console.WriteLine(c);
}

double radius = 2.5;
double area = Math.PI * Math.Pow(radius, 2);
Console.WriteLine($"The area of a circle with radius {radius} is {area}");