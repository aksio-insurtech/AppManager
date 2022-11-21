// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IModalProps } from '@aksio/cratis-mui';
import { Stack, TextField } from '@mui/material';
import { useState } from 'react';

export type AddVariableDialogOutput = {
    key: string;
    value: string;
};

export const AddVariableDialog = (props: IModalProps<{}, AddVariableDialogOutput>) => {
    const [key, setKey] = useState('');
    const [value, setValue] = useState('');

    props.onClose(() => {
        return {
            key,
            value
        };
    });

    return (
        <Stack direction="column" width={400} spacing={1}>
            <TextField label='Key' fullWidth required defaultValue={key} onChange={e => setKey(e.currentTarget.value)} />
            <TextField label='Value' fullWidth required defaultValue={value} onChange={e => setValue(e.currentTarget.value)} />
        </Stack>
    );
};
