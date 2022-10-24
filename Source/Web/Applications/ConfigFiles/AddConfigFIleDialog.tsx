// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IModalProps } from '@aksio/cratis-mui';
import { Stack, TextField } from '@mui/material';
import { useEffect, useState } from 'react';

import Editor, { useMonaco } from '@monaco-editor/react';


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
        <Stack direction="column" width={500} spacing={1}>
            <TextField label='Name' fullWidth required defaultValue={name} onChange={e => setName(e.currentTarget.value)} />
            <Editor
                height="45vh"
                theme="vs-dark"
                defaultValue="{}"
                language="JSON"
                defaultLanguage="JSON"
                onChange={(json) => setValue(json!)}
            />
        </Stack>
    );
};
