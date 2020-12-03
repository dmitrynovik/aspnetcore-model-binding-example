using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SampleWebApi.ModelBinders
{
    public class PathModelBinder<T> : IModelBinder where T: new()
    {
        private readonly int _skipPathSegments;
        private static readonly char[] Slash = new char[] { '/' };
        private IDictionary<string, PropertyInfo> _props;

        public PathModelBinder(int skipPathSegments = 2)
        {
            _skipPathSegments = skipPathSegments;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var watch = Stopwatch.StartNew();
            var model = new T();
            var i = 0;
            string propName = "";

            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty;

            _props = typeof(T).GetProperties(flags).ToDictionary(
                p => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(p.Name),
                p => p);

            var pathTokens = bindingContext.ActionContext.HttpContext.Request.Path.Value
                .Split(Slash, StringSplitOptions.RemoveEmptyEntries)
                .Skip(_skipPathSegments);

            foreach (var pathToken in pathTokens)
            {
                if (i % 2 == 0)
                    propName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(pathToken);
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
                            Trace.TraceError("Cannot find property: {0}", propName);
                        }
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError(e.ToString());
                    }
                }

                i++;
            }

            bindingContext.Result = ModelBindingResult.Success(model);
            watch.Stop();
            Console.WriteLine("PathModelBinder, elapsed = {0}", watch.Elapsed);
            return Task.CompletedTask;
        }
    }
}
