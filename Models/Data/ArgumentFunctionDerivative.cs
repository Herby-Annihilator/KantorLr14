using System;
using System.Collections.Generic;
using System.Text;

namespace KantorLr14.Models.Data
{
	public class ArgumentFunctionDerivative
	{
		public double Argument { get; set; }
		public double FunctionValue { get; set; }
		public double DerivativeValue { get; set; }

		public ArgumentFunctionDerivative(double argument, double functionValue, double derivativeValue)
		{
			Argument = argument;
			FunctionValue = functionValue;
			DerivativeValue = derivativeValue;
		}
	}
}
