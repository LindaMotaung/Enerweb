import DietaryTags from '../features/recipes/enums/EDietaryTags';

const dietaryTagsMapper = (dietaryTagsString: string | string[]): number => {
  if (Array.isArray(dietaryTagsString)) {
    dietaryTagsString = dietaryTagsString[0];
  }
  
  if (typeof dietaryTagsString !== "string") {
    throw new Error(`Invalid dietary tag type: ${typeof dietaryTagsString}`);
  }

    switch (dietaryTagsString.toLowerCase()) {
      case 'keto':
        return DietaryTags.Keto;
      case 'vegan':
        return DietaryTags.Vegan;
      case 'lactose':
        return DietaryTags.lactose;
      case 'Lactose':
        return DietaryTags.lactose;
      case 'atchaar':
        return DietaryTags.Atchaar;
      case 'spinach':
        return DietaryTags.Spinach;
      case 'lemon':
        return DietaryTags.Lemon; 
      case 'sugar':
        return DietaryTags.Sugar; 
      default:
        throw new Error(`Invalid dietary tag: ${dietaryTagsString}`);
    }
  };

  export default dietaryTagsMapper;