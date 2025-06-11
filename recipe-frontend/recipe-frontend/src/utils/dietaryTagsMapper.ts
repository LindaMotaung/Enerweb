import DietaryTags from '../features/recipes/enums/EDietaryTags';

const dietaryTagsMapper = (dietaryTagsString: string | string[]): string => {
  if (Array.isArray(dietaryTagsString)) {
    dietaryTagsString = dietaryTagsString[0];
  }
  if (typeof dietaryTagsString !== "string") {
    throw new Error(`Invalid dietary tag type: ${typeof dietaryTagsString}`);
  }
  switch (dietaryTagsString.toLowerCase()) {
    case 'keto':
      return 'keto';
    case 'vegan':
      return 'vegan';
    case 'lactose':
      return 'lactose';
    case 'atchaar':
      return 'atchaar';
    case 'spinach':
      return 'spinach';
    case 'lemon':
      return 'lemon';
    case 'sugar':
      return 'sugar';
    default:
      return dietaryTagsString; // Return the dietary tag as is
  }
};

  export default dietaryTagsMapper;