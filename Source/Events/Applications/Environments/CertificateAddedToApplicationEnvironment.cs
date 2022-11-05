// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Concepts.Applications.Environments;

namespace Events.Applications.Environments;

[EventType("ff757985-016c-4074-8c5c-6d5e5da3e03e")]
public record CertificateAddedToApplicationEnvironment(ApplicationId ApplicationId, ApplicationEnvironmentId EnvironmentId, CertificateId CertificateId, CertificateName Name, Certificate Certificate);
