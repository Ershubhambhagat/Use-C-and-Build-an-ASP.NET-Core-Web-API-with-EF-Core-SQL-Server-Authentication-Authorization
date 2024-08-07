﻿using NZWalk_API.Model.Domain;

namespace NZWalk_API.Repositories.Walk_Repository.Interface
{
    public interface IWalkRepository
    {
        Task<Walk> CreateWalkAsync(Walk walk);
        Task<Walk?> GetWalkById(Guid Id);
        Task<List<Walk>> GetAllWalkAsync(string? FilterOn=null,string? FilteQuary=null, string? sortBy=null, bool isAscending=true, int pageNumber = 1, int pageSize = 1000);
        Task<Walk?> UpdateWAlkAsync(Guid Id,Walk walk);
        Task<Walk?> DeleteAsync(Guid Id);
        Task SaveWalk();
    }
}



