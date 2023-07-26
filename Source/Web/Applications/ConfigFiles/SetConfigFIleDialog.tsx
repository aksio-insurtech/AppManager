// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IModalProps } from '@aksio/applications-mui';
import { Stack, TextField } from '@mui/material';
import { useState } from 'react';

import Editor, { useMonaco } from '@monaco-editor/react';
import { ConfigFile } from 'API/applications/environments/microservices/ConfigFile';


export type AddConfigFileDialogOutput = {
    name: string;
    content: string;
};

export const SetConfigFileDialog = (props: IModalProps<ConfigFile, AddConfigFileDialogOutput>) => {
    const [name, setName] = useState(props.input?.name || '');
    const [content, setContent] = useState(props.input?.content || '{}');

    props.onClose(() => {
        return {
            name,
            content
        };
    });

    return (
        <Stack direction="column" width={500} spacing={1}>
            <TextField label='Name' fullWidth required defaultValue={name} onChange={e => setName(e.currentTarget.value)} />
            <Editor
                height="45vh"
                theme="vs-dark"
                defaultValue="{}"
                value={content}
                language="JSON"
                defaultLanguage="JSON"
                onChange={(json) => setContent(json!)}
            />
        </Stack>
    );
};
