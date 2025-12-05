using Application.Common.Models;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Ingredients.Queries.GetIngredientById;

/// <summary>
/// Handler for getting a single ingredient by ID
/// </summary>
public class GetIngredientByIdQueryHandler : IRequestHandler<GetIngredientByIdQuery, OperationResult<IngredientDto>>
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IMapper _mapper;

    public GetIngredientByIdQueryHandler(IIngredientRepository ingredientRepository, IMapper mapper)
    {
        _ingredientRepository = ingredientRepository;
        _mapper = mapper;
    }

    public async Task<OperationResult<IngredientDto>> Handle(GetIngredientByIdQuery request, CancellationToken cancellationToken)
    {
        var ingredient = await _ingredientRepository.GetByIdAsync(request.Id);

        if (ingredient == null)
        {
            return OperationResult<IngredientDto>.Failure("Ingredient not found");
        }

        var ingredientDto = _mapper.Map<IngredientDto>(ingredient);

        return OperationResult<IngredientDto>.Success(ingredientDto);
    }
}