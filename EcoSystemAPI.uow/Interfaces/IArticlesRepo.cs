
using EcoSystemAPI.Context.Models;

using System;
using System.Collections.Generic;
using System.Text;

namespace EcoSystemAPI.uow.Interfaces
{
    public interface IArticlesRepo
    {
        bool SaveChanges();
        IEnumerable<Article> GetAllArticles();
        Article GetArticleById(int id);
        void CreateArticle(Article art);
        void UpdateArticle(Article art);
        void DeleteArticle(Article art);
        void PartialArticleUpdate(Article art);
    }
}
