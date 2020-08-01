// Code generated by a template
// Project: Remember
// https://github.com/yiyungent/Remember
// Author: yiyun <yiyungent@gmail.com>
// LastUpadteTime: 2020-07-06 10:34:32
using Domain.Entities;
using Repositories.Core;
using Repositories.Interface;

namespace Repositories.Implement
{
    public partial class Follower_FollowedRepository : BaseRepository<Follower_Followed>, IFollower_FollowedRepository
    {
        private readonly RemDbContext _context;

        public Follower_FollowedRepository(RemDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}