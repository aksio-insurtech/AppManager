// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useEffect, useState } from 'react';
import { CratisVersions } from 'API/cratis/CratisVersions';
import { InputLabel, MenuItem, Select } from '@mui/material';
import { SemanticVersion } from 'API/cratis/SemanticVersion';

function formatVersion(version: SemanticVersion) {
    return `${version.major}.${version.minor}.${version.patch}`;
}

export const General = () => {
    const [versions] = CratisVersions.use();

    const currentVersion = new SemanticVersion();
    currentVersion.major = 6;
    currentVersion.minor = 11;
    currentVersion.patch = 5;

    const [version, setVersion] = useState(formatVersion(currentVersion));

    let actualVersions = [currentVersion];

    if (!versions.isPerforming && versions.hasData) {
        actualVersions = versions.data;
    }

    return (
        <>
            <InputLabel>Version</InputLabel>
            <Select
                label="Version"
                placeholder="Select Cratis Version"
                value={version}
                onChange={(ev) => {
                    setVersion(ev.target.value);
                }}>
                {actualVersions.map(version => {
                    const formatted = formatVersion(version);
                    return (
                        <MenuItem key={formatted} value={formatted}>{formatted}</MenuItem>
                    );
                })}
            </Select>

        </>
    );
};
