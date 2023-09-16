using System;
using Serilog;

class TriangleCalculator
{
    private ILogger logger;

    public TriangleCalculator()
    {
        // Инициализация логгера Serilog для записи в файл и консоль
        logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }

    public (string, (int, int)[]) CalculateTriangle(string sideA, string sideB, string sideC)
    {
        logger.Information("Запрос: сторона A = {SideA}, сторона B = {SideB}, сторона C = {SideC}", sideA, sideB, sideC);
        string triangleType = "";
        (int, int)[] vertices = new (int, int)[3];
        double a, b, c;

        if (double.TryParse(sideA, out a) && double.TryParse(sideB, out b) && double.TryParse(sideC, out c))
        {
            try
            {
                // Проверка на неотрицательность сторон
                if (a <= 0 || b <= 0 || c <= 0)
                {
                    logger.Error("Ошибка: одна или несколько сторон имеют недопустимое значение");
                }

                // Проверка на существование треугольника
                if (a + b > c && b + c > a && c + a > b)
                {

                    if (a == b && b == c)
                    {
                        triangleType = "равносторонний";
                    }
                    else if (a == b || b == c || c == a)
                    {
                        triangleType = "равнобедренный";
                    }
                    else
                    {
                        triangleType = "разносторонний";
                    }

                    // Вычисление координат вершин треугольника
                    int scalingFactor = 50; // Масштабный коэффициент для отображения в поле 100x100 px
                    vertices[0] = (scalingFactor, scalingFactor);
                    vertices[1] = (scalingFactor + (int)(a * scalingFactor), scalingFactor);
                    vertices[2] = (scalingFactor + (int)((c * c - b * b + a * a) / (2.0 * a) * scalingFactor), scalingFactor + (int)(Math.Sqrt(c * c - ((c * c - b * b + a * a) / (2.0 * a)) * ((c * c - b * b + a * a) / (2.0 * a))) * scalingFactor));
                    logger.Information("Результат: Тип треугольника - {triangleType}, Координаты вершин: A({vertexA.Item1},{vertexA.Item2}), B({vertexB.Item1},{vertexB.Item2}), C({vertexC.Item1},{vertexC.Item2})", triangleType, vertices[0], vertices[1], vertices[2]);
                }
                else
                {
                    triangleType = "";
                    vertices[0] = (-1, -1);
                    vertices[1] = (-1, -1);
                    vertices[2] = (-1, -1);
                    logger.Information("Результат: Не треугольник");
                }
            }
            catch (Exception ex)
            {
                triangleType = "";
                vertices[0] = (-1, -1);
                vertices[1] = (-1, -1);
                vertices[2] = (-1, -1);
                logger.Error(ex, "Ошибка при вычислении треугольника");
            }
        }
        else
        {
            triangleType = "";
            vertices[0] = (-2, -2);
            vertices[1] = (-2, -2);
            vertices[2] = (-2, -2);
            logger.Error("Ошибка: некорректные данные сторон треугольника");
        }
        return (triangleType, vertices);
    }
}

class Program
{
    static void Main()
    {
        ILogger logger;
        // Инициализация логгера Serilog для записи в файл и консоль
        logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        string a="";
        string b="";
        string c="";
        // Инициализация калькулятора треугольников
        var triangleCalculator = new TriangleCalculator();
        Console.WriteLine("Сторона А:");
        a=Console.ReadLine();
        Console.WriteLine("Сторона B:");
        b=Console.ReadLine();
        Console.WriteLine("Сторона C:");
        c=Console.ReadLine();
        triangleCalculator.CalculateTriangle(a,b,c);
    }
}