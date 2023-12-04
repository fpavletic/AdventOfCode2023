namespace AdventOfCode2023;

public static class PersonalContants{

    public static (string Name, string Value) GetCookiesName() => 
        ("session", "53616c7465645f5fe4f41d6a8ff549001be6e3f914346b4b65df8fdc6e605ffcf307718211b25be28e32f60f0df3f155930f7ef4f22d17851964f3db3d644fd9");

    public static string GetInputFilePathTemplate() =>
        "C:/Dev/AdventOfCode2023/data/year{0}/day{1}/input.txt";
        
}