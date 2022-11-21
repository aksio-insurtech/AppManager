// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IModalProps } from '@aksio/cratis-mui';
import { Button, InputLabel, Stack, TextField } from '@mui/material';
import { ChangeEvent, useState } from 'react';
import * as icons from '@mui/icons-material';

export type AdCertificateDialogOutput = {
    name: string;
    certificate: string;
};

export const AddCertificateDialog = (props: IModalProps<{}, AdCertificateDialogOutput>) => {
    const [name, setName] = useState('');
    const [certificate, setCertificate] = useState<string>('');

    props.onClose(() => {
        return {
            name: name,
            certificate
        };
    });

    const handleCapture = ({ target }: ChangeEvent<HTMLInputElement>) => {
        const fileReader = new FileReader();
        fileReader.readAsBinaryString(target.files![0]);
        fileReader.onload = () => {
            setCertificate(fileReader.result! as string);
        };
    };

    return (
        <Stack direction="column" width={400} spacing={1}>
            <TextField label='Name' fullWidth required defaultValue={name} onChange={e => setName(e.currentTarget.value)} />

            <input
                id="file-upload"
                type="file"
                style={{ display: 'none' }}
                onChange={handleCapture}
            />

            <label htmlFor="file-upload">
                <InputLabel>Upload certificate</InputLabel>
                <Button variant="contained" component="span" size="large">
                    <icons.FileUpload />
                </Button>
            </label>
        </Stack>
    );
};
