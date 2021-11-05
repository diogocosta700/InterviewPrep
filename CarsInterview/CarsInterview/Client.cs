using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarsInterview
{
    public class Client : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Car> Cars { get; set;}
    }
}