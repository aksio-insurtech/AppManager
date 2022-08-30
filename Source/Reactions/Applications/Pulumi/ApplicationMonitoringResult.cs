// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Pulumi.AzureNative.Insights.V20200202;
using Pulumi.AzureNative.OperationalInsights;

namespace Reactions.Applications.Pulumi;

public record ApplicationMonitoringResult(Workspace Workspace, Component ApplicationInsight);
