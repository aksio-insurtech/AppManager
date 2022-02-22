// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Reactions;

public interface IPassiveProjectionFor<TModel>
{
    ProjectionId Identifier { get; }

    void Define(IProjectionBuilderFor<TModel> builder);
}
