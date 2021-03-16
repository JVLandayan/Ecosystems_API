using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcoSystemAPI.uow.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;   
using Microsoft.AspNetCore.Hosting;
using EcoSystemAPI.Core.Dtos;
using EcoSystemAPI.Context.Models;

namespace EcoSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ITeamsRepo _repository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public TeamsController(ITeamsRepo repository, IConfiguration configuration, IMapper mapper, IWebHostEnvironment env)
        {
            _repository = repository;
            _configuration = configuration;
            _mapper = mapper;
            _env = env;
        }

        [HttpGet]

        public ActionResult<IEnumerable<TeamsReadDto>> GetAllTeams()
        {
            var teamMembers= _repository.GetAllTeams();
            return Ok(_mapper.Map<IEnumerable<TeamsReadDto>>(teamMembers));
            
        }


        [HttpGet("{id}", Name = "GetTeamById")]
        public ActionResult<IEnumerable<TeamsReadDto>> GetTeamById(int id)
        {
            var teamMember = _repository.GetTeamById(id);
            if (teamMember != null)
            {
                return Ok(_mapper.Map<TeamsReadDto>(teamMember));
            }
            else
                return NotFound();

        }

        [Authorize]
        [HttpPost]
        public ActionResult<TeamsReadDto> CreateTeam(TeamsCreateDto teamsCreateDto)
        {
            var teamsModel = _mapper.Map<Teams>(teamsCreateDto);
            _repository.CreateTeam(teamsModel);
            _repository.SaveChanges();

            var teamsReadDto = _mapper.Map<TeamsReadDto>(teamsModel);
            return CreatedAtRoute(nameof(GetTeamById), new { Id = teamsReadDto.TeamsId }, teamsReadDto);

        }

        [Authorize]
        [HttpPut("{id}")]

        public ActionResult UpdateTeam(int id, TeamsUpdateDto teamsUpdateDto)
        {
            var teamsModelFromRepo = _repository.GetTeamById(id);

            if (teamsModelFromRepo == null)
            {
                return NotFound();
            }
            _mapper.Map(teamsUpdateDto, teamsModelFromRepo);
            _repository.UpdateTeam(teamsModelFromRepo);
            _repository.SaveChanges();

            return NoContent();

        }
        [Authorize]
        [HttpPatch("{id}")]

        public ActionResult PartialTeamsUpdate(int id, JsonPatchDocument<TeamsUpdateDto> patchDoc)
        {
            var teamModelFromRepo = _repository.GetTeamById(id);
            if (teamModelFromRepo == null)
            {
                return NotFound();
            }

            var teamToPatch = _mapper.Map<TeamsUpdateDto>(teamModelFromRepo);
            patchDoc.ApplyTo(teamToPatch, ModelState);
            if (!TryValidateModel(teamToPatch))
            {
                return ValidationProblem();
            }
            _mapper.Map(teamToPatch, teamModelFromRepo);
            _repository.UpdateTeam(teamModelFromRepo);
            _repository.SaveChanges();
            return NoContent();

        }
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult DeleteMerch(int id)
        {
            var photoFolderPath = _env.ContentRootPath + "/Photos/";
            var teamModelFromRepo = _repository.GetTeamById(id);
            if (teamModelFromRepo == null)
            {
                return NotFound();
            }

            _repository.DeleteTeam(teamModelFromRepo);
            System.IO.File.Delete(photoFolderPath + teamModelFromRepo.TeamsImage);
            _repository.SaveChanges();
            return NoContent();
        }
        [Authorize]
        [Route("SaveFile")]
        [HttpPost]
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
