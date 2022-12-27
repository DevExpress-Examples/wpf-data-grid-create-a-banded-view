using System.Collections.ObjectModel;
using System.Windows;

namespace BandedViewSample {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            grid.ItemsSource = Vehicles.GetVehicles();
        }
    }
    public class Vehicle {
        public string Trademark { get; set; }
        public string Model { get; set; }
        public string Modification { get; set; }
        public int Doors { get; set; }
        public int Cyl { get; set; }
        public int MPGCity { get; set; }
        public int MPGHighway { get; set; }
        public string Transmission { get; set; }
        public int Gears { get; set; }
        public string Description { get; set; }
    }
    public class Vehicles {
        public static ObservableCollection<Vehicle> GetVehicles() {
            var vehicles = new ObservableCollection<Vehicle>();
            vehicles.Add(new Vehicle() { Trademark = "Honda", Model = "Crosstour", Modification = "EX 2WD 2.4L I4 5A", Doors = 4, Cyl = 4, MPGCity = 22, MPGHighway = 31, Transmission = "Automatic", Gears = 5, Description = "The Crosstour (initially branded the Accord Crosstour) is a full-size crossover SUV manufactured by Japanese automaker Honda. Sales began in November 2009 for the 2010 model year." });
            vehicles.Add(new Vehicle() { Trademark = "Ford", Model = "Edge", Modification = "SEL FWD 3.5L V6 6A", Doors = 4, Cyl = 6, MPGCity = 19, MPGHighway = 27, Transmission = "Automatic", Gears = 6, Description = "The Ford Edge is a mid-size crossover SUV (CUV) manufactured by Ford, based on the Ford CD3 platform shared with the Mazda CX-9." });
            return vehicles;
        }
    }
}
