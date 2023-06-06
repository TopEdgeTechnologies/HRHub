using System.Net.Http.Headers;
using System.Text;
using System;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace HRHUBWEB.Extensions
{
	public class APIHelper
	{

		private readonly HttpClient _client;
		private IWebHostEnvironment _webHostEnvironment;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public APIHelper(IHttpClientFactory httpClient, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
		{
			_client = httpClient.CreateClient("APIClient");
			_webHostEnvironment = webHostEnvironment;
			_httpContextAccessor = httpContextAccessor;
		}

		public  async Task<T> CallApiAsyncPost<T>(object model, string apiUrl, HttpMethod httpMethod)
		{
			//get Token
			string jwtToken = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");

			// Set the Authorization header with the JWT token
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

			// Serialize the model to JSON
			string json = JsonSerializer.Serialize(model);

			// Create the HTTP request message
			var request = new HttpRequestMessage(httpMethod, apiUrl);
			request.Content = new StringContent(json, Encoding.UTF8, "application/json");

			// Send the HTTP request
			HttpResponseMessage response = await _client.SendAsync(request);

			// Handle the response
			if (response.IsSuccessStatusCode)
			{
				JsonSerializerOptions options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};
				string content = await response.Content.ReadAsStringAsync();
				return JsonSerializer.Deserialize<T>(content, options);
			}
			else
			{
				throw new Exception($"Error calling API: {response.StatusCode}");
			}
		}

		public async Task<T> CallApiAsyncGet<T>( string apiUrl, HttpMethod httpMethod)
		{
			// Replace this with the JWT token you received from the server
			string jwtToken = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");

			// Create an HttpClient instance
		
			// Set the Authorization header with the JWT token
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

			// Serialize the model to JSON
			//string json = JsonSerializer.Serialize(model);

			// Create the HTTP request message
			var request = new HttpRequestMessage(httpMethod, apiUrl);
			//request.Content = new StringContent(json, Encoding.UTF8, "application/json");

			// Send the HTTP request
			HttpResponseMessage response = await _client.SendAsync(request);

			// Handle the response
			if (response.IsSuccessStatusCode)
			{
				JsonSerializerOptions options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};

				string content = await response.Content.ReadAsStringAsync();
				return JsonSerializer.Deserialize<T>(content, options);
			}
			else
			{
				throw new Exception($"Error calling API: {response.StatusCode}");
			}
		}



        public async Task<dynamic> CallApiDynamic<T>(object model, string apiUrl, HttpMethod httpMethod)
        {
            // Replace this with the JWT token you received from the server
            string jwtToken = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<string>("AuthenticatedToken");

            // Create an HttpClient instance

            // Set the Authorization header with the JWT token
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

		//	Serialize the model to JSON

			string json = JsonSerializer.Serialize(model);

			// Create the HTTP request message
			var request = new HttpRequestMessage(httpMethod, apiUrl);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send the HTTP request
            HttpResponseMessage response = await _client.SendAsync(request);

            // Handle the response
            if (response.IsSuccessStatusCode)
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<dynamic>(content, options);
            }
            else
            {
                throw new Exception($"Error calling API: {response.StatusCode}");
            }
        }

        //var model = new { Name = "John", Age = 30 };

        //// Replace this with your actual API URL
        //string apiUrl = "https://your-api-url";

        //// Call the API with a POST request and return an object
        //var result = await CallApiAsync<object>(model, apiUrl, HttpMethod.Post);

        //// Call the API with a GET request and return a list
        //var resultList = await CallApiAsync<List<object>>(model, apiUrl, HttpMethod.Get);

    }
}



