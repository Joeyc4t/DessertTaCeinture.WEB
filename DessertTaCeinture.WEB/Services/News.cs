﻿using DessertTaCeinture.WEB.Models.News;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DessertTaCeinture.WEB.Services
{
    public class News
    {
        #region Instances
        private static News _Instance;
        public static News Instance
        {
            get { return _Instance = _Instance ?? new News(); }
        }
        private News() { }
        #endregion

        public IEnumerable<NewsModel> GetAll()
        {
            IEnumerable<NewsModel> models = Enumerable.Empty<NewsModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:50140/");
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = client.GetAsync($"api/News").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var result = Res.Content.ReadAsStringAsync().Result;
                        models = JsonConvert.DeserializeObject<IEnumerable<NewsModel>>(result);
                    }
                }
                return models.OrderByDescending(m => m.ReleaseDate);
            }
            catch
            {
                return null;
            }
        }

        public NewsModel Get(int id)
        {
            NewsModel model = new NewsModel();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:50140/");
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = client.GetAsync($"api/News?id={id}").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var result = Res.Content.ReadAsStringAsync().Result;
                        model = JsonConvert.DeserializeObject<NewsModel>(result);
                    }
                }
                return model;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<NewsModel> GetLastPublished()
        {
            return GetAll().Take(6);
        }
    }
}