using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace CarsInterview.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarController : ControllerBase
    {
        /*
        * THE LICENSE PLATE FOR A CAR SHOULD BE UNIQUE. THIS CONSTRAINT SHOULD BE GUARANTEED BY THE CODE IN THIS FILE
        */
        
        /*
        * A CLIENT MUST BE ABLE TO RENT A CAR FOR A CERTAIN PERIOD OF TIME, AND SHOULD BE ABLE TO RENT THE SAME CAR FOR DIFFERENT TIME PERIODS
        */
        
        private readonly ILogger<CarController> _logger;
        private readonly CarContext _carDbContext;

        public CarController(ILogger<CarController> logger, CarContext carDbContext)
        {
            _logger = logger;
            _carDbContext = carDbContext;
        }
        
        #region GET
        [HttpGet("/{licensePlate}")]
        private ActionResult<Car> DecentGet(string licensePlate)
        {
            try
            {
                return Ok(_carDbContext.Cars.Single(car => car.LicensePlate.ToUpper().Equals(licensePlate.ToUpper())));
            }
            catch (InvalidOperationException e)
            {
                return NotFound();
            }
        }
        #endregion
        
        
        //[HttpGet("/{licensePlate}")]
        private ActionResult<Car> Get(string licensePlate)
        {
            //In case license plate doesn't exist single will throw InvalidOperationException
            return _carDbContext.Cars.Single(car => car.LicensePlate.ToUpper().Equals(licensePlate.ToUpper()));
        }
        
        [HttpGet]
        public ActionResult<IList<Car>> Get()
        {
            return Ok(_carDbContext.Cars.ToList());
        }
        
        #region SurpriseRegion
        [HttpPut("/{licensePlate}")]
        //Not all exceptions are handled since that's not the point of the exercise
        public ActionResult<CarDto> DecentPut(string licensePlate, [FromBody] CarDto carDto)
        {
            try
            {
                Car car = _carDbContext.Cars.Single(car => car.LicensePlate.ToUpper().Equals(licensePlate.ToUpper()));
                car.Brand = carDto.Brand;
                car.Color = carDto.Color;
                SaveToDatabase();
                return Ok(carDto);
            }
            catch (InvalidOperationException e)
            {
                Car newCar = new Car()
                {
                    LicensePlate = licensePlate,
                    Brand = carDto.Brand,
                    Color = carDto.Color
                };
            
                AddToDatabase(newCar);
                SaveToDatabase();
                return Created($"/{newCar.LicensePlate}", carDto);
            }
        }
        #endregion
        
        #region PUTCREATE
        //[HttpPut("/{licensePlate}")]
        private ActionResult<CarDto> PutAdd(string licensePlate, [FromBody] CarDto carDto)
        {
            Car newCar = new Car()
            {
                LicensePlate = licensePlate,
                Brand = carDto.Brand,
                Color = carDto.Color
            };
            
            AddToDatabase(newCar);
            SaveToDatabase();

            CarDto carDtoResult = new CarDto()
            {
                LicensePlate = newCar.LicensePlate,
                Brand = newCar.Brand,
                Color = newCar.Color
            };
            return Created($"/{newCar.LicensePlate}", carDtoResult);
        }
        #endregion
        
        [HttpPut("client/{id}")]
        private ActionResult<Client> PutClient(int id, [FromBody] Client client)
        {
            try
            {
                Client updatedClient = _carDbContext.Clients.Find(id);
                updatedClient.Name = client.Name;
                SaveToDatabase();
                return Ok(updatedClient);
            }
            catch (InvalidOperationException e)
            {
                Client newClient = new Client()
                {
                    Id = id,
                    Name = client.Name
                };

                _carDbContext.Clients.Add(newClient);
                SaveToDatabase();
                return Created($"client/{id}", newClient);
            }
        }
        
        private EntityEntry<Car> AddToDatabase(Car car)
        {
            return _carDbContext.Cars.Add(car);
        }
        
        private int SaveToDatabase()
        {
            return _carDbContext.SaveChanges();
        }
    }
}