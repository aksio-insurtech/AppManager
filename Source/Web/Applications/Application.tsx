// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ScrollableDetailsList } from '@aksio/cratis-fluentui';
import { CommandBar, IColumn, ICommandBarItemProps, Pivot, PivotItem, Stack } from '@fluentui/react';
import { useParams } from 'react-router-dom';
import { ApplicationSettings } from './ApplicationSettings';
import { MicroserviceSettings } from './MicroserviceSettings';
import { Tenants } from './Tenants';
import { Application as ApplicationModel } from 'API/applications/Application';

const columns: IColumn[] = [
    {
        key: 'name',
        name: 'Name',
        fieldName: 'name',
        minWidth: 200
    }
];

export interface IApplicationProps {
    application: ApplicationModel;
}

export const Application = (props: IApplicationProps) => {


    const commandBarItems: ICommandBarItemProps[] = [
        {
            key: 'addMicroservice',
            name: 'Add Microservice',
            iconProps: { iconName: 'WebAppBuilderFragmentCreate' }
        }
    ];

    const items: any[] = [];

    return (
        <Stack style={{ height: '100%' }}>
            <Stack.Item>
                <CommandBar items={commandBarItems} />
                <ScrollableDetailsList columns={columns} items={items} />
            </Stack.Item>
            <Stack.Item grow={1}>
                <Pivot>
                    <PivotItem headerText='General'>
                        <ApplicationSettings application={props.application}/>
                    </PivotItem>
                    <PivotItem headerText='Tenants'>
                        <Tenants />
                    </PivotItem>
                    <PivotItem headerText='Production'>
                        <MicroserviceSettings />
                    </PivotItem>
                    <PivotItem headerText='Development'>
                        <MicroserviceSettings />
                    </PivotItem>
                </Pivot>
            </Stack.Item>
        </Stack>
    );
};
