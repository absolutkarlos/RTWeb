using System.Collections.Generic;
using GoldDataWeb.Models;

namespace GoldDataWeb.Providers.Services.Interface
{
	public interface IService<TModel>
	{
		ResponseService<TModel> GetById<TId>(TId id);
		ResponseService<IEnumerable<TModel>> GetAll();
		ResponseService<TModel> Update(TModel model);
		ResponseService<long> Insert(TModel model);
		ResponseService<TModel> Delete<TId>(TId id);
	}
}
