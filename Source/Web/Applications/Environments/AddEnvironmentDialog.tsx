// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IModalProps } from '@aksio/cratis-mui';
import { Stack, TextField } from '@mui/material';
import { useState } from 'react';

export type AddEnvironmentDialogOutput = {
    name: string;
    displayName: string;
    shortDisplayName: string;
};

export const AddEnvironmentDialog = (props: IModalProps<{}, AddEnvironmentDialogOutput>) => {
    const [name, setName] = useState('');
    const [displayName, setDisplayName] = useState('');
    const [shortName, setShortName] = useState('');

    props.onClose(() => {
        return {
            name,
            displayName,
            shortDisplayName: shortName
        };
    });

    return (
        <Stack direction="column" width={400} spacing={1}>
            <TextField label='Name' fullWidth required defaultValue={name} onChange={e => setName(e.currentTarget.value)} />
            <TextField label='Display Name' fullWidth required defaultValue={displayName} onChange={e => setDisplayName(e.currentTarget.value)} />
            <TextField label='Short Name' fullWidth required defaultValue={shortName} onChange={e => setShortName(e.currentTarget.value)} />
        </Stack>
    );
};
