// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IModalProps } from '@aksio/applications-mui';
import { InputLabel, MenuItem, Select, Stack, TextField } from '@mui/material';
import { useState } from 'react';
import { CertificatesForApplicationEnvironmentId } from 'API/applications/environments/CertificatesForApplicationEnvironmentId';


export interface AssociateDomainDialogInput {
    environmentId: string;
}

export type AssociateDomainDialogOutput = {
    domain: string;
    certificateId: string;
};

export const AssociateDomainDialog = (props: IModalProps<AssociateDomainDialogInput, AssociateDomainDialogOutput>) => {
    const [domain, setDomain] = useState('');
    const [certificateId, setCertificateId] = useState<string>('');

    const [certificatesForEnvironment] = CertificatesForApplicationEnvironmentId.use({ environmentId: props.input!.environmentId });
    const certificates = certificatesForEnvironment.data?.certificates ?? [];

    props.onClose(() => {
        return {
            domain,
            certificateId
        };
    });

    return (
        <Stack direction="column" width={400} spacing={1}>
            <TextField label='DomainName' fullWidth required defaultValue={domain} onChange={e => setDomain(e.currentTarget.value)} />

            <InputLabel>Certificate</InputLabel>
            <Select
                label="Certificate"
                placeholder="Select certificate"
                value={certificateId}
                onChange={(ev) => {
                    setCertificateId(ev.target.value);
                }}>
                {certificates.map(_ => (
                    <MenuItem key={_.certificateId} value={_.certificateId}>{_.name}</MenuItem>
                ))}
            </Select>
        </Stack>
    );
};
