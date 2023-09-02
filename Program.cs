using System;
using System.IO;
using System.Text.Unicode;
using Serilog;
using static System.Net.WebRequestMethods;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введите длину стороны A:");
        string sideA = Console.ReadLine();
        Console.WriteLine("Введите длину стороны B:");
        string sideB = Console.ReadLine();
        Console.WriteLine("Введите длину стороны C:");
        string sideC = Console.ReadLine();

        try
        {
            float a = float.Parse(sideA);
            float b = float.Parse(sideB);
            float c = float.Parse(sideC);
            string result = CalculateTriangleType(a, b, c, out var vertices);
            string template = "{Timestamp:HH:mm:ss} | [{Level:u3}] | {Message:lj}{NewLine}{Exception}";
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: template)
                .WriteTo.File("logs/file.txt", outputTemplate: template)
                .CreateLogger();

            Log.Information("Логгер сконфигурирован");
            Log.Information("Приложение запущено");
            Log.Debug($"Параметры запроса: сторона А={sideA}, сторона Б={sideB}, сторона С={sideC}\n" +
                $"Результаты запроса: тип треугольника - {result}, координаты вершин - ({vertices[0].Item1},{vertices[0].Item2}), ({vertices[1].Item1},{vertices[1].Item2}), ({vertices[2].Item1},{vertices[2].Item2})");
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Ошибка запроса: {DateTime.Now}\n" +
                $"Параметры запроса: сторона А={sideA}, сторона Б={sideB}, сторона С={sideC}");
        }

        Log.CloseAndFlush();
        Console.ReadKey();
    }
    static string CalculateTriangleType(string a, string b, string c, out (int, int)[] vertices)
    {
        vertices = new (int, int)[3];//координаты

        if (a <= 0 || b <= 0 || c <= 0)
        {
            vertices[0] = (-1, -1);
            vertices[1] = (-1, -1);
            vertices[2] = (-1, -1);
            return "не треугольник";
        }

        float max = Math.Max(a, Math.Max(b, c));
        float sum = a + b + c;

        if (max >= sum - max)
        {
            vertices[0] = (-1, -1);
            vertices[1] = (-1, -1);
            vertices[2] = (-1, -1);
            return "не треугольник";
        }

        if (a == b && b == c)
        {
            vertices[0] = (0, 0);
            vertices[1] = (a, 0);
            vertices[2] = (a / 2, (int)(Math.Sqrt(3) * a / 2));
            return "равносторонний";
        }

        else if (a == b || b == c || a == c)
        {
            // Найдем координаты самой короткой стороны
            int shortestSide = Math.Min(Math.Min(a, b), c);

            if (a == b)
            {
                vertices[0] = (0, 0);
                vertices[1] = (shortestSide, 0);
                vertices[2] = (shortestSide / 2, (int)(Math.Sqrt(3) * shortestSide / 2));
            }
            else if (b == c)
            {
                vertices[0] = (0, 0);
                vertices[1] = (shortestSide / 2, (int)(Math.Sqrt(3) * shortestSide / 2));
                vertices[2] = (shortestSide, 0);
            }
            else if (a == c)
            {
                vertices[0] = (shortestSide, 0);
                vertices[1] = (0, 0);
                vertices[2] = (shortestSide / 2, (int)(Math.Sqrt(3) * shortestSide / 2));
            }

            return "равнобедренный";
        }
        else
        {
            int height = CalculateTriangleHeight(a, b, c);

            vertices[0] = (0, 0);
            vertices[1] = (a, 0);
            vertices[2] = (b, height);

            return "разносторонний";
        }
    }

    static int CalculateTriangleHeight(int a, int b, int c)
    {
        // Найдем наибольшую сторону, чтобы использовать ее в формуле для вычисления высоты
        int max = Math.Max(a, Math.Max(b, c));

        if (max == a)
        {
            return (int)(2 * Math.Sqrt(b * c * (b + c - a)) / b);
        }
        else if (max == b)
        {
            return (int)(2 * Math.Sqrt(a * c * (a + c - b)) / a);
        }
        else
        {
            return (int)(2 * Math.Sqrt(a * b * (a + b - c)) / a);
        }
    }
    static void LogSuccess(string sideA, string sideB, string sideC, string result, (int, int)[] vertices) //логирование 
    {
        string logEntry = $"Успешный запрос: {DateTime.Now}\n" +
                          $"Параметры запроса: сторона А={sideA}, сторона Б={sideB}, сторона С={sideC}\n" +
                          $"Результаты запроса: тип треугольника - {result}, координаты вершин - ({vertices[0].Item1},{vertices[0].Item2}), ({vertices[1].Item1},{vertices[1].Item2}), ({vertices[2].Item1},{vertices[2].Item2})";

        Console.WriteLine(logEntry);
        using (StreamWriter writer = new StreamWriter("log.txt", true))
        {
            writer.WriteLine(logEntry);
        }
    }
    static void LogError(string sideA, string sideB, string sideC, string errorMessage)//логирование 
    {
        string logEntry = $"Ошибка запроса: {DateTime.Now}\n" +
                          $"Параметры запроса: сторона А={sideA}, сторона Б={sideB}, сторона С={sideC}\n" +
                          $"Текст ошибки: {errorMessage}";

        Console.WriteLine(logEntry);
        using (StreamWriter writer = new StreamWriter("log.txt", true))
        {
            writer.WriteLine(logEntry);
        }
    }
}