using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcoSystemAPI.Context.Models;
using EcoSystemAPI.Core.Dtos;
using EcoSystemAPI.uow.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EcoSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchandiseController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMerchandiseRepo _repository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public MerchandiseController(IMerchandiseRepo repository, IConfiguration configuration, IMapper mapper, IWebHostEnvironment env)
        {
            _repository = repository;
            _configuration = configuration;
            _mapper = mapper;
            _env = env;
        }

        [HttpGet]
        public ActionResult<IEnumerable<MerchandiseReadDto>> GetAllMerch()
        {
            var merchItems = _repository.GetAllMerch();
            return Ok(_mapper.Map<IEnumerable<MerchandiseReadDto>>(merchItems));
        }

        [HttpGet("{id}", Name = "GetMerchById")]
        public ActionResult<IEnumerable<MerchandiseReadDto>> GetMerchById(int id)
        {
            var merchItem = _repository.GetMerchById(id);
            if (merchItem != null)
            {
                return Ok(_mapper.Map<MerchandiseReadDto>(merchItem));
            }
            else
                return NotFound();

        }

        [HttpPost]
        [Authorize]
        public ActionResult<MerchandiseReadDto> CreateMerch(MerchandiseCreateDto merchandiseCreateDto)
        {
            var merchModel = _mapper.Map<Merchandise>(merchandiseCreateDto);
            _repository.CreateMerch(merchModel);
            _repository.SaveChanges();

            var merchReadDto = _mapper.Map<MerchandiseReadDto>(merchModel);
            return CreatedAtRoute(nameof(GetMerchById), new { Id = merchReadDto.MerchId }, merchReadDto);

        }

        [HttpPut("{id}")]
        [Authorize]

        public ActionResult UpdateMerch(int id, MerchandiseUpdateDto merchandiseUpdateDto)
        {
            var merchModelFromRepo = _repository.GetMerchById(id);

            if (merchModelFromRepo == null)
            {
                return NotFound();
            }
            _mapper.Map(merchandiseUpdateDto, merchModelFromRepo);

            _repository.UpdateMerch(merchModelFromRepo);

            _repository.SaveChanges();

            return NoContent();

        }

        [HttpPatch("{id}")]
        [Authorize]

        public ActionResult PartialAccountUpdate(int id, JsonPatchDocument<MerchandiseUpdateDto> patchDoc)
        {
            var merchModelFromRepo = _repository.GetMerchById(id);
            if (merchModelFromRepo == null)
            {
                return NotFound();
            }

            var merchToPatch = _mapper.Map<MerchandiseUpdateDto>(merchModelFromRepo);
            patchDoc.ApplyTo(merchToPatch, ModelState);
            if (!TryValidateModel(merchToPatch))
            {
                return ValidationProblem();
            }
            _mapper.Map(merchToPatch, merchModelFromRepo);
            _repository.UpdateMerch(merchModelFromRepo);
            _repository.SaveChanges();
            return NoContent();

        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult DeleteMerch(int id)
        {
            var photoFolderPath = _env.ContentRootPath + "/Photos/";
            var merchModelFromRepo = _repository.GetMerchById(id);
            if (merchModelFromRepo == null)
            {
                return NotFound();
            }

            _repository.DeleteMerch(merchModelFromRepo);
            System.IO.File.Delete(photoFolderPath + merchModelFromRepo.MerchImage);
            _repository.SaveChanges();
            return NoContent();

        }

        [Route("SaveFile")]
        [HttpPost]
        [Authorize]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;
                var filextn = Path.GetExtension(physicalPath); //Extension
                var newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + filextn;
                var newFilePath = _env.ContentRootPath + "/Photos/" + newFileName;

                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(newFileName);
            }
            catch (Exception)
            {
                return new JsonResult("Anonymous.png");
            }
        }

    }
}
