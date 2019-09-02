using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Webapi.Core.Template.Models
{
    public class ValidationResult
    {
        public List<string> Errors { get; }

        public ValidationResult(ModelStateDictionary modelState)
        {
            Errors = modelState.Keys.SelectMany(key => modelState[key].Errors.Select(x => x.ErrorMessage)).ToList();
        }
    }
}
