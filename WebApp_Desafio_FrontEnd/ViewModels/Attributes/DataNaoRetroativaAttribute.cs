using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using WebApp_Desafio_FrontEnd.ViewModels.Interfaces;

namespace WebApp_Desafio_FrontEnd.ViewModels.Attributes
{
    public class DataNaoRetroativaAttribute : ValidationAttribute, IClientModelValidator
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = validationContext.ObjectInstance as IViewModel;
            if (model == null)
            {
                return new ValidationResult("Model is null");
            }

            // Ignora caso seja atualização
            if (model.ID != 0)
            {
                return ValidationResult.Success;
            }

            DateTime data;
            bool isDate = DateTime.TryParse(value.ToString(), out data);

            if (isDate && data >= DateTime.Now.Date)
            {
                // Data válida (não é retroativa)
                return ValidationResult.Success;
            }

            // Data é retroativa
            return new ValidationResult("A data não pode ser retroativa.");
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            var errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-datanaoretroativa", errorMessage);
        }

        private static bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
                return false;

            attributes.Add(key, value);

            return true;
        }
    }
}



