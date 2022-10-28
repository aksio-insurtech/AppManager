// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IModalProps } from '@aksio/cratis-mui';
import { Stack, TextField } from '@mui/material';
import { useState } from 'react';

export type AddCustomDomainDialogOutput = {
    domain: string;
    certificateId: string;
};

export const AddCustomDomainDialog = (props: IModalProps<{}, AddCustomDomainDialogOutput>) => {
    const [domain, setDomain] = useState('');
    const [certificate, setCertificate] = useState<string>('');

    props.onClose(() => {
        return {
            domain: domain,
            certificateId: ''
        };
    });

    return (
        <Stack direction="column" width={400} spacing={1}>
            <TextField label='DomainName' fullWidth required defaultValue={domain} onChange={e => setDomain(e.currentTarget.value)} />
        </Stack>
    );
};
