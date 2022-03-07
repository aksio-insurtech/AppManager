// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi;

namespace Reactions.Applications.Pulumi;

public static class OutputExtensionMethods
{
    public static Task<T> GetValue<T>(this Output<T> output) => output.GetValue(_ => _);

    public static Task<TResult> GetValue<T, TResult>(this Output<T> output, Func<T, TResult> valueResolver)
    {
        var tcs = new TaskCompletionSource<TResult>();
        output.Apply(_ =>
        {
            var result = valueResolver(_);
            tcs.SetResult(result);
            return result;
        });
        return tcs.Task;
    }
}
