// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi;

namespace Reactions.Applications;

public static class InputExtensionMethods
{
    public static Task<T> GetValue<T>(this Input<T> input) => input.GetValue(_ => _);

    public static Task<TResult> GetValue<T, TResult>(this Input<T> input, Func<T, TResult> valueResolver)
    {
        var tcs = new TaskCompletionSource<TResult>();
        input.Apply(_ =>
        {
            var result = valueResolver(_);
            tcs.SetResult(result);
            return result;
        });
        return tcs.Task;
    }
}
