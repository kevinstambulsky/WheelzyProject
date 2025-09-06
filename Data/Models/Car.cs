namespace WheelzyProject.Data.Models
{
    public class Car
    {
        public int CarId { get; set; }
        public short Year { get; set; }
        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public int? SubmodelId { get; set; }


        public CarMake Make { get; set; } = null!;
        public CarModel Model { get; set; } = null!;
        public CarSubmodel? Submodel { get; set; }
    }
}
