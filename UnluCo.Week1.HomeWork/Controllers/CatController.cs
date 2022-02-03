﻿using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnluCo.Week1.HomeWork.CatOperations.CreateCat;
using UnluCo.Week1.HomeWork.CatOperations.DeleteCat;
using UnluCo.Week1.HomeWork.CatOperations.GetByIdCats;
using UnluCo.Week1.HomeWork.CatOperations.GetCats;
using UnluCo.Week1.HomeWork.CatOperations.OrderByCat;
using UnluCo.Week1.HomeWork.CatOperations.PostCat;
using UnluCo.Week1.HomeWork.CatOperations.UpdateCat;
using UnluCo.Week1.HomeWork.DBOperations;
using static UnluCo.Week1.HomeWork.CatOperations.CreateCat.AddCatCommand;


namespace UnluCo.Week1.HomeWork.Controllers
{
    /* [Authorize(Roles = "Admin")]*/
    [Authorize]
    [Route("[controller]s")]
    [ApiController]
    public class CatController : ControllerBase
    {
        private readonly CatStoreDbContext _context;  //Database'den verileri çekmek için
        private readonly IMapper _mapper;
        public CatController(CatStoreDbContext context, IMapper mapper) //context ve mapper contrellera entegre edildi
        {
            _context = context;
            _mapper = mapper;
        }


        
        [HttpGet("all")] /*Bütün listenin çağrıldığı durum*/

        public IActionResult GetCats()
        {
            GetCatsQuery query = new GetCatsQuery(_context, _mapper);
            var result = query.Handle();
            return Ok(result);
        }

        [HttpGet("{id}/catswithId")]  /*Bütün listenin id ile çağrıldığı durum*/

        public IActionResult GetById(int id)
        {
            ByIdCatsViewModel result;

            GetByIdCatsQuery query = new GetByIdCatsQuery(_context, _mapper);
            query.CatId = id;
            GetByIdCatsQueryValidator validator = new GetByIdCatsQueryValidator();
            validator.ValidateAndThrow(query);
            result = query.Handle();

            return Ok(result);

        }

        [HttpPost("addNewCat")] /*Listeye yeni bir elemanın eklendiği komut.*/

        public IActionResult AddCat([FromForm] AddCatModel newCat)
        {
            AddCatCommand command = new AddCatCommand(_context, _mapper);


            command.Model = newCat;
            AddCatCommandValidator validator = new AddCatCommandValidator();
            validator.ValidateAndThrow(command);
            command.Handle();

            return Ok();

        }

        [HttpPut("{id}/updateCatInformations")] /*Id'si ile çağırdığımız elemanın bilgilerini değiştirebileceğimiz komut.*/

        public IActionResult UpdateCat(int id, [FromForm] UpdateCatModel updateCat)
        {
            UpdateCatCommand command = new UpdateCatCommand(_context);

            command.CatId = id;
            command.Model = updateCat;
            UpdateCatCommandValidator validator = new UpdateCatCommandValidator();
            validator.ValidateAndThrow(command);
            command.Handle();

            return Ok();
        }
        [HttpPatch("{id}/updateCatName")] /*Elemanın sadece bir ögesinin(burada Name değiştiriliyor)değiştirildiği komut */
        public IActionResult UpdateCatAd(int id, [FromBody] UpdateCatNameModel updateCatName)
        {
            UpdateCatNameCommand nameCommand = new UpdateCatNameCommand(_context);

            nameCommand.CatId = id;
            nameCommand.Model = updateCatName;
            UpdateCatNameCommandValidator validator = new UpdateCatNameCommandValidator();
            validator.ValidateAndThrow(nameCommand);
            nameCommand.Handle();

            return Ok();
        }


        [HttpDelete("{id}deleteCat")] /*Id'si girilen elemanın listeden silinmesini sağlayan komut*/

        public IActionResult DeleteCat(int id)
        {
            DeleteCatCommand command = new DeleteCatCommand(_context);

            command.CatId = id;
            DeleteCatCommandValidator validator = new DeleteCatCommandValidator();
            validator.ValidateAndThrow(command);
            command.Handle();

            return Ok();
        }

        [HttpGet("List/{colon}/{orderby}")]  /*Elemanları sıralayan komut.(Burada name'ler a-z'ye olmak üzere sıralanıyor)*/
        /*OrderBy() bir listeyi sıralamak için kullanılır.*/
        public IActionResult GetCatList(string colon = "Name", string orderby = "asc")
        {
            OrderByCatQuery byCatQuery = new OrderByCatQuery(_context, _mapper);
            OrderByCatQueryValidator validator = new OrderByCatQueryValidator();

            byCatQuery.colon = colon;
            byCatQuery.orderby = orderby;
            validator.ValidateAndThrow(byCatQuery);
            var result = byCatQuery.Handle();

            return Ok(result);

        }

    }
}
