// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IModalProps } from '@aksio/cratis-fluentui';
import { Stack, TextField } from '@fluentui/react';
import React, { useState } from 'react';

export interface AddSubscriptionDialogOutput {
    id: string;
    name: string;
    tenantName: string;
}

export const AddSubscriptionDialog = (props: IModalProps<{}, AddSubscriptionDialogOutput>) => {
    const [name, setName] = useState('');
    const [tenantName, setTenantName] = useState('');
    const [id, setId] = useState('');

    props.onClose(result => {
        return {
            id,
            name,
            tenantName
        };
    });

    return (
        <div>
            <Stack>
                <TextField label='Id' required defaultValue={name} onChange={(e, n) => setId(n!)} />
                <TextField label='Name' required defaultValue={name} onChange={(e, n) => setName(n!)} />
                <TextField label='Tenant Name (Domain)' required defaultValue={name} onChange={(e, n) => setTenantName(n!)} />
            </Stack>
        </div>
    );
};
