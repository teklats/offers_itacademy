using MediatR;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Admin.Categories.Commands.CreateCategory;

public class CreateCategoryCommand : IRequest<CategoryDto>
{
    public string Name { get; set; }
    public string Description { get; set; }

    public CreateCategoryCommand(string name, string description)
    {
        Name = name;
        Description = description;
    }
}