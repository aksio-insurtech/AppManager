// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications;

namespace Domain.Applications.Environments.Ingresses;

public record DefineRoute(string Path, MicroserviceId TargetMicroservice, string TargetPath);
