using Application.Common.Models;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Ingredients.Commands.CreateIngredient;

/// <summary>
/// Handler for creating a new ingredient
/// </summary>
public class CreateIngredientCommandHandler : IRequestHandler<CreateIngredientCommand, OperationResult<IngredientDto>>
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IMapper _mapper;

    public CreateIngredientCommandHandler(
        IIngredientRepository ingredientRepository,
        IMapper mapper)
    {
        _ingredientRepository = ingredientRepository;
        _mapper = mapper;
    }

    public async Task<OperationResult<IngredientDto>> Handle(CreateIngredientCommand request, CancellationToken cancellationToken)
    {
        // Check if ingredient already exists
        var existingIngredient = await _ingredientRepository.GetByNameAsync(request.Name);
        if (existingIngredient != null)
        {
            return OperationResult<IngredientDto>.Failure($"Ingredient '{request.Name}' already exists");
        }

        // Create new ingredient
        var ingredient = new Ingredient
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Unit = request.Unit,
            CreatedAt = DateTime.UtcNow,
        };

        await _ingredientRepository.AddAsync(ingredient);
        await _ingredientRepository.SaveChangesAsync();

        var ingredientDto = _mapper.Map<IngredientDto>(ingredient);

        return OperationResult<IngredientDto>.Success(ingredientDto);
    }
}