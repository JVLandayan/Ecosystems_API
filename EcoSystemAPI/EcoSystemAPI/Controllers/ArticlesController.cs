using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcoSystemAPI.uow.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net;
using EcoSystemAPI.Context.Models;
using EcoSystemAPI.Core.Dtos;
using Microsoft.AspNetCore.JsonPatch;

namespace EcoSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IArticlesRepo _repository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;


        public ArticlesController(IArticlesRepo repository, IConfiguration configuration, IMapper mapper, IWebHostEnvironment env)
        {
            _repository = repository;
            _configuration = configuration;
            _mapper = mapper;
            _env = env;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ArticleReadDto>> GetAllArticles()
        {
            var article = _repository.GetAllArticles();
            return Ok(_mapper.Map<IEnumerable<ArticleReadDto>>(article));
        }

        [HttpGet("{id}", Name = "GetArticleById")]
        public ActionResult<IEnumerable<ArticleReadDto>> GetArticleById(int id)
        {
            var article = _repository.GetArticleById(id);
            
            if (article == null)
            {
                return NotFound(new { message = "Article doesn't exist" });
            }

            var convertedData = new Article
            {
                Id = article.Id,
                Image = article.Image,
                AuthorImg = article.AuthorImg,
                AuthorName = article.AuthorName,
                Content = WebUtility.HtmlDecode(article.Content).ToString(),
                DateofPublish = article.DateofPublish,
                Name = article.Name,
                ContentIntro = article.ContentIntro,
                AuthorId = article.AuthorId
            };
            if (article != null)
            {
                return Ok(_mapper.Map<ArticleReadDto>(convertedData));
            }
            else
                return NotFound();

        }

        [HttpPost]
        [Authorize]
        public ActionResult<ArticleReadDto> CreateArticle(ArticleCreateDto articleCreateDto)
        {
            var convertedData = new ArticleCreateDto
            {
                AuthorImg = articleCreateDto.AuthorImg,
                Content = WebUtility.HtmlEncode(articleCreateDto.Content),
                AuthorName = articleCreateDto.AuthorName,
                Image = articleCreateDto.Image,
                DateofPublish = DateTime.Now.ToString("dd/MM/yy"),
                Name = articleCreateDto.Name,
                ContentIntro = articleCreateDto.ContentIntro,
                AuthorId = articleCreateDto.AuthorId
            };
            var articleModel = _mapper.Map<Article>(convertedData);
            _repository.CreateArticle(articleModel);
            _repository.SaveChanges();

            var articleReadDto = _mapper.Map<ArticleReadDto>(articleModel);
            return CreatedAtRoute(nameof(GetArticleById), new { Id = articleReadDto.Id}, articleReadDto);
        }

        [HttpPut("{id}")]

        public ActionResult UpdateArticle(int id, ArticleUpdateDto articleUpdateDto)
        {
            var articleModelFromRepo = _repository.GetArticleById(id);

            var processedData = new ArticleCreateDto
            {
                Image = articleUpdateDto.Image,
                Content = WebUtility.HtmlEncode(articleUpdateDto.Content),
                Name = articleUpdateDto.Name,
                ContentIntro = articleUpdateDto.ContentIntro,
                AuthorImg = articleModelFromRepo.AuthorImg,
                AuthorName = articleModelFromRepo.AuthorName,
                DateofPublish = articleModelFromRepo.DateofPublish,
                AuthorId = articleModelFromRepo.AuthorId
            };

            if (articleModelFromRepo == null)
            {
                return NotFound();
            }
            _mapper.Map(processedData, articleModelFromRepo);
            _repository.UpdateArticle(articleModelFromRepo);
            _repository.SaveChanges();

            return Ok(articleModelFromRepo);

        }

        [HttpPatch("{id}")]
        public ActionResult PartialArticleUpdate(int id, JsonPatchDocument<ArticleUpdateDto> patchDoc)
        {
            
            var articleModelFromRepo = _repository.GetArticleById(id);
            if (articleModelFromRepo == null)
            {
                return NotFound();
            }
            var articleToPatch = _mapper.Map<ArticleUpdateDto>(articleModelFromRepo);
            patchDoc.ApplyTo(articleToPatch, ModelState);


            if (!TryValidateModel(articleToPatch))
            {
                return ValidationProblem();
            }
            _mapper.Map(articleToPatch, articleModelFromRepo);
            _repository.UpdateArticle(articleModelFromRepo);
            _repository.SaveChanges();
            return NoContent();        
                       

        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult DeleteArticle(int id)
        {
            var articleModelFromRepo = _repository.GetArticleById(id);
            if (articleModelFromRepo == null)
            {
                return NotFound();
            }

            _repository.DeleteArticle(articleModelFromRepo);
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

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("Anonymous.png");
            }
        }

    }
}
