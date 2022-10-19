// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Stack, TextField } from '@mui/material';
import React, { useState } from 'react';
import { IModalProps } from '@aksio/cratis-mui';

export interface AddSubscriptionDialogOutput {
    id: string;
    name: string;
    tenantId: string;
    tenantName: string;
}

export const AddSubscriptionDialog = (props: IModalProps<{}, AddSubscriptionDialogOutput>) => {
    const [id, setId] = useState('');
    const [name, setName] = useState('');
    const [tenantName, setTenantName] = useState('');
    const [tenantId, setTenantId] = useState('');

    props.onClose(() => {
        return {
            id,
            name,
            tenantId,
            tenantName
        };
    });

    return (
        <Stack direction="column">
            <TextField label='Id' required defaultValue={id} onChange={(e) => setId(e.currentTarget.value)} />
            <TextField label='Name' required defaultValue={name} onChange={(e) => setName(e.currentTarget.value)} />
            <TextField label='Tenant Id' required defaultValue={tenantId} onChange={(e) => setTenantId(e.currentTarget.value)} />
            <TextField label='Tenant Name (Domain)' required defaultValue={tenantName} onChange={(e) => setTenantName(e.currentTarget.value)} />
        </Stack>
    );
};
