using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenHouseWebApi.Dto;
using GreenHouseWebApi.Model;
using GreenHouseWebApi.Repository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace GreenHouseWebApi.Controllers
{
    [Route("api/[controller]")]
    public class FoodController : Controller
    {
        private readonly IFoodRepository _foodRepository;

        public FoodController(IFoodRepository foodRepository)
        {
            _foodRepository = foodRepository;
        }

        [HttpGet]
        public IActionResult GetAllFoodItems()
        {
            ICollection<FoodItem> foodItems = _foodRepository.GetAll();

            var mappedItems = foodItems.Select(x => AutoMapper.Mapper.Map<FoodItemDto>(x));

            return Ok(mappedItems);
        }

        [HttpGet("{id:int}", Name = "GetSingleFoodItem")]
        public IActionResult GetSingleFoodItem(int id)
        {
            var foodItem = _foodRepository.GetSingle(id);

            if (foodItem == null)
            {
                return NotFound();
            }

            return Ok(AutoMapper.Mapper.Map<FoodItemDto>(foodItem));
        }

        [HttpPost]
        public IActionResult AddNewFoodItem([FromBody] FoodItemDto foodItemDto)
        {
            if (foodItemDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FoodItem foodItem = _foodRepository.Add(AutoMapper.Mapper.Map<FoodItem>(foodItemDto));

            return CreatedAtRoute("GetSingleFoodItem", new {id = foodItem.Id}, AutoMapper.Mapper.Map<FoodItemDto>(foodItem));
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateFoodItem(int id, [FromBody] FoodItemDto dto)
        {
            var foodItemToCheck = _foodRepository.GetSingle(id);
            if (foodItemToCheck == null)
            {
                return NotFound();
            }
            if (id != dto.Id)
            {
                return BadRequest("ids do not match");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedFoodItem = _foodRepository.Update(id, AutoMapper.Mapper.Map<FoodItem>(dto));
            return Ok(AutoMapper.Mapper.Map<FoodItemDto>(updatedFoodItem));
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartialUpdate(int id, [FromBody] JsonPatchDocument<FoodItemDto> foodPatchDocument )
        {
            if (foodPatchDocument == null)
            {
                return BadRequest();
            }
            FoodItem existingFoodItem = _foodRepository.GetSingle(id);

            if (existingFoodItem == null)
            {
                return NotFound();
            }

            FoodItemDto foodDto = AutoMapper.Mapper.Map<FoodItemDto>(existingFoodItem);

            foodPatchDocument.ApplyTo(foodDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedItem = _foodRepository.Update(id, AutoMapper.Mapper.Map<FoodItem>(foodDto));

            return Ok(AutoMapper.Mapper.Map<FoodItemDto>(updatedItem));
        }

        [HttpDelete("{id:int}")]
        public IActionResult Remove(int id)
        {
            var existingFoodItem = _foodRepository.GetSingle(id);
            if (existingFoodItem == null)
            {
                NotFound();
            }

            _foodRepository.Delete(id);

            return NoContent();
        }
    }
}
