// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Strings;
using HandlebarsDotNet;

namespace Reactions.Applications.Templates;

/// <summary>
/// Holds all the available templates.
/// </summary>
public static class TemplateTypes
{
    /// <summary>
    /// The template for 'appsettings.json'.
    /// </summary>
    public static readonly HandlebarsTemplate<object, object> AppSettings = Handlebars.Compile(GetTemplate("appsettings.json"));

    /// <summary>
    /// The template for 'cluster.json' for client.
    /// </summary>
    public static readonly HandlebarsTemplate<object, object> ClusterClient = Handlebars.Compile(GetTemplate("cluster.client.json"));

    /// <summary>
    /// The template for 'nginx.conf' for the "auth ingress controller".
    /// </summary>
    public static readonly HandlebarsTemplate<object, object> IngressConfig = Handlebars.Compile(GetTemplate("nginx.conf"));

    /// <summary>
    /// The template for Ingress Middleware config.
    /// </summary>
    public static readonly HandlebarsTemplate<object, object> IngressMiddlewareConfig = Handlebars.Compile(GetTemplate("ingress-middleware.json"));

    static TemplateTypes()
    {
        Handlebars.RegisterHelper("camelcase", (writer, _, parameters) => writer.WriteSafeString(parameters[0].ToString()!.ToCamelCase()));
    }

    static string GetTemplate(string name)
    {
        var rootType = typeof(TemplateTypes);
        var stream = rootType.Assembly.GetManifestResourceStream($"{rootType.Namespace}.{name}.hbs");
        if (stream != default)
        {
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
        return string.Empty;
    }
}
