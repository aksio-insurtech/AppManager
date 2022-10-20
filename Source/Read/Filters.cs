// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq.Expressions;
using Aksio.Cratis.Reflection;
using Aksio.Cratis.Strings;

namespace Read;

public static class Filters
{
    public static FilterDefinition<T> StringFilterFor<T>(Expression<Func<T, object>> property, object value)
        where T : notnull
    {
        var propertyInfo = property.GetPropertyInfo();
        var name = propertyInfo.Name.ToCamelCase();
        return Builders<T>.Filter.Eq(new StringFieldDefinition<T, string>(name), value.ToString()!);
    }
}
