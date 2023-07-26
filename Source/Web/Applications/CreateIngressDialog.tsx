// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { TextField } from '@mui/material';
import React, { useState } from 'react';
import { IModalProps } from '@aksio/applications-mui';


export interface CreateIngressDialogInput {
    applicationId: string;
}

export interface CreateIngressDialogOutput {
    applicationId: string;
    name: string;
}

export const CreateIngressDialog = (props: IModalProps<CreateIngressDialogInput, CreateIngressDialogOutput>) => {
    const [name, setName] = useState('');

    props.onClose(() => {
        return {
            applicationId: props.input!.applicationId,
            name
        };
    });

    return (
        <div>
            <TextField label='Name' required defaultValue={name} onChange={(e) => setName(e.currentTarget.value)} />
        </div>
    );
};
