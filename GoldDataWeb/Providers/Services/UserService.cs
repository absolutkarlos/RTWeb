using System.Collections.Generic;
using GD.Models.Commons;
using GoldDataWeb.Providers.Services.Interface;

namespace GoldDataWeb.Providers.Services
{
	public class UserService : IService<User>
	{
		public RestfulServiceProvider Service;

		public string Token
		{
			set { Service.AuthorizationToken = value; }
		}

		public UserService()
		{
			Service = new RestfulServiceProvider(RestfulServiceProvider.UserUrlApi);
		}

		public UserService(string token)
		{
			Service = new RestfulServiceProvider(RestfulServiceProvider.UserUrlApi, token);
		}

		public User GetById<TId>(TId id)
		{
			Service.AddQueryParameters(new Dictionary<string, string>
			{
				{ @"id", id.ToString() }
			});
			return Service.Get<User>();
		}

		public IEnumerable<User> GetAll()
		{
			return Service.Get<IEnumerable<User>>();
		}

		public void Update(User user)
		{
			Service.AddBodyToRequest(user);
			Service.Put<User>();
		}

		public long Insert(User user)
		{
			Service.AddBodyToRequest(user);
			return Service.Post<long>();
		}

		public void Delete<TId>(TId id)
		{
			Service.AddQueryParameters(new Dictionary<string, string>
			{
				{ @"id", id.ToString() }
			});
			Service.Delete();
		}

		public void LogOff()
		{
			//Service.Execute(@"LogOff", Method.GET);
		}
	}
}
