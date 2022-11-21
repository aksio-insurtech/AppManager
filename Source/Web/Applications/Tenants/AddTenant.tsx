// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IModalProps } from '@aksio/cratis-mui';
import { Stack, TextField } from '@mui/material';
import { useState } from 'react';

export type AddTenantDialogOutput = {
    name: string;
    shortName: string;
};

export const AddTenantDialog = (props: IModalProps<{}, AddTenantDialogOutput>) => {
    const [name, setName] = useState('');
    const [shortName, setShortName] = useState('');
    const [domainName, setDomainName] = useState('');

    props.onClose(() => {
        return {
            name,
            shortName
        };
    });

    return (
        <Stack direction="column" width={400} spacing={1}>
            <TextField label='Name' fullWidth required defaultValue={name} onChange={e => setName(e.currentTarget.value)} />
        </Stack>
    );
};
