// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq.Expressions;
using Aksio.Cratis.Reflection;

namespace Read;

public static class Filters
{
    public static FilterDefinition<T> StringFilterFor<T>(Expression<Func<T, object>> property, object value)
        where T : notnull
    {
        var propertyPath = property.GetPropertyPath();
        var actualPropertyPath = string.Join('.', propertyPath.Segments.Select(_ => _.Value.Equals("id") ? "_id" : _.Value));
        return Builders<T>.Filter.Eq(new StringFieldDefinition<T, string>(actualPropertyPath), value.ToString()!);
    }
}
