using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Cantin.Service.Extensions
{
	public static class ValidationExtension
	{
		public static ModelStateDictionary AddErrorsToModelState(this ValidationResult result,ModelStateDictionary modelState) {
            foreach (var error in result.Errors)
            {
                modelState.AddModelError(error.PropertyName,error.ErrorMessage);
            }
            return modelState;
        }
	}
}
