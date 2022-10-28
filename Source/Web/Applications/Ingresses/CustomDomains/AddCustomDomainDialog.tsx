// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IModalProps } from '@aksio/cratis-mui';
import { InputLabel, MenuItem, Select, Stack, TextField } from '@mui/material';
import { useState } from 'react';
import { CertificatesForApplicationEnvironmentId } from 'API/applications/environments/CertificatesForApplicationEnvironmentId';


export interface AddCustomDomainDialogInput {
    environmentId: string;
}

export type AddCustomDomainDialogOutput = {
    domain: string;
    certificateId: string;
};

export const AddCustomDomainDialog = (props: IModalProps<AddCustomDomainDialogInput, AddCustomDomainDialogOutput>) => {
    const [domain, setDomain] = useState('');
    const [certificateId, setCertificateId] = useState<string>('');

    const [certificatesForEnvironment] = CertificatesForApplicationEnvironmentId.use({ environmentId: props.input!.environmentId });
    const certificates = certificatesForEnvironment.data?.certificates ?? [];

    console.log(certificatesForEnvironment.data);

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
