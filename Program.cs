using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введите длину стороны А:");
        string sideA = Console.ReadLine();
        Console.WriteLine("Введите длину стороны Б:");
        string sideB = Console.ReadLine();
        Console.WriteLine("Введите длину стороны С:");
        string sideC = Console.ReadLine();

        try
        {
            float a = float.Parse(sideA);
            float b = float.Parse(sideB);
            float c = float.Parse(sideC);

            string result = CalculateTriangleType(a, b, c, out var vertices);
            LogSuccess(sideA, sideB, sideC, result, vertices);
        }
        catch (Exception ex)
        {
            LogError(sideA, sideB, sideC, ex.ToString());
        }
    }

    static string CalculateTriangleType(float a, float b, float c, out (int, int)[] vertices)
    {
        vertices = new (int, int)[3];

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
            return "равносторонний";
        else if (a == b || b == c || a == c)
            return "равнобедренный";
        else
            return "разносторонний";
    }

    static void LogSuccess(string sideA, string sideB, string sideC, string result, (int, int)[] vertices)
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

    static void LogError(string sideA, string sideB, string sideC, string errorMessage)
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