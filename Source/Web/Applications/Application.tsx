// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Pivot, PivotItem, Stack } from '@fluentui/react';
import { ApplicationSettings } from './ApplicationSettings';
import { Tenants } from './Tenants';
import { Application as ApplicationModel } from 'API/applications/Application';

export interface IApplicationProps {
    application?: ApplicationModel;
}

export const Application = (props: IApplicationProps) => {
    const items: any[] = [];

    return (
        <Stack style={{ height: '100%' }}>
            <Stack.Item>
                <Pivot>
                    <PivotItem headerText='General'>
                        {/* <ApplicationSettings application={props.application}/> */}
                    </PivotItem>
                    <PivotItem headerText='Tenants'>
                        <Tenants />
                    </PivotItem>
                </Pivot>
            </Stack.Item>
        </Stack>
    );
};
