// // Copyright (C) TBC Bank. All Rights Reserved.
//
// using AutoMapper;
// using MediatR;
// using OffersPlatform.Application.Common.Interfaces.IRepositories;
// using OffersPlatform.Application.DTOs;
//
// namespace OffersPlatform.Application.Features.Companies.Offers.Queries.GetCategoryByName
// {
//     public class GetCategoryByNameQueryHandler : IRequestHandler<GetCategoryByNameQuery, CategoryDto>
//     {
//         private readonly ICategoryRepository _categoryRepository;
//         private readonly IMapper _mapper;
//
//         public GetCategoryByNameQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
//         {
//             _categoryRepository = categoryRepository;
//             _mapper = mapper;
//         }
//
//         public async Task<CategoryDto> Handle(GetCategoryByNameQuery request, CancellationToken cancellationToken)
//         {
//             var category = await _categoryRepository
//                 .GetByNameAsync(request.CategoryName, cancellationToken)
//                 .ConfigureAwait(false);
//             return _mapper.Map<CategoryDto>(category);
//         }
//
//     }
// }
