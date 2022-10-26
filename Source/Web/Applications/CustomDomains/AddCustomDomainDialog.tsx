// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IModalProps } from '@aksio/cratis-mui';
import { Button, InputLabel, Stack, TextField } from '@mui/material';
import { ChangeEvent, useState } from 'react';
import * as icons from '@mui/icons-material';

export type AddCustomDomainDialogOutput = {
    domain: string;
    certificate: string;
};

export const AddCustomDomainDialog = (props: IModalProps<{}, AddCustomDomainDialogOutput>) => {
    const [domain, setDomain] = useState('');
    const [certificate, setCertificate] = useState<string>('');

    props.onClose(() => {
        return {
            domain: domain,
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
            <TextField label='DomainName' fullWidth required defaultValue={domain} onChange={e => setDomain(e.currentTarget.value)} />

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
