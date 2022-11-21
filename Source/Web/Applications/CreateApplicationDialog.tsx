// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useState } from 'react';
import { TextField, Stack } from '@mui/material';
import { IModalProps } from '@aksio/cratis-mui';

export interface CreateApplicationDialogOutput {
    name: string;
}

export const CreateApplicationDialog = (props: IModalProps<{}, CreateApplicationDialogOutput>) => {
    const [name, setName] = useState('');

    props.onClose(() => {
        return {
            name
        };
    });

    return (
        <Stack direction="column" width={400} spacing={1}>
            <TextField label='Name' fullWidth required defaultValue={name} onChange={e => setName(e.currentTarget.value)} />
        </Stack>
    );
};
