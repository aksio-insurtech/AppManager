// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { TextField } from '@mui/material';
import React, { useState } from 'react';
import { IModalProps } from '@aksio/cratis-mui';


export interface CreateMicroserviceDialogInput {
    applicationId: string;
}

export interface CreateMicroserviceDialogOutput {
    applicationId: string;
    name: string;
}


export const CreateMicroserviceDialog = (props: IModalProps<CreateMicroserviceDialogInput, CreateMicroserviceDialogOutput>) => {
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
