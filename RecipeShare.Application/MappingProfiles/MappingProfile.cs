using AutoMapper;
using RecipeShare.Application.Models;
using RecipeShare.Domain;

namespace RecipeShare.Application.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Recipe, RecipeDto>()
                .ForMember(dest => dest.DietaryTags, opt =>
                    opt.MapFrom(src => src.RecipeDietaryTags.Select(rdt => rdt.DietaryTag.Name)));

            CreateMap<AddRecipesRequest, Recipe>();
        }
    }
}
