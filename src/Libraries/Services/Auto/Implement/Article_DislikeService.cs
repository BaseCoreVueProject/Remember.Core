// Code generated by a template
// Project: remember
// https://github.com/yiyungent/remember
// Author: yiyun <yiyungent@gmail.com>
// LastUpadteTime: 2020-07-06 10:36:54
using Domain.Entities;
using Repositories.Interface;
using Services.Core;
using Services.Interface;

namespace Services.Implement
{
    public partial class Article_DislikeService : BaseService<Article_Dislike>, IArticle_DislikeService
    {
        private readonly IArticle_DislikeRepository _repository;
        public Article_DislikeService(IArticle_DislikeRepository repository) : base(repository)
        {
            this._repository = repository;
        }
    }
}