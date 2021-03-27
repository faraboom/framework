﻿using Faraboom.Framework.Core;
using Faraboom.Framework.DataAnnotation;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Faraboom.Framework.Cookie
{
    [ServiceLifetime(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton)]
    public class CookieProvider : ICookieProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment webHostEnvironment;
        public CookieProvider(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.webHostEnvironment = webHostEnvironment;
        }

        public bool TryGetValue<TItem, TKey>(TKey key, out TItem value)
            where TKey : struct
        {
            return TryGetValue(key.ToString(), out value);
        }
        public bool TryGetValue<TItem>(string key, out TItem value)
        {
            value = Get<TItem>(key);
            return value.Equals(default(TItem));
        }

        public async Task<TItem> GetAsync<TItem, TKey>(TKey key)
            where TKey : struct
        {
            return await GetAsync<TItem>(key.ToString());
        }
        public async Task<TItem> GetAsync<TItem>(string key)
        {
            var cookie = httpContextAccessor.HttpContext.Request.Cookies[generateKey(key)];
            if (string.IsNullOrEmpty(cookie))
                return default;

            return await System.Text.Json.JsonSerializer.DeserializeAsync<TItem>(new MemoryStream(Convert.FromBase64String(cookie)));
        }

        public TItem Get<TItem, TKey>(TKey key)
            where TKey : struct
        {
            return Get<TItem>(key.ToString());
        }
        public TItem Get<TItem>(string key)
        {
            var cookie = httpContextAccessor.HttpContext.Request.Cookies[generateKey(key)];
            if (string.IsNullOrEmpty(cookie))
                return default;

            return System.Text.Json.JsonSerializer.Deserialize<TItem>(Convert.FromBase64String(cookie));
        }

        public void Remove<TKey>(TKey key)
            where TKey : struct
        {
            Remove(key.ToString());
        }
        public void Remove(string key)
        {
            httpContextAccessor.HttpContext.Response.Cookies.Delete(generateKey(key));
        }

        public async Task<TItem> SetAsync<TItem, TKey>(TKey key, TItem value, TimeSpan? maxAge = null, DateTimeOffset? Expires = null, bool httpOnly = true)
            where TKey : struct
        {
            return await SetAsync(key.ToString(), value, maxAge, Expires, httpOnly);
        }
        public async Task<TItem> SetAsync<TItem>(string key, TItem value, TimeSpan? maxAge = null, DateTimeOffset? Expires = null, bool httpOnly = true)
        {
            if (httpContextAccessor.HttpContext.Request.Cookies[generateKey(key)] != "")
                Remove(key);

            var jsonValue = string.Empty;
            using (var stream = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync(stream, value);
                stream.Position = 0;
                using var reader = new StreamReader(stream);
                jsonValue = await reader.ReadToEndAsync();
            }

            CookieOptions option = new CookieOptions
            {
                HttpOnly = httpOnly,
                Secure = !webHostEnvironment.IsDevelopment(),
                IsEssential = true,
                MaxAge = maxAge,
                Expires = Expires
            };


            httpContextAccessor.HttpContext.Response.Cookies.Append(generateKey(key), Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonValue)), option);

            return value;
        }

        public TItem Set<TItem, TKey>(TKey key, TItem value, TimeSpan? maxAge = null, DateTimeOffset? Expires = null, bool httpOnly = true)
            where TKey : struct
        {
            return Set(key.ToString(), value, maxAge, Expires, httpOnly);
        }
        public TItem Set<TItem>(string key, TItem value, TimeSpan? maxAge = null, DateTimeOffset? Expires = null, bool httpOnly = true)
        {
            if (httpContextAccessor.HttpContext.Request.Cookies[generateKey(key)] != "")
                Remove(key);

            var jsonValue = JsonSerializer.Serialize(value);

            CookieOptions option = new CookieOptions
            {
                HttpOnly = httpOnly,
                Secure = !webHostEnvironment.IsDevelopment(),
                IsEssential = true,
                MaxAge = maxAge,
                Expires = Expires
            };


            httpContextAccessor.HttpContext.Response.Cookies.Append(generateKey(key), Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonValue)), option);

            return value;
        }

        private string generateKey(string key)
        {
            return $"{Constants.Cookie}{key}";
        }
    }
}