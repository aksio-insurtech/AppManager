// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Stack, TextField } from '@mui/material';
import React, { useState } from 'react';
import { IModalProps } from '@aksio/applications-mui';

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
        <Stack direction="column" width={400} spacing={1}>
            <TextField label='Id' fullWidth required defaultValue={id} onChange={(e) => setId(e.currentTarget.value)} />
            <TextField label='Name' fullWidth required defaultValue={name} onChange={(e) => setName(e.currentTarget.value)} />
            <TextField label='Tenant Id' fullWidth required defaultValue={tenantId} onChange={(e) => setTenantId(e.currentTarget.value)} />
            <TextField label='Tenant Name (Domain)' fullWidth required defaultValue={tenantName} onChange={(e) => setTenantName(e.currentTarget.value)} />
        </Stack>
    );
};
