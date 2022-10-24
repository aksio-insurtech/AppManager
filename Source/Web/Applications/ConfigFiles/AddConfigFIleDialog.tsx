// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IModalProps } from '@aksio/cratis-mui';
import { Stack, TextField } from '@mui/material';
import { useState } from 'react';

export type AddConfigFileDialogOutput = {
    name: string;
    value: string;
};

export const AddConfigFileDialog = (props: IModalProps<{}, AddConfigFileDialogOutput>) => {
    const [name, setName] = useState('');
    const [value, setValue] = useState('');

    props.onClose(() => {
        return {
            name,
            value
        };
    });

    return (
        <Stack direction="column" width={400} spacing={1}>
            <TextField label='Name' fullWidth required defaultValue={name} onChange={e => setName(e.currentTarget.value)} />
            <TextField label='Value' fullWidth required defaultValue={value} onChange={e => setValue(e.currentTarget.value)} />
        </Stack>
    );
};
