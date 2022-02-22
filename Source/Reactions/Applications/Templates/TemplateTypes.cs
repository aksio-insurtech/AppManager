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
