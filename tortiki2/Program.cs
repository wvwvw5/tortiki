using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Добро пожаловать в кондитерскую!");
        var order = new Order();

        while (true)
        {
            Console.WriteLine("Выберите опцию:");
            Console.WriteLine("1. Сделать заказ");
            Console.WriteLine("2. Посмотреть историю заказов");
            Console.WriteLine("3. Выйти");

            var choice = Console.ReadKey().Key;

            switch (choice)
            {
                case ConsoleKey.D1:
                    Console.Clear();
                    order.MakeOrder();
                    break;
                case ConsoleKey.D2:
                    Console.Clear();
                    order.DisplayOrderHistory();
                    break;
                case ConsoleKey.D3:
                    return;
                default:
                    Console.Clear();
                    Console.WriteLine("Некорректный выбор. Попробуйте снова.");
                    break;
            }
        }
    }
}

class Order
{
    private List<OrderItem> items = new List<OrderItem>();

    public void MakeOrder()
    {
        var menu = new Menu("Торты");
        menu.AddMenuItem("Шоколадный", 10);
        menu.AddMenuItem("Ванильный", 12);
        menu.AddMenuItem("Фруктовый", 15);

        Console.WriteLine("Выберите торт:");
        var selectedCake = menu.DisplayAndGetChoice();

        Console.WriteLine("Выберите размер торта:");
        var sizeMenu = new Menu("Размер");
        sizeMenu.AddMenuItem("Маленький", 5);
        sizeMenu.AddMenuItem("Средний", 10);
        sizeMenu.AddMenuItem("Большой", 15);
        var selectedSize = sizeMenu.DisplayAndGetChoice();

        // Добавьте дополнительные меню для вкуса, количества, глазури, декора и т. д.

        Console.WriteLine("Введите количество:");
        int quantity = int.Parse(Console.ReadLine());

        items.Add(new OrderItem(selectedCake.Description, selectedSize.Description, quantity, selectedCake.Price));
        Console.WriteLine("Заказ добавлен в корзину.");

        Console.WriteLine("1. Сделать еще заказ");
        Console.WriteLine("2. Завершить заказ");

        var choice = Console.ReadKey().Key;
        if (choice == ConsoleKey.D2)
        {
            SaveOrder();
        }
    }

    private void SaveOrder()
    {
        using (StreamWriter writer = new StreamWriter("История заказов.txt", true))
        {
            writer.WriteLine("Заказ:");
            foreach (var item in items)
            {
                writer.WriteLine($"{item.Quantity} x {item.Cake} ({item.Size}): {item.TotalPrice:C}");
            }
            writer.WriteLine($"Сумма заказа: {items.Sum(item => item.TotalPrice):C}");
            writer.WriteLine(new string('-', 20));
        }

        items.Clear();
        Console.WriteLine("Заказ сохранен.");
    }

    public void DisplayOrderHistory()
    {
        Console.WriteLine("История заказов:");
        if (File.Exists("История заказов.txt"))
        {
            using (StreamReader reader = new StreamReader("История заказов.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }
        else
        {
            Console.WriteLine("История заказов пуста.");
        }
    }
}

class Menu
{
    private List<MenuItem> items = new List<MenuItem>();
    public string Description { get; }

    public Menu(string description)
    {
        Description = description;
    }

    public void AddMenuItem(string description, decimal price)
    {
        items.Add(new MenuItem(description, price));
    }

    public MenuItem DisplayAndGetChoice()
    {
        Console.WriteLine(Description + ":");
        for (int i = 0; i < items.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {items[i].Description} ({items[i].Price:C})");
        }

        int choice = int.Parse(Console.ReadLine());
        return items[choice - 1];
    }
}

class MenuItem
{
    public string Description { get; }
    public decimal Price { get; }

    public MenuItem(string description, decimal price)
    {
        Description = description;
        Price = price;
    }
}

class OrderItem
{
    public string Cake { get; }
    public string Size { get; }
    public int Quantity { get; }
    public decimal TotalPrice { get; }

    public OrderItem(string cake, string size, int quantity, decimal price)
    {
        Cake = cake;
        Size = size;
        Quantity = quantity;
        TotalPrice = price * quantity;
    }
}
