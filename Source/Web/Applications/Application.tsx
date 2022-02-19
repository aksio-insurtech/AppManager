// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Pivot, PivotItem, Stack } from '@fluentui/react';
import { ApplicationSettings } from './ApplicationSettings';
import { Tenants } from './Tenants';
import { GetApplication } from 'API/applications/GetApplication';
import { useParams } from 'react-router-dom';

export const Application = () => {
    const { applicationId } = useParams();
    const [application] = GetApplication.use({ applicationId: applicationId! });

    return (
        <Stack style={{ height: '100%' }}>
            <Stack.Item>
                <Pivot>
                    <PivotItem headerText='General'>
                        { application.data.id && <ApplicationSettings application={application.data} /> }
                    </PivotItem>
                    <PivotItem headerText='Tenants'>
                        <Tenants />
                    </PivotItem>
                    <PivotItem headerText='Environment'>
                    </PivotItem>
                    <PivotItem headerText='Secrets'>
                    </PivotItem>
                </Pivot>
            </Stack.Item>
        </Stack>
    );
};
