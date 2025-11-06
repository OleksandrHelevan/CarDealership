using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using CarDealership.config;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.page.guest;

public partial class GuestCarsPage : Page
{
    private readonly DealershipContext _context;

    public GuestCarsPage()
    {
        InitializeComponent();
        _context = new DealershipContext();
        LoadCars();
    }

    private void LoadCars()
    {
        var cars = _context.Cars
            .Include(c => c.Engine)
            .OrderBy(c => c.Brand)
            .ThenBy(c => c.ModelName)
            .ThenBy(c => c.Year)
            .ToList();

        var items = cars.Select(c => new GuestCarItem
        {
            Title = $"{c.Brand} {c.ModelName}",
            YearText = $"Рік випуску: {c.Year}",
            PriceText = $"Ціна: ${c.Price:N2}",
            MileageText = $"Пробіг: {c.Mileage} км",
            BodyText = $"Тип кузова: {c.BodyTypeString}",
            DriveText = $"Привід: {c.DriveTypeString}",
            TransmissionText = $"Трансмісія: {c.TransmissionString}",
            ColorText = $"Колір: {c.ColorString}",
            DoorsText = $"Кількість дверей: {c.NumberOfDoors}",
            CarTypeText = $"Тип авто: {c.CarTypeString}",
            WeightText = $"Вага: {c.Weight} кг",
            EngineSummary = c.Engine?.EngineString ?? "Дані про двигун відсутні"
        }).ToList();

        CarsList.ItemsSource = items;
    }

    private record GuestCarItem
    {
        public string Title { get; init; }
        public string YearText { get; init; }
        public string PriceText { get; init; }
        public string MileageText { get; init; }
        public string BodyText { get; init; }
        public string DriveText { get; init; }
        public string TransmissionText { get; init; }
        public string ColorText { get; init; }
        public string DoorsText { get; init; }
        public string CarTypeText { get; init; }
        public string WeightText { get; init; }
        public string EngineSummary { get; init; }
    }
}
