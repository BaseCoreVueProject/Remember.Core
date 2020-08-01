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
    public partial class Article_ParticipantService : BaseService<Article_Participant>, IArticle_ParticipantService
    {
        private readonly IArticle_ParticipantRepository _repository;
        public Article_ParticipantService(IArticle_ParticipantRepository repository) : base(repository)
        {
            this._repository = repository;
        }
    }
}
