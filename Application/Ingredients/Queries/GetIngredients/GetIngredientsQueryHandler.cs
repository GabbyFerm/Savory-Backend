using Application.Common.Models;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Ingredients.Queries.GetIngredients;

/// <summary>
/// Handler for getting all ingredients with optional search
/// </summary>
public class GetIngredientsQueryHandler : IRequestHandler<GetIngredientsQuery, OperationResult<List<IngredientDto>>>
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IMapper _mapper;

    public GetIngredientsQueryHandler(IIngredientRepository ingredientRepository, IMapper mapper)
    {
        _ingredientRepository = ingredientRepository;
        _mapper = mapper;
    }

    public async Task<OperationResult<List<IngredientDto>>> Handle(GetIngredientsQuery request, CancellationToken cancellationToken)
    {
        // Get all ingredients
        var ingredients = await _ingredientRepository.ListAsync();

        // Filter by search term if provided
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var lowerSearchTerm = request.SearchTerm.ToLower();
            ingredients = ingredients.Where(i => i.Name.ToLower().Contains(lowerSearchTerm)).ToList();
        }

        var ingredientDtos = _mapper.Map<List<IngredientDto>>(ingredients);

        return OperationResult<List<IngredientDto>>.Success(ingredientDtos);
    }
}