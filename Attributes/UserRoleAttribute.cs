using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZooManager.Attributes
{
    public class UserRoleAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        { 
            RoleManager<IdentityRole> manager = (RoleManager<IdentityRole>)context.GetService(typeof(RoleManager<IdentityRole>));

            bool isValid = value is IEnumerable ? TestForArray(value as IEnumerable, manager) : TestForSingleValue(value, manager);

            return isValid ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }

        private bool TestForSingleValue(object value, RoleManager<IdentityRole> roleManager)
        {
            var task = roleManager.FindByNameAsync(value.ToString());
            task.Wait();
            return task.Result != null;
        }

        private bool TestForArray(IEnumerable value, RoleManager<IdentityRole> roleManager)
        {
            bool result = true;
            foreach (var item in value)
            {
                var task = roleManager.FindByNameAsync(item.ToString());
                task.Wait();

                if (task.Result == null)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }
    }
}
