// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Pivot, PivotItem, Stack } from '@fluentui/react';
import { useParams } from 'react-router-dom';
import { GetMicroservice } from 'API/applications/microservices/GetMicroservice';
import { General } from './General';

export const Microservice = () => {
    const { microserviceId } = useParams();
    const [microservice, performMicroserviceQuery] = GetMicroservice.use({microserviceId: microserviceId!});

    return (
        <Stack style={{ height: '100%' }}>
            <Stack.Item>
                <Pivot>
                    <PivotItem headerText="General" itemIcon="Settings">
                        <General microservice={microservice.data}/>
                    </PivotItem>
                    <PivotItem headerText="Environment" itemIcon="Variable">
                    </PivotItem>
                    <PivotItem headerText="Secrets" itemIcon="Encryption">
                    </PivotItem>
                </Pivot>
            </Stack.Item>
        </Stack>
    );
};
