using DiplomaDataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OptionsWebSite.Models.CustomValidation
{
    public class CheckValidOption : ValidationAttribute
    {
        private OptionPickerContext db = new OptionPickerContext();

        private readonly string _option;

        public CheckValidOption(string option) : base("{0} cannot be a duplicate option.")
        {
            _option = option;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {   
            if (value != null)
            {
                var q = from y in db.Options
                        where y.Title == (string)value
                        select y;
                var t = q.FirstOrDefault();
                if(t.IsActive == false)
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}