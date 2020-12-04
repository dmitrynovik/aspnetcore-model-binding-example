using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace UrlPathModelBinder
{
    public class PathModelBinder<T> : IModelBinder where T: new()
    {
        private readonly ILogger<PathModelBinder<T>> _logger;
        private static readonly char[] Slash = new char[] { '/' };
        private IDictionary<string, PropertyInfo> _props;

        public PathModelBinder(ILogger<PathModelBinder<T>> logger)
        {
            _logger = logger;
        }

        public uint SkipPathSegments { get; set; } = 2; // (default): skip [controller]/[action]

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var watch = Stopwatch.StartNew();
            var model = new T();
            var i = 0;
            string propName = "";
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty;
            uint skipSegments;

            if (model is IUrlPathModel urlPathModel)
            {
                skipSegments = urlPathModel.SkipPathSegments;
            }
            else
            {
                skipSegments = SkipPathSegments;
            }

            _props = typeof(T).GetProperties(flags).ToDictionary(
                p => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(p.Name),
                p => p);

            var pathTokens = bindingContext.ActionContext.HttpContext.Request.Path.Value
                .Split(Slash, StringSplitOptions.RemoveEmptyEntries)
                .Skip((int)skipSegments);

            foreach (var pathToken in pathTokens)
            {
                if (i % 2 == 0)
                {
                    propName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(pathToken);
                }
                else
                {
                    try
                    {
                        _props.TryGetValue(propName, out var propertyInfo);
                        if (propertyInfo != null)
                        {
                            propertyInfo.SetValue(model, pathToken);
                        }
                        else
                        {
                            _logger.LogError("Cannot find property: {0}", propName);
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.ToString());
                    }
                }

                i++;
            }

            bindingContext.Result = ModelBindingResult.Success(model);
            watch.Stop();
            _logger.LogTrace("PathModelBinder, elapsed = {0}", watch.Elapsed);
            return Task.CompletedTask;
        }
    }
}
