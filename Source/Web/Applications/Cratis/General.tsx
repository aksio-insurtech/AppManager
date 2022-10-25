// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useEffect, useState } from 'react';
import { CratisVersions } from 'API/cratis/CratisVersions';
import { InputLabel, MenuItem, Select } from '@mui/material';
import { SemanticVersion } from 'API/cratis/SemanticVersion';
import { ApplicationEnvironment } from 'API/applications/environments/ApplicationEnvironment';

function formatVersion(version: SemanticVersion) {
    if (!version) {
        return '0.0.0';
    }
    return `${version.major}.${version.minor}.${version.patch}`;
}

interface GeneralProps {
    environment: ApplicationEnvironment;
}

export const General = (props: GeneralProps) => {
    const [versions] = CratisVersions.use();
    const [version, setVersion] = useState<string | undefined>();

    useEffect(() => {
        setVersion(formatVersion(props.environment.cratisVersion));
    }, [props.environment.cratisVersion]);

    let actualVersions = [props.environment.cratisVersion];

    if (!versions.isPerforming && versions.hasData) {
        actualVersions = versions.data;
    }

    return (
        <>
            {(version && version != '0.0.0') &&
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
            }
        </>
    );
};
