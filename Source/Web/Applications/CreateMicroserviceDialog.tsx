// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IModalProps } from '@aksio/cratis-fluentui';
import { Stack, TextField } from '@fluentui/react';
import React, { useState } from 'react';


export interface CreateMicroserviceDialogOutput {
    name: string;
}


export const CreateMicroserviceDialog = (props: IModalProps<{}, CreateMicroserviceDialogOutput>) => {
    const [name, setName] = useState('');

    props.onClose(result => {
        return {
            name
        };
    });

    return (
        <div>
            <Stack>
                <TextField label='Name' required defaultValue={name} onChange={(e, n) => setName(n!)} />
            </Stack>
        </div>
    );
}
