// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IModalProps } from '@aksio/cratis-fluentui';
import { TextField } from '@fluentui/react';
import { useState } from 'react';

export interface CreateApplicationDialogOutput {
    name: string;
}

export const CreateApplicationDialog = (props: IModalProps<{}, CreateApplicationDialogOutput>) => {
    const [name, setName] = useState('');

    props.onClose(result => {
        return {
            name
        };
    });

    return (
        <div>
            <TextField label='Name' required defaultValue={name} onChange={(e, n) => setName(n!)} />
        </div>
    );
};
