// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { TextField } from '@mui/material';
import React, { useState } from 'react';

export interface AddSubscriptionDialogOutput {
    id: string;
    name: string;
    tenantName: string;
}

export const AddSubscriptionDialog = (props: AddSubscriptionDialogOutput) => {
    const [name, setName] = useState('');
    const [tenantName, setTenantName] = useState('');
    const [id, setId] = useState('');

    return (
        <div>
            <TextField label='Id' required defaultValue={name} onChange={(e) => setId(e.currentTarget.value)} />
            <TextField label='Name' required defaultValue={name} onChange={(e) => setName(e.currentTarget.value)} />
            <TextField label='Tenant Name (Domain)' required defaultValue={name} onChange={(e) => setTenantName(e.currentTarget.value)} />
        </div>
    );
};
