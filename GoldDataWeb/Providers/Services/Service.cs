using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Authentication;
using GD.Models.Commons.Utilities;
using GoldDataWeb.Models;
using GoldDataWeb.Providers.Services.Interface;
using RestSharp;

namespace GoldDataWeb.Providers.Services
{
	public class Service<TModel> : IService<TModel>
	{
		public RestfulServiceProvider Provider;

		public string Token
		{
			set { Provider.AuthorizationToken = value; }
		}

		public Service(string url)
		{
			Provider = new RestfulServiceProvider(url);
		}

		public Service(string url, string token)
		{
			Provider = new RestfulServiceProvider(url, token);
		}

		public ResponseService<TModel> Execute(string actionName, Method method, string parameter = null, string jsonBody = null)
		{
			var response = new ResponseService<TModel>();

			try
			{
				if (!string.IsNullOrWhiteSpace(parameter))
				{
					actionName += "/" + parameter;
				}

				response.Data = Provider.Execute<TModel>(actionName, method, null, jsonBody);
				response.Status = HttpStatusCode.OK;
			}
			catch (AuthenticationException e)
			{
				response.ErrorMessage = e.Message;
				response.Status = HttpStatusCode.Unauthorized;
			}
			catch (Exception e)
			{
				response.ErrorMessage = e.Message;
				response.Status = HttpStatusCode.InternalServerError;
			}

			return response;
		}

		public ResponseService<T> Execute<T>(string actionName, Method method, string parameter = null, string jsonBody = null)
		{
			var response = new ResponseService<T>();

			try
			{
				if (!string.IsNullOrWhiteSpace(parameter))
				{
					actionName += "/" + parameter;
				}

				response.Data = Provider.Execute<T>(actionName, method, null, jsonBody);
				response.Status = HttpStatusCode.OK;
			}
			catch (AuthenticationException e)
			{
				response.ErrorMessage = e.Message;
				response.Status = HttpStatusCode.Unauthorized;
			}
			catch (Exception e)
			{
				response.ErrorMessage = e.Message;
				response.Status = HttpStatusCode.InternalServerError;
			}

			return response;
		}

		public ResponseService<TModel> GetById<TId>(TId id)
		{
			var response = new ResponseService<TModel>();

			try
			{
				Provider.AddQueryParameters(new Dictionary<string, string>
				{
					{ @"id", id.ToString() }
				});
				response.Data = Provider.Get<TModel>();
				response.Status = HttpStatusCode.OK;
			}
			catch (AuthenticationException e)
			{
				response.ErrorMessage = e.Message;
				response.Status = HttpStatusCode.Unauthorized;
			}
			catch (Exception e)
			{
				response.ErrorMessage = e.Message;
				response.Status = HttpStatusCode.InternalServerError;
			}

			return response;
		}

		public ResponseService<IEnumerable<TModel>> GetAll()
		{
			var response = new ResponseService<IEnumerable<TModel>>();

			try
			{
				response.Data = Provider.Get<IEnumerable<TModel>>();
				response.Status = HttpStatusCode.OK;
			}
			catch (AuthenticationException e)
			{
				response.ErrorMessage = e.Message;
				response.Status = HttpStatusCode.Unauthorized;
			}
			catch (Exception e)
			{
				response.ErrorMessage = e.Message;
				response.Status = HttpStatusCode.InternalServerError;
			}

			return response;
		}

		public ResponseService<TModel> Update(TModel model)
		{
			var response = new ResponseService<TModel>();

			try
			{
				Provider.AddJsonToRequest(model);
				response.Data = Provider.Put<TModel>();
				response.Status = HttpStatusCode.OK;
			}
			catch (AuthenticationException e)
			{
				response.ErrorMessage = e.Message;
				response.Status = HttpStatusCode.Unauthorized;
			}
			catch (Exception e)
			{
				response.ErrorMessage = e.Message;
				response.Status = HttpStatusCode.InternalServerError;
			}

			return response;
		}

		public ResponseService<long> Insert(TModel model)
		{
			var response = new ResponseService<long>();

			try
			{
				Provider.AddJsonToRequest(model);
				response.Data = Provider.Post<long>();
				response.Status = HttpStatusCode.OK;
			}
			catch (AuthenticationException e)
			{
				response.ErrorMessage = e.Message;
				response.Status = HttpStatusCode.Unauthorized;
			}
			catch (Exception e)
			{
				response.ErrorMessage = e.Message;
				response.Status = HttpStatusCode.InternalServerError;
			}

			return response;
		}

		public ResponseService<TModel> Delete<TId>(TId id)
		{
			var response = new ResponseService<TModel>();

			try
			{
				Provider.AddQueryParameters(new Dictionary<string, string>
				{
					{ @"id", id.ToString() }
				});
				Provider.Delete();
				response.Status = HttpStatusCode.OK;
			}
			catch (AuthenticationException e)
			{
				response.ErrorMessage = e.Message;
				response.Status = HttpStatusCode.Unauthorized;
			}
			catch (Exception e)
			{
				response.ErrorMessage = e.Message;
				response.Status = HttpStatusCode.InternalServerError;
			}

			return response;
		}
	}
}