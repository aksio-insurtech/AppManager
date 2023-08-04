// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Reactions.Applications;

/// <summary>
/// File upload results.
/// </summary>
public enum FileStorageResult
{
    /// <summary>
    /// Uknown status.
    /// </summary>
    Undefined = 0,

    /// <summary>
    /// No file content was specified.
    /// </summary>
    NoContent = 1,

    /// <summary>
    /// An identical file existed at the target, so no upload performed.
    /// </summary>
    FileExisted = 2,

    /// <summary>
    /// The file was uploaded.
    /// </summary>
    FileUploaded = 3
}