using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Security.Authentication;
using GD.Models.Commons.Utilities;
using RestSharp;

namespace GoldDataWeb.Providers
{
	public class RestfulServiceProvider
	{
		private readonly IRestClient _client;
		private IRestResponse _response;
		public IRestRequest Request;

		protected const string TokenType = @"Bearer ";
		protected const string ContentType = @"application/json";
		protected string HostApi = ConfigurationManager.AppSettings[@"HostApi"];
		public static string GrantType = @"password";
		
		public static string AuthUrlApi = @"/token";
		public static string UserUrlApi = @"/api/user/";
		public static string RolUrlApi = @"/api/rol/";
		public static string ServiceTypeUrlApi = @"/api/servicetype/";
		public static string AccessTypeUrlApi = @"/api/accesstype/";
		public static string EntityTypeUrlApi = @"/api/entitytype/";
		public static string EntityContactUrlApi = @"/api/entitycontact/";
		public static string EntityChannelUrlApi = @"/api/entitychannel/";
		public static string EntityChannelTypeUrlApi = @"/api/entitychanneltype/";
		public static string OrderStatusUrlApi = @"/api/orderstatus/";
		public static string OrderShotTypeUrlApi = @"/api/ordershottype/";
		public static string OrderShotUrlApi = @"/api/ordershot/";
		public static string PositionUrlApi = @"/api/position/";
		public static string MaterialUrlApi = @"/api/material/";
		public static string CountryUrlApi = @"/api/country/";
		public static string OrderUrlApi = @"/api/order/";
		public static string ClientTypeUrlApi = @"/api/clienttype/";
		public static string ClientUrlApi = @"/api/client/";
		public static string RadioBaseUrlApi = @"/api/radiobase/";
		public static string StateUrlApi = @"/api/state/";
		public static string CityUrlApi = @"/api/city/";
		public static string ZoneUrlApi = @"/api/zone/";
		public static string SiteUrlApi = @"/api/site/";
		public static string SiteScheduleUrlApi = @"/api/siteschedule/";
		public static string LineSightUrlApi = @"/api/linesight/";
		public static string SiteAccessTypeUrlApi = @"/api/siteaccesstype/";
		public static string OrderMaterialUrlApi = @"/api/ordermaterial/";
		public static string OrderFlowServiceUrlApi = @"/api/orderflow/";

		public string AuthorizationToken { private get; set; }

		public RestfulServiceProvider(string apiUrl)
		{
			_client = new RestClient(HostApi + apiUrl);
			Request = new RestRequest
			{
				RequestFormat = DataFormat.Json
			};
		}

		public RestfulServiceProvider(string apiUrl, string token)
		{
			AuthorizationToken = token;
			_client = new RestClient(HostApi + apiUrl);
			Request = new RestRequest
			{
				RequestFormat = DataFormat.Json
			};
			AddHeadersClientAuthentication();
		}

		public T Execute<T>(string actionName, Method method, IDictionary<string, string> queryParameters = null, string jsonBody = null)
		{
			var client = new RestClient(_client.BaseUrl + actionName);
			Request = new RestRequest(method);
			AddHeadersClientAuthentication();

			if (queryParameters != null)
			{
				AddQueryParameters(queryParameters);
			}
			else if (!string.IsNullOrWhiteSpace(jsonBody))
			{
				AddJsonToRequest(jsonBody);
			}

			_response = client.Execute(Request);

			if (_response.StatusCode == HttpStatusCode.Unauthorized)
			{
				throw new AuthenticationException();
			}

			return TranslateResponse<T>(_response);
		}

		public T Get<T>()
		{
			_response = _client.Get(Request);

			if (_response.StatusCode == HttpStatusCode.Unauthorized)
			{
				throw new AuthenticationException();
			}

			return TranslateResponse<T>(_response);
		}

		public T Post<T>()
		{
			_response = _client.Post(Request);

			if (_response.StatusCode == HttpStatusCode.Unauthorized)
			{
				throw new AuthenticationException();
			}

			return TranslateResponse<T>(_response);
		}

		public T Put<T>()
		{
			_response = _client.Put(Request);

			if (_response.StatusCode == HttpStatusCode.Unauthorized)
			{
				throw new AuthenticationException();
			}

			return TranslateResponse<T>(_response);
		}

		public void Delete()
		{
			_response = _client.Delete(Request);

			if (_response.StatusCode == HttpStatusCode.Unauthorized)
			{
				throw new AuthenticationException();
			}
			ClearParameters();
		}

		private T TranslateResponse<T>(IRestResponse restResponse)
		{
			ClearParameters();
			return JsonHelper.Deserialize<T>(restResponse.Content);
		}

		private void AddHeadersToRequest(IDictionary<string, string> headers)
		{
			foreach (KeyValuePair<string, string> header in headers)
			{
				Request.AddHeader(header.Key, header.Value);
			}
		}

		public void AddQueryParameters(IDictionary<string, string> queryParameters)
		{
			foreach (KeyValuePair<string, string> queryParameter in queryParameters)
			{
				Request.AddQueryParameter(queryParameter.Key, queryParameter.Value);
			}
		}

		private void ClearParameters()
		{
			Request.Parameters.Clear();
			AddHeadersClientAuthentication();
		}

		private void AddHeadersClientAuthentication()
		{
			AddHeadersToRequest(new Dictionary<string, string>
			{
				{@"Authorization", TokenType + AuthorizationToken},
				{@"Content-Type", ContentType}
			});
		}

		public void AddJsonToRequest<T>(T body)
		{
			Request.AddParameter(@"text/json", body.ToJson(), ParameterType.RequestBody);
		}

		public void AddJsonToRequest(string body)
		{
			Request.AddParameter(@"text/json", body, ParameterType.RequestBody);
		}

		public void AddObjectToRequest<T>(T body)
		{
			Request.AddObject(body);
		}
	}
}
