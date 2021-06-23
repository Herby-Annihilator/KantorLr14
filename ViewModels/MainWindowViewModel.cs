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

		private string _y = "";
		public string Y { get => _y; set => Set(ref _y, value); }

		private string _yTextBlock = "U() = ";
		public string YTextBlock { get => _yTextBlock; set => Set(ref _yTextBlock, value); }

		private string _dy = "";
		public string Dy { get => _dy; set => Set(ref _dy, value); }

		private string _dyTextBlock = "U'() = ";
		public string DyTextBlock { get => _dyTextBlock; set => Set(ref _dyTextBlock, value); }

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
				double currentDy = double.MaxValue;
				RungeKuttaMethod rungeKuttaMethod = new RungeKuttaMethod();
				GlobalVectorDerivativeFunction f = GetFunction(alpha, beta, ro, teta);
				List<Point>[] functionsPoints;
				while (Math.Abs(currentDy - dy) > precision)
				{

				}
				Status = "Успешный расчет";
			}
			catch(Exception e)
			{
				Status = $"Неудача, причина: {e.Message}";
			}
		}
		private bool CanCalculateCommandExecute(object p) => !(string.IsNullOrWhiteSpace(Left) || string.IsNullOrWhiteSpace(Right) || string.IsNullOrWhiteSpace(Alpha) || string.IsNullOrWhiteSpace(Beta) || string.IsNullOrWhiteSpace(Ro) || string.IsNullOrWhiteSpace(Teta));
		#endregion

		#region ShowCommand
		public ICommand ShowCommand { get; }
		private void OnShowCommandExecuted(object p)
		{
			try
			{
				
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
