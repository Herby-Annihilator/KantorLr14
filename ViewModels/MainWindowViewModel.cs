using KantorLr14.Infrastructure.Commands;
using KantorLr14.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows.Markup;
using CompMathLibrary.KoshiTask.RungeKutt;
using KantorLr14.Models.Function;
using CompMathLibrary.Data;
using System.Collections.ObjectModel;
using KantorLr14.Models.Data;
using CompMathLibrary.Vectors;

namespace KantorLr14.ViewModels
{
	[MarkupExtensionReturnType(typeof(MainWindowViewModel))]
	public class MainWindowViewModel : ViewModel
	{
		public MainWindowViewModel()
		{
			CalculateCommand = new LambdaCommand(OnCalculateCommandExecuted, CanCalculateCommandExecute);
			ShowCommand = new LambdaCommand(OnShowCommandExecuted, CanShowCommandExecute);
			ClearCommand = new LambdaCommand(OnClearCommandExecuted, CanClearCommandExecute);
		}
		#region Properties
		private string _title = "Title";
		public string Title { get => _title; set => Set(ref _title, value); }

		private string _status = "Status";
		public string Status { get => _status; set => Set(ref _status, value); }

		private string _alpha = "";
		public string Alpha { get => _alpha; set => Set(ref _alpha, value); }

		private string _beta = "";
		public string Beta { get => _beta; set => Set(ref _beta, value); }

		private string _ro = "";
		public string Ro { get => _ro; set => Set(ref _ro, value); }

		private string _teta = "";
		public string Teta { get => _teta; set => Set(ref _teta, value); }

		private string _left = "";
		public string Left 
		{ 
			get => _left;
			set
			{
				YTextBlock = $"U({value}) = ";
				DyFromLeftTextBlock = $"U'({value})";
				Set(ref _left, value);
			}
		}

		private string _right = "";
		public string Right 
		{ 
			get => _right;
			set
			{
				DyTextBlock = $"U'({value}) = ";				
				Set(ref _right, value);
			}
		}

		private string _precision = "";
		public string Precision { get => _precision; set => Set(ref _precision, value); }

		private string _stepsCount = "";
		public string StepsCount { get => _stepsCount; set => Set(ref _stepsCount, value); }

		private string _y = "";
		public string Y { get => _y; set => Set(ref _y, value); }

		private string _yTextBlock = "U() = ";
		public string YTextBlock { get => _yTextBlock; set => Set(ref _yTextBlock, value); }

		private string _dy = "";
		public string Dy { get => _dy; set => Set(ref _dy, value); }

		private string _dyTextBlock = "U'() = ";
		public string DyTextBlock { get => _dyTextBlock; set => Set(ref _dyTextBlock, value); }

		private string _dyFromLeftTextBlock = "U'()";
		public string DyFromLeftTextBlock { get => _dyFromLeftTextBlock; set => Set(ref _dyFromLeftTextBlock, value); }

		private string _minDyFromLeftTextBlock = "";
		public string MinDyFromLeftTextBlock { get => _minDyFromLeftTextBlock; set => Set(ref _minDyFromLeftTextBlock, value); }

		private string _maxDyFromLeftTextBlock = "";
		public string MaxDyFromLeftTextBlock { get => _maxDyFromLeftTextBlock; set => Set(ref _maxDyFromLeftTextBlock, value); }

		private List<Point>[] answer;
		public ObservableCollection<Point> YFunction { get; private set; } = new ObservableCollection<Point>();
		public ObservableCollection<Point> VFunction { get; private set; } = new ObservableCollection<Point>();
		public ObservableCollection<ArgumentFunctionDerivative> Table { get; private set; } = new ObservableCollection<ArgumentFunctionDerivative>();

		#endregion

		#region Commands

		#region CalculateCommand
		public ICommand CalculateCommand { get; }
		private void OnCalculateCommandExecuted(object p)
		{
			try
			{
				double alpha = Convert.ToDouble(Alpha.Replace('.', ','));
				double beta = Convert.ToDouble(Beta.Replace('.', ','));
				double ro = Convert.ToDouble(Ro.Replace('.', ','));
				double teta = Convert.ToDouble(Teta.Replace('.', ','));
				double leftR = Convert.ToDouble(Left.Replace('.', ','));
				double rightR = Convert.ToDouble(Right.Replace('.', ','));
				double precision = Convert.ToDouble(Precision.Replace('.', ','));
				double dy = Convert.ToDouble(Dy.Replace('.', ','));
				double y = Convert.ToDouble(Y.Replace('.', ','));
				double maxDy = Convert.ToDouble(MaxDyFromLeftTextBlock.Replace('.', ','));
				double minDy = Convert.ToDouble(MinDyFromLeftTextBlock.Replace('.', ','));
				int stepsCount = Convert.ToInt32(StepsCount.Replace('.', ','));
				double leftDy = minDy;
				double rightDy = maxDy;
				double midDy = (rightDy + leftDy) / 2;
				RungeKuttaMethod rungeKuttaMethod = new RungeKuttaMethod();
				GlobalVectorDerivativeFunction f = GetFunction(alpha, beta, ro, teta);
				List<Point>[] functionsPointsLeft;
				List<Point>[] functionsPointsRight;
				List<Point>[] functionsPointsMiddle;
				Vector functionsStartConditions = new Vector(2);

				functionsStartConditions[0] = y;
				functionsStartConditions[1] = leftDy;
				functionsPointsLeft = rungeKuttaMethod.GetSystemSolution(f, leftR, rightR, functionsStartConditions, stepsCount);

				functionsStartConditions[0] = y;
				functionsStartConditions[1] = rightDy;
				functionsPointsRight = rungeKuttaMethod.GetSystemSolution(f, leftR, rightR, functionsStartConditions, stepsCount);

				functionsStartConditions[0] = y;
				functionsStartConditions[1] = midDy;
				functionsPointsMiddle = rungeKuttaMethod.GetSystemSolution(f, leftR, rightR, functionsStartConditions, stepsCount);

				while (Math.Abs(functionsPointsMiddle[1][functionsPointsMiddle[1].Count - 1].Y - dy) > precision)
				{
					if (functionsPointsMiddle[1][functionsPointsMiddle[1].Count - 1].Y * functionsPointsLeft[1][functionsPointsLeft[1].Count - 1].Y < 0)
						rightDy = midDy;
					else
						leftDy = midDy;
					midDy = (rightDy + leftDy) / 2;

					functionsStartConditions[0] = y;
					functionsStartConditions[1] = leftDy;
					functionsPointsLeft = rungeKuttaMethod.GetSystemSolution(f, leftR, rightR, functionsStartConditions, stepsCount);

					functionsStartConditions[0] = y;
					functionsStartConditions[1] = rightDy;
					functionsPointsRight = rungeKuttaMethod.GetSystemSolution(f, leftR, rightR, functionsStartConditions, stepsCount);

					functionsStartConditions[0] = y;
					functionsStartConditions[1] = midDy;
					functionsPointsMiddle = rungeKuttaMethod.GetSystemSolution(f, leftR, rightR, functionsStartConditions, stepsCount);
					if (Math.Abs(rightDy - leftDy) < double.Epsilon)
						throw new Exception($"На отрезке [{minDy};{maxDy}] решений нет");
				}
				answer = functionsPointsMiddle;
				Status = "Успешный расчет";
			}
			catch(Exception e)
			{
				Status = $"Неудача, причина: {e.Message}";
			}
		}
		private bool CanCalculateCommandExecute(object p) => !(string.IsNullOrWhiteSpace(Left) || string.IsNullOrWhiteSpace(Right) || string.IsNullOrWhiteSpace(Alpha) || string.IsNullOrWhiteSpace(Beta) || string.IsNullOrWhiteSpace(Ro) || string.IsNullOrWhiteSpace(Teta) || string.IsNullOrWhiteSpace(Y) || string.IsNullOrWhiteSpace(Dy) || string.IsNullOrWhiteSpace(MinDyFromLeftTextBlock) || string.IsNullOrWhiteSpace(MaxDyFromLeftTextBlock) || string.IsNullOrWhiteSpace(Precision));
		#endregion

		#region ShowCommand
		public ICommand ShowCommand { get; }
		private void OnShowCommandExecuted(object p)
		{
			try
			{
				Table.Clear();
				YFunction.Clear();
				VFunction.Clear();
				for (int i = 0; i < answer.Length; i++)
				{
					if (i == 0)
					{
						for (int j = 0; j < answer[i].Count; j++)
						{
							YFunction.Add(answer[i][j]);
							Table.Add(new ArgumentFunctionDerivative(answer[i][j].X, answer[i][j].Y, answer[i + 1][j].Y));
						}
					}
					else if (i == 1)
					{
						for (int j = 0; j < answer[i].Count; j++)
						{
							VFunction.Add(answer[i][j]);
						}
					}
				}
				Status = "Данные выведены";
			}
			catch (Exception e)
			{
				Status = $"Неудача, причина: {e.Message}";
			}
		}
		private bool CanShowCommandExecute(object p) => answer != null && answer.Length > 0;
		#endregion

		#region ClearCommand
		public ICommand ClearCommand { get; }
		private void OnClearCommandExecuted(object p)
		{
			try
			{
				Table.Clear();
				YFunction.Clear();
				VFunction.Clear();
				Status = "Данные не отображаются";
			}
			catch (Exception e)
			{
				Status = $"Неудача, причина: {e.Message}";
			}
		}
		private bool CanClearCommandExecute(object p) => Table.Count > 0 || YFunction.Count > 0 || VFunction.Count > 0;
		#endregion

		#endregion

		#region Methods
		private GlobalVectorDerivativeFunction GetFunction(double alpha, double beta, double ro, double teta)
		{
			YDerivative y = new YDerivative();
			VDerivative v = new VDerivative(alpha, beta, ro, teta);
			IDerivativeFunction[] derivativeFunctions = new IDerivativeFunction[2];
			derivativeFunctions[0] = y;
			derivativeFunctions[1] = v;
			GlobalVectorDerivativeFunction f = new GlobalVectorDerivativeFunction(derivativeFunctions);
			return f;
		}
		#endregion
	}
}
