using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.CodeAnalysis.Operations;
using Newtonsoft.Json;

namespace PreparationAvaloniaPlanningAndStatistics;

public partial class MainWindow : Window
{
    private int _month = 1;
    public MainWindow()
    {
        InitializeComponent();
        LoadData();
    }

    private async Task LoadData()
    {
        HttpResponseMessage responseMessage = await HttpClientHelper.Client.GetAsync("http://localhost:5263/api/DoctorsChedule/DoctorsCheduleGet");
        string content = await responseMessage.Content.ReadAsStringAsync();
        List<DoctorList> docrors = JsonConvert.DeserializeObject<List<DoctorList>>(content)!.ToList();
        BoxDoctors.ItemsSource = docrors.Select(x => new
        {
            NameDoctor = x.Namrdoctor
        }).ToList();

        HttpResponseMessage httpResponseMessage = await HttpClientHelper.Client.GetAsync("http://localhost:5263/api/DirectionChedule/DirectionCheduleGet");
        string contents = await httpResponseMessage.Content.ReadAsStringAsync();
        List<DirectionsList> directions = JsonConvert.DeserializeObject<List<DirectionsList>>(contents)!.ToList();
        BoxDirections.ItemsSource = directions.Select(x => new
        {
            Namedirection = x.Namedirection
        }).ToList();
        
        
        string monthText = "Декабрь";
        HttpResponseMessage response = await HttpClientHelper.Client.GetAsync($"http://localhost:5263/api/Planing/PlaningGet?mans={_month}");
        string cont = await response.Content.ReadAsStringAsync();
        int quantity = DateTime.DaysInMonth(DateTime.Now.Year, _month);
        List<PlaningList> planing = JsonConvert.DeserializeObject<List<PlaningList>>(cont)!.ToList();
        
    }

    private void Beack(object? sender, RoutedEventArgs e)
    {
        _month--;
        LoadData();
    }

    private void Necst(object? sender, RoutedEventArgs e)
    {
        _month++;
        LoadData();
    }
}

public class DoctorList{
    
    public int Id { get; set; }

    public string Namrdoctor { get; set; }
}

public class DirectionsList{
    
    public int Id { get; set; }

    public string Namedirection { get; set; }
}

public class PlaningList{
    
    public int Id { get; set; }

    public int? Doctor { get; set; }

    public DateTime Timeschedule { get; set; }

    public int? Direction { get; set; }
}