// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useState } from 'react';
import { TextField, Stack, MenuItem, Select } from '@mui/material';
import { IModalProps } from '@aksio/applications-mui';
import { AllSettings } from 'API/settings/AllSettings';

export interface CreateApplicationDialogOutput {
    name: string;
    sharedAzureSubscriptionId: string;
}

export const CreateApplicationDialog = (props: IModalProps<{}, CreateApplicationDialogOutput>) => {
    const [name, setName] = useState('');
    const [sharedAzureSubscriptionId, setSharedAzureSubscriptionId] = useState('');
    const [settings] = AllSettings.use();

    props.onClose(() => {
        return {
            name,
            sharedAzureSubscriptionId
        };
    });

    const subscriptionMenuItems = settings.data.azureSubscriptions?.map(_ => (
        <MenuItem key={_.subscriptionId} value={_.subscriptionId}>{_.name}</MenuItem>
    ));

    return (
        <Stack direction="column" width={400} spacing={1}>
            <TextField label='Name' fullWidth required defaultValue={name} onChange={e => setName(e.currentTarget.value)} />
            <Select
                label="Shared Azure Subscription"
                placeholder="Select the Shared Azure Subscription"
                value={sharedAzureSubscriptionId}
                onChange={(ev) => {
                    setSharedAzureSubscriptionId(ev.target.value);
                }}>
                {subscriptionMenuItems}
            </Select>

        </Stack>
    );
};
