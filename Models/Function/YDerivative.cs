using System;
using System.Collections.Generic;
using System.Text;
using CompMathLibrary.KoshiTask.RungeKutt;
using CompMathLibrary.Vectors;

namespace KantorLr14.Models.Function
{
	public class YDerivative : IDerivativeFunction
	{
		public double Calculate(double x, Vector derivativeArgs)
		{
			return derivativeArgs[0];
		}
	}
}
