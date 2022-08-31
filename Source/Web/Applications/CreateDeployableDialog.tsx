// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { TextField } from '@mui/material';
import React, { useState } from 'react';


export interface CreateDeployableDialogOutput {
    name: string;
}


export const CreateDeployableDialog = (props: CreateDeployableDialogOutput) => {
    const [name, setName] = useState('');

    return (
        <div>
            <TextField label='Name' required defaultValue={name} onChange={(e) => setName(e.currentTarget.value)} />
        </div>
    );
};
