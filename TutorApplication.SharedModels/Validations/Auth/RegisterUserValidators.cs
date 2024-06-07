﻿using FluentValidation;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorApplication.SharedModels.Requests;

namespace TutorApplication.SharedModels.Validations.Auth
{
	public class RegisterUserValidators:AbstractValidator<RegisterUserRequest>
	{
		public RegisterUserValidators()
		{
			RuleFor(x => x.Email)
		   .EmailAddress().WithMessage("Should be an Email")
		   .NotEmpty().WithMessage("Email should not be empty.")
		   .NotNull().WithMessage("Email should not be null.");


			RuleFor(x => x.Password)
			.Equal(x=>x.Verify).WithMessage("Passwords do not match each other")
		   .NotEmpty().WithMessage("Passwords should not be empty.")
		   .NotNull().WithMessage("Passwords should not be null.");


		}
	}
}
