using System;
using System.Collections.Generic;
using System.Text;
using CompMathLibrary.KoshiTask.RungeKutt;
using CompMathLibrary.Vectors;

namespace KantorLr14.Models.Function
{
	public class VDerivative : IDerivativeFunction
	{
		private double _alpha;
		private double _beta;
		private double _ro;
		private double _teta;

		public VDerivative(double alpha, double beta, double ro, double teta)
		{
			_alpha = alpha;
			_beta = beta;
			_ro = ro;
			_teta = teta;
		}

		public double Calculate(double x, Vector derivativeArgs)
		{
			double result = (_beta * derivativeArgs[0] * derivativeArgs[0] * derivativeArgs[0] * derivativeArgs[0]) / ((1 - x) * Math.Tan(_alpha) + _teta);
			result -= (1 / (x + _ro) - Math.Tan(_alpha) / ((1 - x) * Math.Tan(_alpha) + _teta)) * derivativeArgs[1];
			return result;
		}
	}
}
