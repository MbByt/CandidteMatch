﻿using CandidateMatch.Common.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CandidateMatch.Data.Context
{
    public class CandidateAPIContext : ICandidateAPIContext
    {
        private readonly IJobAdderApiOptions _jobAdderApiOptions;

        public CandidateAPIContext(IJobAdderApiOptions jobAdderApiOptions)
        {
            _jobAdderApiOptions = jobAdderApiOptions;
         }

        public async Task<T> GetAsync<T>(string queries = "")
        {
            var client = GetHttpClient(queries);
            var url = $"{client.BaseAddress}";
            var result = client.GetAsync(url).Result;

            if (result.IsSuccessStatusCode)
            {
                var jsonResult = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonResult);
            }
            else
            {
                var msg = await result.Content.ReadAsStringAsync();
                throw new ApplicationException(msg);
            }
        }
        protected HttpClient GetHttpClient(string queries)
        {
            HttpClient httpClient = new HttpClient();
            string url = _jobAdderApiOptions.CandidateApiEndPoint + _jobAdderApiOptions.CandidatesAPIPath;
            if (string.IsNullOrEmpty(queries))
            {
                url += $"?{queries}";
            }
            httpClient.BaseAddress = new Uri(url);
            httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            return httpClient;
        }

    }
}
