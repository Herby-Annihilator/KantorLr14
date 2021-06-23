using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace KantorLr14.ViewModels
{
	public static class ViewModelsRegistrator
	{
		public static IServiceCollection AddViewModels(this IServiceCollection services) => services
		   .AddSingleton<MainWindowViewModel>()
		;
	}
}
