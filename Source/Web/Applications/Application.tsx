// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Pivot, PivotItem, Stack } from '@fluentui/react';
import { ApplicationSettings } from './ApplicationSettings';
import { Tenants } from './Tenants';
import { GetApplication } from 'API/applications/GetApplication';
import { useParams } from 'react-router-dom';

export const Application = () => {
    const items: any[] = [];
    const { applicationId } = useParams();
    const [getApplication] = GetApplication.use({ applicationId: applicationId! });

    return (
        <Stack style={{ height: '100%' }}>
            <Stack.Item>
                <Pivot>
                    <PivotItem headerText='General'>
                        <ApplicationSettings application={getApplication.data} />
                    </PivotItem>
                    <PivotItem headerText='Tenants'>
                        <Tenants />
                    </PivotItem>
                </Pivot>
            </Stack.Item>
        </Stack>
    );
};
